using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Network
{
    public interface IDocumentsClient
    {
        Task DeleteAutomateAsync(int id);
    }
}
