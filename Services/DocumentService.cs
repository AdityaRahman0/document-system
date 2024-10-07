using Entities;
using PublicClass;
using Repository;
using System.Collections.Generic;

namespace Services
{
    public class DocumentService
    {
        private readonly DocumentRepository repository = new DocumentRepository();
        private readonly UserRepository userRepository = new UserRepository();
        private readonly EmailService emailService = new EmailService();
        private readonly DocumentRepository documentRepository = new DocumentRepository();

        public IEnumerable<Department> GetDepartments(string connection) 
        {
            return repository.GetDepartments(connection);
        }

        public void InsertDocument(string connection, Document model, DocumentApproval approval, List<int> department)
        {
            var getManager = GetManager(connection);
            approval.ApprovalId = getManager.UserId;
            repository.InsertDocument(connection, model, approval, department);
            emailService.SendApprovalEmail(getManager.Email, Constant.subject, emailService.BodyEmailApproval(model.DocumentNo));
        }


        public (IEnumerable<Document>, int) GetListDocument(string connection,
            int? month, int? year, string factoryFilter, int pageNumber, int pageSize)
        {
            var result = repository.GetListDocument(connection, month, year, factoryFilter, pageNumber, pageSize);
            return result;
        }

        public User GetManager(string connection) 
        {
            return userRepository.GetManager(connection);
        }

        public Document GetDocument(string connection, int id) 
        {
            return documentRepository.GetDocument(connection, id);
        }


        public void UpdateDocument(string connection, Document model) 
        {
            documentRepository.UpdateDocument(connection, model);
            documentRepository.UpdateDocumetApproval(connection, model.DocumentId);
        }
    }
}
