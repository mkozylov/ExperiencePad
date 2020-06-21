using System;
using System.Collections.Generic;
using System.Text;

namespace ExperiencePad.Data
{
    public class Record
    {
        public const string TableName = "Records";

        public Guid Id { get; set; }

        public Guid? CategoryId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public int Order { get; set; }

        public DateTime CreateDate { get; set; }

        public string Type { get; set; }
    }
}
