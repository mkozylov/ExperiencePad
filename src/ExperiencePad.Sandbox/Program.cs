using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ExperiencePad.Data;
using static Dapper.SqlMapper;
using System.Reflection;
using ExperiencePad.Logic;

namespace ExperiencePad.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbFileName = "storage.db";

            if (File.Exists(dbFileName))
            {
                File.Delete(dbFileName);
                Task.Delay(10).Wait();
            }

            using var connection = new SqliteConnection($"Data Source={dbFileName}");

            connection.Open();

            connection.Execute($"create virtual table if not exists Categories "
                             + $"using fts5(Id, ParentId, Name, CreateDate, Order)");

            connection.Execute($"create virtual table if not exists Records "
                             + $"using fts5(Id, CategoryId, Title, Body, Order, CreateDate, Type)");

            var cId1 = Guid.NewGuid();
            var cSubId1 = Guid.NewGuid();
            var cSubId2 = Guid.NewGuid();
            var cId2 = Guid.NewGuid();
            var cId3 = Guid.NewGuid();

            var rId1 = Guid.NewGuid();
            var rId2 = Guid.NewGuid();
            var rId3 = Guid.NewGuid();

            var now = DateTime.Now;

            connection.Execute("insert into Categories(Id, ParentId, Name, CreateDate, [Order]) values"
                             + $"('{cId1}', null,'Категория 1', '{now:o}', 0)"
                                 + $", ('{cSubId1}', '{cId1}','Подкатегория 1', '{now.AddSeconds(1):o}', 0)"
                                 + $", ('{cSubId2}', '{cId1}','Подкатегория 2', '{now.AddSeconds(2):o}', 0)"
                             + $", ('{cId2}', null,'Категория 2', '{now.AddSeconds(3):o}', 0)"
                             + $", ('{cId3}', null,'Категория 3', '{now.AddSeconds(4):o}', 0)"
                             );

            connection.Execute("insert into Records(Id, CategoryId, Title, Body, [Order], CreateDate, Type) values"
                             + $"('{rId1}', '{cId1}','Заголовок 1', 'Тело 1', 0, '{now:o}', 'c#')"
                             + $", ('{rId2}', '{cId1}','Заголовок 2', 'Тело 2', 0, '{now.AddSeconds(3):o}', 'xml')"
                             + $", ('{rId3}', '{cId1}','Заголовок 3', 'Тело 3', 0, '{now.AddSeconds(4):o}', 'custom')"
                             );

            //for (int i = 4; i < 1001; i++)
            //{
            //    connection.Execute("insert into Categories(Id, ParentId, Name, CreateDate, [Order]) values"
            //              + $"('{Guid.NewGuid()}', null,'Категория {i}', '{now:o}', 0)"
            //              );
            //}

            //var lastRowId = connection.Query<int?>("select last_insert_rowid()").FirstOrDefault();
            //"select last_insert_rowid();";//"select rowid, * from Data where rowid = 2";

            // Console.WriteLine($"{lastRowId}");

            //var dbStorage = new StorageDbContext($"Data Source={dbFileName}");
            //var logic = new AppLogic(dbStorage);
            //var tree = logic.GetCategoryTree().ToArray();
            //var tree2 = dbStorage.QueryCategories().GenerateTree().ToArray();

            //using var connection2 = new SqliteConnection($"Data Source={dbFileName}");
            //connection2.Open();
            //using var cmd = connection2.CreateCommand();
            //cmd.CommandText = "select * from Categories";
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var vals = new object[6];
            //    reader.GetValues(vals);

            //    Console.WriteLine(JsonConvert.SerializeObject(vals, Formatting.Indented));
            //}

            Console.ReadKey();
        }
    }

    internal static class GenericHelpers
    {
        public static IEnumerable<Category> GenerateTree(
            this IEnumerable<Category> collection,
            Category parent = null)
        {
            return collection.Where(x => x.ParentId == parent?.Id)
                             .Select(x =>
                             {
                                 x.Children = collection.GenerateTree(x).ToList();
                                 x.Parent = parent;
                                 return x;
                             });
        }
    }
}
