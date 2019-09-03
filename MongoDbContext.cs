using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using MongoDb.ASP.NETCore3CRUDSample;

namespace MongoDb.ASP.NETCore3CRUDSample
{
    public class MongoDbContext
    {
        public MongoClient mongoClient { get; set; }
        public IMongoDatabase mongoDatabase { get; set; }

        public MongoDbContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            mongoDatabase = client.GetDatabase(options.Value.Database);
        }
        //public MongoDbContext(string connStr)
        //{
        //    mongoClient = new MongoClient(connStr);
        //    mongoDatabase = mongoClient.GetDatabase("CustomerDatabase");
        //}

        public IMongoCollection<Customer> DbSet<Customer>()
        {
            var table = typeof(Customer).GetCustomAttribute<TableAttribute>(false).Name;
            return mongoDatabase.GetCollection<Customer>(table);
        }










    }
}
