using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace Entities
{
    [Table("test.Documents")]
    public class Document
    {
        [ExplicitKey]
        public int DocumentId { get; set; }
        public string DocumentNo { get; set; }
        public DateTime DateCreated { get; set; }
        public int DepartementRequestor { get; set; }
        public string ProductOrMaterial { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Pabrik { get; set; }
        public DateTime dtmUpd { get; set; }
        public string usrUpd { get; set; }
        public string PathPdf { get; set; }
    }
}
