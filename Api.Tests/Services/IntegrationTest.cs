using MongoDB.Driver;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Tests.Services
{
    public class IntegrationTest
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public IntegrationTest()
        {
            var connectionString = "mongodb://admin:admin@localhost:27017";
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("MotorcycleDeliveryRentalBD");
        }

        [Fact(DisplayName = "MogoDB: Integration Test")]
        public void MongoDBIntegrationTest()
        {
            var collectionNames = _database.ListCollectionNames().ToList();

            Assert.NotNull(collectionNames);
            Assert.True(collectionNames.Count > 0);
        }

        [Fact(DisplayName = "RabbitMQ: Integration Test")]
        public void TestRabbitMQIntegrationTest()
        {

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };

            using (var connection = factory.CreateConnection())
            {
                Assert.True(connection.IsOpen);
            }
        }
    }
}
