using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ExperiencePad.Data
{
    public class StorageDbContext
    {
        private string _connectionString;

        public StorageDbContext(string connectionString)
        {
            _connectionString = connectionString;

            SqlMapper.AddTypeHandler(new GuidDbTypeHandler());
            SqlMapper.AddTypeHandler(new NullableGuidDbTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
        }

        public int GetCategoryChildrenCount(Guid? categoryId)
        {
            using var connection = CreateConnection();

            var count = connection.Query<int>($"select count(1) from {Category.TableName} where ParentId = @ParentId", 
                                      new { ParentId = categoryId}
                                      )
                                  .FirstOrDefault();

            return count;
        }

        public void AddCategory(Category category)
        {
            using var connection = CreateConnection();

            connection.Execute($"insert into {Category.TableName}(Id, ParentId, Name, CreateDate, [Order]) values"
                               + $"(@Id, @ParentId, @Name, @CreateDate, @Order)",
                               category
                               );
        }                      

        public Category FindCategory(string filter = null)
        {
            var where = filter == null ? "" : $"where {filter}";

            using var connection = CreateConnection();

            var category = connection.Query<Category>($"select top 1 * from {Category.TableName} {where}")
                                     .FirstOrDefault();

            return category;
        }

        public Record FindRecord(string filter = null)
        {
            var where = filter == null ? "" : $"where {filter}";

            using var connection = CreateConnection();

            var record = connection.Query<Record>($"select top 1 * from {Record.TableName} {where}")
                                   .FirstOrDefault();

            return record;
        }

        public IEnumerable<Category> QueryCategories(string filter = null)
        {
            var where = filter == null ? "" : $"where {filter}";

            using var connection = CreateConnection();

            var categories = connection.Query<Category>($"select * from {Category.TableName} {where}");

            return categories;
        }

        public IEnumerable<Record> QueryRecords(string filter = null)
        {
            var where = filter == null ? "" : $"where {filter}";

            using var connection = CreateConnection();

            var records = connection.Query<Record>($"select * from {Record.TableName} {where}");

            return records;
        }

        #region Internal

        private SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);

            connection.Open();

            return connection;
        }

        #endregion
    }
}
