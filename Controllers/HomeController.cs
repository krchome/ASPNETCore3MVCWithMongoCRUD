using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using MongoDb.ASP.NETCore3CRUDSample.DataAccess.Models;
using MongoDb.ASP.NETCore3CRUDSample.DataAccess.Repositories;
using MongoDb.ASP.NETCore3CRUDSample.DataAccess;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace MongoDb.ASP.NETCore3CRUDSample.Controllers
{
    public class HomeController : Controller
    {

        // private readonly IMongoCollection<Customer> collection;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerContext customerContext;
        private IMongoCollection<Customer> collection;
        public IConfiguration Configuration { get; }
        public HomeController(ICustomerRepository customerRepository)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase mongoDatabase = client.GetDatabase("CustomerDatabase");
            this.collection = mongoDatabase.GetCollection<Customer>("Customers");
            this._customerRepository = customerRepository;
            
        }
        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // return new ObjectResult(await _customerRepository.GetAllCustomers());
            var model = await _customerRepository.GetAllCustomers();
            return View(model);
        }

        // GET: api/Customer/name
        [HttpGet("{name}", Name = "Get")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomer(id);

            if (customer == null)
                return new NotFoundResult();

            return new ObjectResult(customer);
        }

        // GET: api/Customer
        [HttpGet]
        public  IActionResult Insert()
        {
           // await _customerRepository.Create(customer);
            //return new OkObjectResult(customer);
            return View(new Customer());
        }
        // POST: api/Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert([Bind(include: "CustomerID,Name,Age,Salary")]Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerRepository.Create(customer);
                //return new OkObjectResult(customer);
                ViewBag.Message = "Customer Added Successfully";
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        //GET: Customer/Update/5
        public  ActionResult Update(string id)
        {

            ObjectId oId = new ObjectId(id);
            Customer customer = collection.Find(c => c.Id == oId).FirstOrDefault();
            return View(customer);
        }


        //PUT: api/Customer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update( [Bind(include: "CustomerID,Name,Age,Salary")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var customerFromDb = await _customerRepository.GetCustomer(customer.CustomerID);

                //var customerFromDb = await _customerRepository.GetCustomer(name);

                if (customerFromDb == null)
                    return new NotFoundResult();

                customer.Id = customerFromDb.Id;
                await _customerRepository.Update(customer);
                return View(customer);
            }

            return View("Error");
            //return new OkObjectResult(customer);
           
        }

        public IActionResult ConfirmDelete(string id)
        {
            ObjectId objectId = new ObjectId(id);
            Customer customer = collection.Find(y => y.Id == objectId).FirstOrDefault();
            return View(customer);
        }

        // DELETE: api/ApiWithActions/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            // var customerFromDb = await _customerRepository.GetCustomer(customer.CustomerID);
            ObjectId objectId = new ObjectId(id);
            Customer customer = collection.Find(y => y.Id == objectId).FirstOrDefault();

            if (customer == null)
                return new NotFoundResult();

            await _customerRepository.Delete(customer.CustomerID);

            return RedirectToAction("Index");
        }














        //public IActionResult Index()
        //{
        //    //var model = collection.Find(FilterDefinition<Customer>.Empty).ToList();
        //    // return View(model);
        //    var model = _customerRepository.Get();
        //    return View(model);
        //}

        //public IActionResult Insert()
        //{
        //    return View(new Customer());
        //}

        //[HttpPost]
        //public IActionResult Insert(Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = _customerRepository.Create(customer);
        //        if(result > 0)
        //        {
        //            ViewBag.Message = "Cusomer added successfully";
        //            return View();
        //        }

        //      //collection.InsertOne(customer);
        //    }
        //    return View("Error");

        //}

        /// See that id is passed as string (and not an int as in the model)but
        /// it creates a new ObjectId type by the MongoDb.BSon. So ids are treated
        /// as strings even though passed as int in the console commands (or in the Customer model)
        //public IActionResult Update(string id)
        //{
        //    //ObjectId objectId = new ObjectId(id);
        //    //Customer customer = collection.Find(x => x.Id == objectId).FirstOrDefault();
        //    //return View(customer);
        //    var customer = _customerRepository.Find(id);
        //    return View(customer);
        //}

        //[HttpPost]
        //public IActionResult Update( Customer customer)
        //{
        //    //customer.Id = new ObjectId(id);
        //    //var filter = Builders<Customer>.Filter.Eq("Id", customer.Id);
        //    //var updateDef = Builders<Customer>.Update.Set("Name", customer.Name);
        //    //updateDef = updateDef.Set("Age", customer.Age);
        //    //updateDef = updateDef.Set("Salary", customer.Salary);
        //    //try
        //    //{
        //    //    if (ModelState.IsValid)
        //    //    {
        //    //        var updateResult = collection.UpdateOne(filter, updateDef);

        //    //        if (updateResult.IsAcknowledged)
        //    //        {
        //    //            ViewBag.Message = "Customer updated successfully!";
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    //  ViewBag.Message = ex.Message.ToString();
        //    //    ViewBag.Message = "Error while updating customer";
        //    //}
        //    //return View(customer);
        //    bool result = _customerRepository.Update(customer);
        //    if(result)
        //    {
        //        return View(customer);
        //    }
        //    return View("Error");

        //}

        //public IActionResult ConfirmDelete(string id)
        //{
        //    //ObjectId objectId = new ObjectId(id);
        //    //Customer customer = collection.Find(y => y.Id == objectId).FirstOrDefault();
        //    //return View(customer);
        //    var customer = _customerRepository.Find(id);

        //    return View(customer);
        //}

        //[HttpPost]
        //public IActionResult Delete(string id)
        //{
        //    var deleted = _customerRepository.Delete(id);
        //    if(deleted)
        //    {
        //        TempData["Message"] = "Customer deleted successfully";

        //    }
        //    else
        //    {
        //        TempData["Message"] = "Error while deleting customer";
        //    }
        //    return RedirectToAction("Index");
        //}
        //public IActionResult Delete(string id, Customer customer)
        //{
        //    ObjectId objectId = new ObjectId(id);
        //    var result = collection.DeleteOne<Customer>(c => c.Id == objectId);
        //    if(result.IsAcknowledged)
        //    {
        //        TempData["Message"] = "Customer deleted successfully";
        //    }
        //    else
        //    {
        //        TempData["Message"] = "Error while deleting customer";
        //    }
        //    return RedirectToAction("Index");
        //}

    }
}
