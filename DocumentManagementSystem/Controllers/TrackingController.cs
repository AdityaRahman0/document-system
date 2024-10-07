using Repository.ViewModels;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class TrackingController : Controller
    {
        ApprovalService approvalService = new ApprovalService();
        private string connectionString = Helper.ConnectionString.connectionString;

        // GET: Tracking
        public ActionResult Index(int page = 1)
        {
            var view = approvalService.GetTrackingDocumentApproval(connectionString, page, 10);
            return View(view);
        }

        public ActionResult Detail(int documentId) 
        {
            var view = approvalService.GetTrackingStatus(connectionString, documentId);
            DocumentAndDocApprovalViewModel model = new DocumentAndDocApprovalViewModel 
            {
                TrackingStatus = view
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Print(int documentId)
        {
            var document = approvalService.GetDocumentNo(connectionString, documentId);
            if (document == null)
            {
                return HttpNotFound();
            }

            byte[] fileContent = System.IO.File.ReadAllBytes(document.PathPdf);

            // Return file sebagai PDF atau format lainnya
            return File(fileContent, "application/pdf", "Document-" + document.DocumentNo + ".pdf");
        }
    }
}