using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Export
{
    public class ImportCodeTest
    {
        /// <summary>
        /// Test si le docuemnt est créé, si les états sont bons ainsi que leur transition
        /// </summary>
        [Fact]
        public void Import()
        {
            ImportCodeTest importCodeTest = new ImportCodeTest();
            importCodeTest.Import();
        }
    }
}
