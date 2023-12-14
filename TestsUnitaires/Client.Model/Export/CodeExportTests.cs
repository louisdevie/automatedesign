using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Export
{
    public class CodeExportTests
    {
        [Fact]
        public void Export()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            List<State> states = new List<State>();
            Document doc = new Document();
            Position position = new Position();
            State state1 = new State(doc, 1, "Etat Test 1", position, StateKind.Initial);
        }
    }
}
