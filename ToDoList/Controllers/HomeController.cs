using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Bson;
using ToDoList.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
//            MongoCRUD db = new MongoCRUD("ToDo");
//            db.InsertRecord("ToDoItems", new ToDoItemsModel{ WorkItemDescription = "Learn Mongo DB"});

            MongoCRUD db = new MongoCRUD("ToDo");
            var recs = db.LoadRecords<ToDoItemsModel>("ToDoItems");

            foreach(var rec in recs)
            {
                Console.WriteLine($"{rec.Id}: {rec.WorkItemDescription}");
            }

            // IMongoDatabase database;
            // var client = new MongoClient();
            // database = client.GetDatabase("something");
            // var dogs = database.GetCollection<BsonDocument>("dogs");

            // foreach(var dog in dogs.AsQueryable())
            // {
            //     var test = dog;
            // }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ToDoItemsModel
    {
        [BsonId]
        public Guid Id{get;set;}
        public string WorkItemDescription{get;set;}
    }

    public class MongoCRUD
    {
        private IMongoDatabase db;

        public MongoCRUD(string databaseName)
        {
            var client = new MongoClient();
            db = client.GetDatabase(databaseName);
        }

        public void InsertRecord<T>(string tableName, T record)
        {
            var collection = db.GetCollection<T>(tableName);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string tableName)
        {
            var collection = db.GetCollection<T>(tableName);

            return collection.Find(new BsonDocument()).ToList();
        }
    }

}
