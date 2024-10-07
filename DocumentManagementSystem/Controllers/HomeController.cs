using PublicClass;
using Repository;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        readonly DocumentRepository documentRepository = new DocumentRepository();
        private readonly string connectionString = Helper.ConnectionString.connectionString;
        
        
        public ActionResult Index(string factoryId, int? month, int? year)
        {
            string SelectedFactory = factoryId ?? "";
            int? SelectedMonth = month ?? DateTime.Now.Month;
            int? SelectedYear = year ?? DateTime.Now.Year;
            var model = documentRepository.GetDocumentStatus(connectionString, SelectedFactory, SelectedMonth.Value, SelectedYear.Value);
            model.Factories = Constant.listFactort;
            model.Months = GetMonthList();
            model.Years = GetYearList();
            return View(model);
        }


        // Menyiapkan data bulan untuk dropdown
        private List<DropDown> GetMonthList()
        {
            return Enumerable.Range(1, 12).Select(x => new DropDown
            {
                Value = x.ToString(),
                Text = new DateTime(1, x, 1).ToString("MMMM")
            }).ToList();
        }

        // Menyiapkan data tahun untuk dropdown
        private List<DropDown> GetYearList()
        {
            int currentYear = DateTime.Now.Year;
            return Enumerable.Range(currentYear - 10, 11).Select(x => new DropDown
            {
                Value = x.ToString(),
                Text = x.ToString()
            }).ToList();
        }
    }
}