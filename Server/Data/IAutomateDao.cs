using System.Reflection.Metadata;

namespace AutomateDesign.Server.Data
{
    public interface IAutomateDao
    {
        public Document Create(Document document);

        public Document ReadById(int id);

        public void Update(Document document);

        public void Delete(int id);
    }
}
