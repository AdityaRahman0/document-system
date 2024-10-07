using Dapper;
using Repository.ViewModels;
using System.Collections.Generic;

namespace Repository
{
    public class AuditTrailRepository : BaseDataAccess
    {
        public IEnumerable<TrackingStatusViewModel> GetTrackingStatus(string conn, int documentId) 
        {
            using (var Conn = OpenConnection(conn))
            {
                var query = @"
                        select distinct u.UserName, da.ApprovalDate, da.ApprovalStatus, a.Action, d.DocumentNo 
                        from test.AuditTrail a with (nolock)
                        INNER JOIN test.Documents d with (nolock) on a.DocumentId = d.DocumentId
                        INNER JOIN test.DocumentApprovals da with (nolock) on a.DocumentId = da.DocumentId
                        INNER JOIN test.Users u with (nolock) on da.ApproverId = u.UserId
                        where d.DocumentId = @DocumentId
                        Order by da.ApprovalDate desc
                         ";
               return Conn.Query<TrackingStatusViewModel>(query, new { DocumentId = documentId });
            }
        }
    }
}
