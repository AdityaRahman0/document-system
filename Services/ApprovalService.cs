using Entities;
using PublicClass;
using Repository;
using Repository.ViewModels;
using System;
using System.Collections.Generic;

namespace Services
{
    public class ApprovalService
    {
        readonly ApprovalRepository approvalRepository = new ApprovalRepository();
        readonly DocumentRepository documentRepository = new DocumentRepository();
        readonly UserRepository repository = new UserRepository();
        readonly AuditTrailRepository auditTrailRepository = new AuditTrailRepository();
        readonly EmailService emailService = new EmailService();

        public ApprovalPaggingViewModel GetSubmitDocumentForApproval(string connection,string role, int id, int page, int size)
        {
            var result = approvalRepository.GetSubmitDocumentForApproval(connection,role,id ,page, size);
            var total = approvalRepository.TotalSubmitDocumentForApproval(connection, page, size);
            var response = new ApprovalPaggingViewModel 
            { 
                ApprovalDocument = result,
                Pagging = new BasePagging 
                {
                    CurrentPage = page,
                    TotalPages = total
                }
            };
            return response;
        }

        public ApprovalPaggingViewModel GetTrackingDocumentApproval(string connection, int page, int size)
        {
            var result = approvalRepository.GetTrackingDocumentApproval(connection, page, size);
            var total = approvalRepository.TotalTrackingDocumentApproval(connection, page, size);
            var response = new ApprovalPaggingViewModel
            {
                ApprovalDocument = result,
                Pagging = new BasePagging
                {
                    CurrentPage = page,
                    TotalPages = total
                }
            };
            return response;
        }

        public bool UpdateNextApproval(string connection, int approvalId, int documentId, string status, string remaks, int id, string documentNo) 
        {
            var approvalDate = DateTime.Now;
            var model = GetDocumentApproval(connection, approvalId);
            if (model == null)
            {
                return false;
            }
            var user = repository.GetUser(connection, model.ApproverId);
            if (user == null )
            {
                return false;
            }
            string nextApproval = GetNextApprovalRoles(user.Role, status);
            var nextApprove = repository.GetNextUser(connection, nextApproval, documentId);
            approvalRepository.UpdateDocumentApproval(connection, model.ApprovalId, status, approvalDate);
            if (CheckStatus(status))
            {
                if (CheckStatusUpdateDepartment(connection, model.NextApprovalRole, model.DocumentId))
                {
                    foreach (var item in nextApprove)
                    {
                        DocumentApproval documentApproval = new DocumentApproval
                        {
                            NextApprovalRole = nextApproval,
                            ApprovalStatus = "Pending",
                            DocumentId = documentId,
                            usrUpd = Constant.system,
                            dtmUpd = DateTime.Now,
                            ApproverId = item.UserId
                        };
                        approvalRepository.InsertDocumentApproval(connection, documentApproval);
                        emailService.SendApprovalEmail(item.Email, Constant.subject, emailService.BodyEmailApproval(documentNo));
                    }
                }
                AuditTrail audit = new AuditTrail
                {
                    Action = remaks,
                    ActionBy = id,
                    ActionDate = DateTime.Now,
                    DocumentId = model.DocumentId,
                    dtmUpd = DateTime.Now,
                    usrUpd = Constant.system
                };
                approvalRepository.InsertAuditTrail(connection, audit);
            }
            if (CheckBeforeUpdateStatus(user.Role, status))
            {
                var getDocument = documentRepository.GetDocument(connection, documentId);
                getDocument.Status = Status.COMPLETED.ToString();
                documentRepository.UpdateDocument(connection, getDocument);
                AuditTrail audit = new AuditTrail
                {
                    Action = remaks,
                    ActionBy = id,
                    ActionDate = DateTime.Now,
                    DocumentId = model.DocumentId,
                    dtmUpd = DateTime.Now,
                    usrUpd = Constant.system
                };
                approvalRepository.InsertAuditTrail(connection, audit);
            }
            if (CheckStatusUpdateRejectAndRevised(user.Role, status))
            {
                var getDocument = documentRepository.GetDocument(connection, documentId);
                getDocument.Status = status;
                documentRepository.UpdateDocument(connection, getDocument);
                AuditTrail audit = new AuditTrail
                {
                    Action = remaks,
                    ActionBy = id,
                    ActionDate = DateTime.Now,
                    DocumentId = model.DocumentId,
                    dtmUpd = DateTime.Now,
                    usrUpd = Constant.system
                };
                approvalRepository.InsertAuditTrail(connection, audit);
            }
            return true;
        }


        public string GetNextApprovalRoles(string roles, string status) 
        {
            if (roles.ToUpper() == Roles.MANAGER.ToString() && status.ToUpper() == Status.APPROVE.ToString())
            {
                return Roles.DEPARTMENT.ToString();
            }
            else if (roles.ToUpper() == Roles.DEPARTMENT.ToString() && status.ToUpper() == Status.APPROVE.ToString())
            {
                return Roles.DCC.ToString();
            }
            else if (roles.ToUpper() == Roles.DCC.ToString() && status.ToUpper() == Status.APPROVE.ToString())
            {
                return Roles.QA.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public bool CheckStatusUpdateDepartment(string connection, string role, int document) 
        {
            if (role.ToUpper() == Roles.DEPARTMENT.ToString())
            {
                var result = approvalRepository.GetTotalDepartmentApprove(connection, role, document);
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return true;
            }
        }

        public DocumentApproval GetDocumentApproval(string conn, int approvalId) 
        {
            return approvalRepository.GetDocumentApproval(conn, approvalId);
        }

        public Document GetDocumentNo(string conn, int documentId) 
        {
            return documentRepository.GetDocument(conn, documentId);
        }

        public IEnumerable<TrackingStatusViewModel> GetTrackingStatus(string conn, int documentId) 
        {
            return auditTrailRepository.GetTrackingStatus(conn, documentId);
        }

        public bool CheckStatus(string status) 
        {
            if (status.ToUpper() == Status.REJECT.ToString() || status.ToUpper() == Status.REVISED.ToString())
            {
                return false;
            }
            return true;
        }

        private bool CheckBeforeUpdateStatus(string roles, string status) 
        {
            if (roles.ToUpper() == Roles.QA.ToString() && status.ToUpper() == Status.APPROVE.ToString())
            {
                return true;
            }
         
            return false;
        }
        private bool CheckStatusUpdateRejectAndRevised(string roles, string status) 
        {
            if (status.ToUpper() == Status.REJECT.ToString() || status.ToUpper() == Status.REVISED.ToString())
            {
                return true;
            }
            return false;
        }
    }
}
