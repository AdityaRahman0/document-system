using Dapper;
using Entities;
using Repository.ViewModels;
using System;
using System.Collections.Generic;

namespace Repository
{
    public class ApprovalRepository : BaseDataAccess
    {
        public void InsertDocumentApproval(string connection, DocumentApproval documentApproval)
        {
            using (var Conn = OpenConnection(connection))
            {
                var insertQueryApproval = "INSERT INTO test.DocumentApprovals" +
                    "(DocumentId, ApproverId, dtmUpd, usrUpd, NextApprovalRole, ApprovalStatus, ApprovalDate) " +
                    "VALUES(@DocumentId, @ApproverId, getdate(), @usrUpd, @NextApprovalRole, @ApprovalStatus, @ApprovalDate);";
                Conn.Execute(insertQueryApproval, documentApproval);
            }
        }


        public void InsertAuditTrail(string connection, AuditTrail auditTrail)
        {
            using (var Conn = OpenConnection(connection))
            {
                var insertQueryApproval = "INSERT INTO test.AuditTrail " +
                    "(DocumentId, [Action], ActionBy, ActionDate, dtmUpd, usrUpd) " +
                    "VALUES(@DocumentId, @Action, @ActionBy, getdate(), getdate(), @ActionBy)";
                Conn.Execute(insertQueryApproval, auditTrail);
            }
        }

        public IEnumerable<ApprovalViewModel> GetSubmitDocumentForApproval(string connection, 
            string role, int userId,int page, int size)
        {
            using (var Conn = OpenConnection(connection))
            {
                var offset = (page - 1) * size;
                var query = @" 
                    select d.DocumentId , d.DocumentNo,d.Description ,d.ProductOrMaterial , d.Status, da.ApprovalId, da.NextApprovalRole
                    from test.Documents d with(nolock)
                    INNER JOIN test.DocumentApprovals da with(nolock) on d.DocumentId = da.DocumentId 
                    where d.Status = 'Submitted' 
                    and da.ApprovalStatus = 'Pending' 
                    and da.NextApprovalRole = @NextApprovalRole
                    and da.ApproverId = @ApproverId
                    ORDER BY d.DocumentId DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY; ";
                return Conn.Query<ApprovalViewModel>(query, new { NextApprovalRole = role, ApproverId = userId, Offset = offset, PageSize = size });
            }
        }

        public IEnumerable<ApprovalViewModel> GetTrackingDocumentApproval(string connection, int page, int size)
        {
            using (var Conn = OpenConnection(connection))
            {
                var offset = (page - 1) * size;
                var query = @"
                    select distinct d.DocumentId , d.DocumentNo,d.Description ,d.ProductOrMaterial , d.Status
                    from test.Documents d with(nolock)
                    ORDER BY d.DocumentId DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY; ";
                return Conn.Query<ApprovalViewModel>(query, new {Offset = offset, PageSize = size });
            }
        }

        public int TotalTrackingDocumentApproval(string connection, int page, int size)
        {
            using (var Conn = OpenConnection(connection))
            {
                var offset = (page - 1) * size;
                var query = "select COUNT(*) " +
                    "from test.Documents d with(nolock)";
                var total = (int)Math.Ceiling(Conn.ExecuteScalar<int>(query, new { Offset = offset, PageSize = size }) / (double)size);
                return total;
            }
        }

        public int TotalSubmitDocumentForApproval(string connection, int page, int size)
        {
            using (var Conn = OpenConnection(connection))
            {
                var offset = (page - 1) * size;
                var query = "select COUNT(*) " +
                    "from test.Documents d with(nolock) " +
                    "INNER JOIN test.DocumentApprovals da with(nolock) on d.DocumentId = da.DocumentId " +
                    "where d.Status = 'Submitted' and da.ApprovalStatus = 'Pending' ";
                var total = (int)Math.Ceiling(Conn.ExecuteScalar<int>(query, new { Offset = offset, PageSize = size }) / (double)size);
                return total;
            }
        }

        public DocumentApproval GetDocumentApproval(string connection, int id)
        {
            using (var Conn = OpenConnection(connection))
            {
                string query = @"SELECT * FROM test.DocumentApprovals with (nolock) where ApprovalId = @ApprovalId";
                return Conn.QuerySingleOrDefault<DocumentApproval>(query, new { ApprovalId = id });
            }
        }

        public void UpdateDocumentApproval(string connection, int approvalId, string status, DateTime? date)
        {
            using (var Conn = OpenConnection(connection))
            {
                string query = "update test.DocumentApprovals set ApprovalStatus = @ApprovalStatus , ApprovalDate = @ApprovalDate where ApprovalId = @ApprovalId";
                Conn.Execute(query, new { ApprovalStatus = status, ApprovalDate = date, ApprovalId = approvalId });
            }
        }

        public int GetTotalDepartmentApprove(string connection, string roles, int documetId) 
        {
            using (var Conn = OpenConnection(connection)) 
            {
                string query = "SELECT COUNT(*) from test.DocumentApprovals with (nolock) where NextApprovalRole = @NextApprovalRole and DocumentId = @DocumentId and UPPER(ApprovalStatus) = 'PENDING' ";
                return Conn.ExecuteScalar<int>(query, new { NextApprovalRole = roles, DocumentId = documetId});
            }
        }
    }
}
