using AutomateDesign.Core.Documents;
using System.Reflection.Metadata;

namespace AutomateDesign.Server.Data
{
    public interface IAutomateDao
    {
        public DocumentCrypte Create(DocumentCrypte document);

        public DocumentCrypte ReadById(int id);

        public void Update(DocumentCrypte document);

        public void Delete(int id);
    }
}
