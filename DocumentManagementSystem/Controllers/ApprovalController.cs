using DocumentManagementSystem.Provider;
using PublicClass;
using Repository.ViewModels;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class ApprovalController : Controller
    {
        ApprovalService approvalService = new ApprovalService();
        private string connectionString = Helper.ConnectionString.connectionString;

        [Authorize(Roles = "Manager, QA, DCC, Department")]
        // GET: Approval
        public ActionResult Index(int page = 1)
        {
            string role = Session["Role"].ToString();
            int user = (int)Session["Id"];
            var view = approvalService.GetSubmitDocumentForApproval(connectionString,role, user, page, 10);
            return View(view);
        }

        [Authorize(Roles = "Manager, QA, DCC, Department")]

        public ActionResult Approval(int approvalId)
        {
            var approval = approvalService.GetDocumentApproval(connectionString,approvalId);
            var document = approvalService.GetDocumentNo(connectionString, approval.DocumentId);
            var status = approvalService.GetTrackingStatus(connectionString, approval.DocumentId);
            DocumentAndDocApprovalViewModel model = new DocumentAndDocApprovalViewModel
            {
                Document = document,
                DocumentApproval = approval,
                TrackingStatus = status
            };
            return View(model);
        }

        [Authorize(Roles = "Manager, QA, DCC, Department")]

        public ActionResult ViewPdf(string fileName)
        {
            var filePath = Path.Combine(fileName); 
            var contentType = "application/pdf";

            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }

            return File(filePath, contentType);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, QA, DCC, Department")]
        public ActionResult Approval(int approvalId, int documentId, string remarks, string action, string documentNo)
        {
            string status;
            switch (action.ToLower())
            {
                case "approve":
                    status = Status.APPROVE.ToString();
                    break;
                case "reject":
                    status = Status.REJECT.ToString();
                    break;
                case "revised":
                    status = Status.REVISED.ToString();
                    break;
                default:
                    status = Status.PENDING.ToString();
                    break;
            }
            int userId = (int)Session["Id"];
            var response = approvalService.UpdateNextApproval(connectionString, approvalId, documentId, status, remarks, userId, documentNo);

            if (response)
            {
                TempData["Message"] = $"Document approval has been {status.ToLower()}.";
            }
            else 
            {
                TempData["Message"] = $"Document approval failed to Update.";
            }
            return RedirectToAction("Index");
        }
    }
}