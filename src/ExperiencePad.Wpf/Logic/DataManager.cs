using ExperiencePad.Data;
using NWrath.Synergy.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ExperiencePad.Logic
{
    public class DataManager
    {
        private StorageDbContext _storageDb;

        public DataManager(StorageDbContext storageDb)
        {
            _storageDb = storageDb;
        }

        public IEnumerable<RecordViewModel> GetRecords(IEnumerable<Guid> ids)
        {
            return _storageDb.QueryRecords($"Id in ({ids.ToSql()})")
                             .DeepMap<RecordViewModel[]>();
        }

        public void DeleteRecord(RecordViewModel record)
        {
            _storageDb.DeleteRecord(record);
        }

        public void UpdateRecordData(RecordViewModel record)
        {
            _storageDb.UpdateRecordData(record);
        }

        public void AddRecord(RecordViewModel record)
        {
            record.Id = record.Id == Guid.Empty ? Guid.NewGuid() : record.Id;
            record.CreateDate = record.CreateDate == DateTime.MinValue ? DateTime.Now : record.CreateDate;
            record.Title = record.Title.IsEmpty() ? "Новая запись" : record.Title;
            record.Type = record.Type.IsEmpty() ? "text" : record.Type;
            record.Order = record.Order == 0
                           ? (_storageDb.GetCategoryRecordsCount(record.CategoryId) + 1)
                           : record.Order;

            _storageDb.AddRecord(record);
        }

        public void DeleteCategory(CategoryViewModel category)
        {
            _storageDb.DeleteCategory(category);
        }

        public void RenameCategory(CategoryViewModel category, string newName)
        {
            newName = newName.IsEmpty() ? category.Name : newName;

            _storageDb.RenameCategory(category, newName);
        }

        public void AddCategory(CategoryViewModel category)
        {
            category.Id = category.Id == Guid.Empty ? Guid.NewGuid() : category.Id;
            category.CreateDate = category.CreateDate == DateTime.MinValue ? DateTime.Now : category.CreateDate;
            category.Name = category.Name.IsEmpty() ? "Новая категория" : category.Name;
            category.Order = category.Order == 0 
                             ? (_storageDb.GetCategoryChildrenCount(category.ParentId) + 1) 
                             : category.Order;

            _storageDb.AddCategory(category);
        }

        public IEnumerable<CategoryViewModel> GetCategoryTree()
        {
            var categories = _storageDb.QueryCategories()
                                       .DeepMap<CategoryViewModel[]>()
                                       .OrderBy(x => x.ParentId)
                                       .ThenBy(x => x.Order)
                                       .ToArray();

            var tree = GenerateCategoryTree(categories);

            return tree;
        }

        public IEnumerable<RecordViewModel> GetCategoryRecords(Guid? categoryId)
        {
            var filter = $"CategoryId {(categoryId.HasValue ? $"= '{categoryId.Value}'" : "is null")}";

            var records = _storageDb.GetCategoryRecords(categoryId)
                                    .OrderBy(x => x.Order)
                                    .DeepMap<RecordViewModel[]>();

            return records;
        }

        public IEnumerable<CategoryViewModel> GetCategories(IEnumerable<Guid> ids)
        {
            return _storageDb.QueryCategories($"Id in ({ids.ToSql()})")
                             .DeepMap<CategoryViewModel[]>();
        }

        #region Internal

        private IEnumerable<CategoryViewModel> GenerateCategoryTree(
            IEnumerable<CategoryViewModel> collection,
            CategoryViewModel parent = null)
        {
            return collection.Where(x => x.ParentId == parent?.Id)
                             .Select(x =>
                             {
                                 var childrenTree = GenerateCategoryTree(collection, x);

                                 x.Children = new ObservableCollection<CategoryViewModel>(childrenTree);
                                 x.Parent = parent;

                                 return x;
                             });
        }

        #endregion
    }
}
