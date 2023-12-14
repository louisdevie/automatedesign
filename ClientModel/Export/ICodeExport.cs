using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Export
{
    public interface ICodeExport
    {
        public void Export(string path, Document document);
    }
}
