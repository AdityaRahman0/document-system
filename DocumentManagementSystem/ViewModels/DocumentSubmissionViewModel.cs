using Entities;
using System;
using System.Collections.Generic;
using System.Web;

namespace DocumentManagementSystem.ViewModels
{
    public class DocumentSubmissionViewModel
    {
        public int DocumentId { get; set; }
        public string DocumentNo { get; set; }
        public DateTime DateCreated { get; set; }
        public int DepartementRequestor { get; set; }
        public string ProductOrMaterial { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Submitted"; 
        public HttpPostedFileBase PdfFile { get; set; } 
        public string usrUpd { get; set; }
        public string Pabrik { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public List<int> SelectedDepartmentIds { get; set; }
        public string FactoryFilter { get; set; }
        public int? Month { get; set; } // Nullable to allow empty
        public int? Year { get; set; }
        public IEnumerable<Document> documents { get; set; }
        public List<string> PabrikOptions { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string Role { get; set; }
        public Document document { get; set; }

    }
}