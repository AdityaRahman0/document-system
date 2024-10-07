using Dapper;
using Dapper.Contrib.Extensions;
using Entities;
using Repository.ViewModels;
using System;
using System.Collections.Generic;

namespace Repository
{
    public class DocumentRepository : BaseDataAccess
    {
        public IEnumerable<Department> GetDepartments(string conncection)
        {
            using (var Conn = OpenConnection(conncection))
            {
                var sql = "SELECT DepartmentId, DepartmentName, dtmUpd, usrUpd FROM test.Departments with(nolock)";
                return Conn.Query<Department>(sql);
            }
        }


        public void InsertDocument(string connection, Document model, DocumentApproval documentApproval, List<int> departmentId)
        {
            using (var Conn = OpenConnection(connection))
            {
                var insertQuery = "INSERT INTO test.Documents " +
                    "(DocumentNo, DateCreated , ProductOrMaterial, Description, Status, dtmUpd, usrUpd, DepartementRequestor, Pabrik, PathPdf) " +
                    "VALUES(@DocumentNo, getdate(), @ProductOrMaterial, @Description, @Status, getdate(), @usrUpd, @DepartementRequestor, @Pabrik, @PathPdf); " +
                    "SELECT SCOPE_IDENTITY();";
                var documentId = Conn.ExecuteScalar<int>(insertQuery, model);
                foreach (var item in departmentId)
                {
                    var insertQueryRealedDepartemen = "INSERT INTO test.DocumentRelatedDepartments (DocumentId, DepartmentId) VALUES(@DocumentNo, @DepartmentId)";
                    Conn.Execute(insertQueryRealedDepartemen, new { DocumentNo = documentId, DepartmentId = item });
                }
                documentApproval.DocumentId = documentId;
                var insertQueryApproval = "INSERT INTO test.DocumentApprovals" +
                    "(DocumentId, ApproverId, dtmUpd, usrUpd, NextApprovalRole, ApprovalStatus) " +
                    "VALUES(@DocumentId, @ApprovalId, getdate(), @usrUpd, @NextApprovalRole, 'Pending');";
                Conn.Execute(insertQueryApproval, documentApproval);
            }
        }

        public (IEnumerable<Document>, int) GetListDocument(string conncection, 
            int? month, int? year, string factoryFilter, int pageNumber, int pageSize)
        {
            using (var Conn = OpenConnection(conncection))
            {
                var sql = @"SELECT * FROM test.Documents with(nolock) WHERE 1 = 1 and Status in('Submitted', 'REVISED') ";
                var countSql = @"SELECT COUNT(*) FROM test.Documents with(nolock) WHERE 1 = 1 and Status in('Submitted', 'REVISED') ";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(factoryFilter))
                {
                    sql += " AND Pabrik = @factoryFilter";
                    countSql += " AND Pabrik = @factoryFilter";
                    parameters.Add("factoryFilter", factoryFilter);
                }

                if (month.HasValue)
                {
                    sql += " AND MONTH(DateCreated) = @month";
                    countSql += " AND MONTH(DateCreated) = @month";
                    parameters.Add("month", month.Value);
                }

                if (year.HasValue)
                {
                    sql += " AND YEAR(DateCreated) = @year";
                    countSql += " AND YEAR(DateCreated) = @year";
                    parameters.Add("year", year.Value);
                }

                sql += " ORDER BY DateCreated DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
                parameters.Add("Offset", (pageNumber - 1) * pageSize);
                parameters.Add("PageSize", pageSize);

                var total = (int)Math.Ceiling(Conn.ExecuteScalar<int>(countSql, parameters) / (double)pageSize);
                var result = Conn.Query<Document>(sql, parameters);

                return (result, total);
            }
        }

        public DocumentStatusViewModel GetDocumentStatus(string connection, string factory, int month, int year)
        {
            using (var Conn = OpenConnection(connection))
            {
                string query = @" 
                                SELECT 
                                    SUM(CASE WHEN Status = 'COMPLETED' THEN 1 ELSE 0 END) AS ApprovedCount,
                                    SUM(CASE WHEN Status = 'REJECT' THEN 1 ELSE 0 END) AS RejectedCount,
                                    SUM(CASE WHEN Status = 'REVISED' THEN 1 ELSE 0 END) AS RevisedCount
                                FROM test.Documents
                                WHERE MONTH(DateCreated) = @Month AND YEAR(DateCreated) = @Year ";

                var parameters = new DynamicParameters();
                if (!string.IsNullOrEmpty(factory))
                {
                    query += " AND Pabrik = @FactoryId";
                    parameters.Add("FactoryId", factory);
                }
                parameters.Add("Month", month);
                parameters.Add("Year", year);
                return Conn.QuerySingle<DocumentStatusViewModel>(query, parameters);
            }
        }

        public Document GetDocument(string connectiom, int id)
        {
            using (var Conn = OpenConnection(connectiom))
            {
                string query = "select * from test.Documents with (nolock) where DocumentId = @Id";
                return Conn.QuerySingleOrDefault<Document>(query, new { Id = id });
            }
        }

        public void UpdateDocument(string connection, Document model) 
        {
            using (var Conn = OpenConnection(connection))
            {
                Conn.Update<Document>(model);
            }
        }

        public void UpdateDocumetApproval(string connection, int documentId) 
        {
            using (var Conn = OpenConnection(connection))
            {
                var query = "UPDATE aisdb.test.DocumentApprovals SET ApprovalStatus='PENDING' WHERE DocumentId =@DocumentId and ApprovalStatus = 'REVISED';";
                Conn.Execute(query, new { DocumentId = documentId });
            }
        }
    }
}
