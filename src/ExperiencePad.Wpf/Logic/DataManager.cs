using ExperiencePad.Data;
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

        public void AddCategory(CategoryViewModel category)
        {
            category.Id = category.Id == Guid.Empty ? Guid.NewGuid() : category.Id;
            category.CreateDate = category.CreateDate == DateTime.MinValue ? DateTime.Now : category.CreateDate;
            category.Name = category.Name ?? "Категория";
            category.Order = category.Order == 0 
                ? (_storageDb.GetCategoryChildrenCount(category.ParentId) + 1) 
                : category.Order;

            _storageDb.AddCategory(category);
        }

        public IEnumerable<CategoryViewModel> GetCategoryTree()
        {
            var categories = _storageDb.QueryCategories()
                                       .Select(x => (CategoryViewModel)x)
                                       .OrderBy(x => x.ParentId)
                                       .ThenBy(x => x.Order)
                                       .ThenBy(x => x.CreateDate)
                                       .ToArray();

            var tree = GenerateCategoryTree(categories);

            return tree;
        }

        public IEnumerable<RecordViewModel> GetCategoryRecords(Guid? categoryId)
        {
            var filter = $"CategoryId {(categoryId.HasValue ? $"= '{categoryId.Value}'" : "is null")}";

            return _storageDb.QueryRecords(filter)
                             .Select(x => (RecordViewModel)x);
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
