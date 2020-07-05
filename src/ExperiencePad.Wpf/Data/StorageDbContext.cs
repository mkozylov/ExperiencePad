using Dapper;
using Microsoft.Data.Sqlite;
using NWrath.Synergy.Common.Extensions;
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

        public void DeleteRecord(Record record)
        {
            using var connection = CreateConnection();

            var categoryRecords = GetCategoryRecords(record.CategoryId)
                                     .Where(x => x.Id != record.Id)
                                     .OrderBy(x => x.Order)
                                     .Select((x, i) => 
                                     {
                                         x.Order = i + 1;
                                         return x;
                                     })
                                     .ToList();

            foreach (var rec in categoryRecords)
            {
                UpdateRecordOrderInternal(connection, rec, rec.Order);
            }

            connection.Execute($"delete from {Record.TableName} where Id = @Id", record);
        }

        public void UpdateRecordOrder(Record record, int newOrder)
        {
            using var connection = CreateConnection();

            UpdateRecordOrderInternal(connection, record, newOrder);
        }

        public void UpdateRecordData(Record record)
        {
            using var connection = CreateConnection();

            connection.Execute($"update {Record.TableName} set "
                               + "CategoryId = @CategoryId"
                               + ", Body = @Body"
                               + ", Title = @Title"
                               + ", Type = @Type"
                               + " where Id = @Id",
                               record
                               );
        }

        public void AddRecord(Record record)
        {
            using var connection = CreateConnection();

            connection.Execute($"insert into {Record.TableName}(Id, CategoryId, Title, Body, [Order], CreateDate, Type) values"
                               + $"(@Id, @CategoryId, @Title, @Body, @Order, @CreateDate, @Type)",
                               record
                               );
        }

        public IEnumerable<Record> GetCategoryRecords(Guid? categoryId)
        {
            var filter = $"CategoryId {(categoryId.HasValue ? $"= '{categoryId.Value}'" : "is null")}";

            return QueryRecords(filter);
        }

        public int GetCategoryRecordsCount(Guid? categoryId)
        {
            using var connection = CreateConnection();

            var count = connection.Query<int>($"select count(1) from {Record.TableName} where CategoryId = @CategoryId",
                                      new { CategoryId = categoryId }
                                      )
                                  .FirstOrDefault();

            return count;
        }

        public void DeleteCategory(Category category)
        {
            var flatten = FlattenCategoryTree(new[] { category });
            var categoryStack = new Stack<Category>(flatten);
            var categoryIds = categoryStack.Select(x => x.Id).ToArray();

            using var connection = CreateConnection();

            var categories = GetCategoryChildrens(category.ParentId)
                                 .Where(x => x.Id != category.Id)
                                 .OrderBy(x => x.Order)
                                 .Select((x, i) =>
                                 {
                                     x.Order = i + 1;
                                     return x;
                                 })
                                 .ToList();

            foreach (var cat in categories)
            {
                UpdateCategoryOrderInternal(connection, cat, cat.Order);
            }

            connection.Execute(
                $"delete from {Record.TableName} where CategoryId in ({categoryIds.ToSql()})"
                );

            connection.Execute(
                $"delete from {Category.TableName} where Id in ({categoryIds.ToSql()})"
                );      
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

        public IEnumerable<Category> GetCategoryChildrens(Guid? categoryId)
        {
            using var connection = CreateConnection();

            var children = connection.Query<Category>($"select * from {Category.TableName} where ParentId = @ParentId",
                                new { ParentId = categoryId }
                                );

            return children;
        }

        public void RenameCategory(Category category, string newName)
        {
            category.Name = newName;

            using var connection = CreateConnection();

            connection.Execute($"update {Category.TableName} set Name = @Name where Id = @Id",
                               category
                               );
        }

        public void AddCategory(Category category)
        {
            using var connection = CreateConnection();

            connection.Execute($"insert into {Category.TableName}(Id, ParentId, Name, CreateDate, [Order]) values"
                               + $"(@Id, @ParentId, @Name, @CreateDate, @Order)",
                               category
                               );
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

        private IEnumerable<Category> FlattenCategoryTree(IEnumerable<Category> collection)
        {
            var flattenChildrens = collection.SelectMany(c => FlattenCategoryTree(c.Children));

            return collection.Concat(flattenChildrens);
        }

        private void UpdateRecordOrderInternal(SqliteConnection connection, Record record, int newOrder)
        {
            record.Order = newOrder;

            connection.Execute($"update {Record.TableName} set "
                               + "[Order] = @Order"
                               + " where Id = @Id",
                               record
                               );
        }

        private void UpdateCategoryOrderInternal(SqliteConnection connection, Category category, int newOrder)
        {
            category.Order = newOrder;

            connection.Execute($"update {Category.TableName} set "
                               + "[Order] = @Order"
                               + " where Id = @Id",
                               category
                               );
        }

        #endregion
    }
}
