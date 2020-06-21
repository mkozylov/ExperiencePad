using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExperiencePad.Data
{
    public class Category
    {
        public const string TableName = "Categories";

        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public int Order { get; set; }

        public Category Parent { get; set; }

        public List<Category> Children { get; set; } = new List<Category>();
    }
}
