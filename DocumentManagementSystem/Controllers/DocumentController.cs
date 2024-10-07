using DocumentManagementSystem.ViewModels;
using Entities;
using PublicClass;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace DocumentManagementSystem.Controllers
{
    public class DocumentController : Controller
    {
        private string connection = Helper.ConnectionString.connectionString;
        DocumentService service = new DocumentService();

        // GET: Document
        [Authorize(Roles = "Requestor")]
        public ActionResult SubmitDocument()
        {
            string docNumber = $"DOCT-{DateTime.Now:yyyy-MM-dd-HHmmss}";
            DocumentService documentService = new DocumentService();
            var departments = documentService.GetDepartments(connection);
            var model = new DocumentSubmissionViewModel
            {
                Departments = departments,
                DateCreated = DateTime.Now
            };
            ViewBag.DocumentNumber = docNumber;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Requestor")]
        public ActionResult SubmitDocument(DocumentSubmissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.usrUpd = (string)Session["UserName"];
                string path = null;
                if (model.PdfFile != null && model.PdfFile.ContentLength > 0)
                {
                    string newFileName = $"REQUEST-DOC-{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(model.PdfFile.FileName)}";

                    path = Path.Combine(Server.MapPath("~/App_Data/UploadedFiles"), newFileName);

                    model.PdfFile.SaveAs(path);
                }

                SaveDocumentToDatabase(model, path);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        private void SaveDocumentToDatabase(DocumentSubmissionViewModel model, string path)
        {
            DocumentService service = new DocumentService();
            var document = new Document
            {
                DocumentNo = model.DocumentNo,
                DateCreated = model.DateCreated,
                ProductOrMaterial = model.ProductOrMaterial,
                Description = model.Description,
                Status = model.Status,
                DepartementRequestor = model.DepartementRequestor,
                Pabrik = model.Pabrik,
                usrUpd = model.usrUpd,
                dtmUpd = DateTime.Now,
                PathPdf = path
            };
            DocumentApproval documentApproval = new DocumentApproval
            {
                usrUpd = (string)Session["UserName"],
                NextApprovalRole = "Manager"
            };
            List<int> department = model.SelectedDepartmentIds;
            service.InsertDocument(connection, document, documentApproval, department);
        }
        public ActionResult DocumentSubmissionSuccess()
        {
            return View();
        }

        public ActionResult Index(int pageNumber = 1)
        {
            var model = new DocumentSubmissionViewModel
            {
                PabrikOptions = Constant.listFactort,
                Month = null,
                Year = null,
                PageNumber = pageNumber,
                Role = (string)Session["Role"]
            };

            LoadDocuments(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DocumentSubmissionViewModel model)
        {
            model.PabrikOptions = Constant.listFactort;
            model.PageNumber = model.PageNumber > 0 ? model.PageNumber : 1;

            LoadDocuments(model);
            return View(model);
        }

        private void LoadDocuments(DocumentSubmissionViewModel model)
        {
            DocumentService service = new DocumentService();
            var result = service.GetListDocument(connection, model.Month, model.Year, model.FactoryFilter, model.PageNumber, model.PageSize);
            model.documents = result.Item1;
            model.TotalPages = result.Item2;
        }

        [HttpPost]
        [Authorize(Roles = "Requestor")]
        public ActionResult EditDocument(DocumentSubmissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string path = null;
                    var document = service.GetDocument(connection, model.DocumentId);
                    if (model.PdfFile != null && model.PdfFile.ContentLength > 0)
                    {
                        DeleteExistingFile(document.PathPdf);
                        
                        string newFileName = $"REQUEST-DOC-{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(model.PdfFile.FileName)}";

                        path = Path.Combine(Server.MapPath("~/App_Data/UploadedFiles"), newFileName);
                        model.PdfFile.SaveAs(path);
                    }
                    UpdateDocument(model, path, document);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating document: {ex.Message}");
                }
            }
            return View(model);
        }


        public ActionResult EditDocument(int id)
        {
            var document = service.GetDocument(connection, id);
            var departments = service.GetDepartments(connection);
            if (document == null || departments == null)
            {
                return HttpNotFound();
            }
            var model = new DocumentSubmissionViewModel
            {
                Departments = departments,
                DateCreated = DateTime.Now,
                document = document
            };
            return View(model);
        }

        private void DeleteExistingFile(string fullPath)
        {
            if (!string.IsNullOrEmpty(fullPath))
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }

        private void UpdateDocument(DocumentSubmissionViewModel model, string path, Document document)
        {
            document.DepartementRequestor = model.DepartementRequestor;
            document.Pabrik = model.Pabrik;
            document.ProductOrMaterial = model.ProductOrMaterial;
            document.Status = "Submitted";
            if (!string.IsNullOrEmpty(path))
            {
                document.PathPdf = path;
            }
            service.UpdateDocument(connection, document);
        }
    }
}