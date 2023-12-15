using AutomateDesign.Client.Model.Export;
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
        /// <summary>
        /// Test si les dossiers sont créés, si les fichiers existent et si ils sont non vident
        /// </summary>
        [Fact]
        public void Export()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            List<State> states = new List<State>();
            Document doc = new Document();
            Position position = new Position();

            State state1 = new State(doc, 1, "Etat Test 1", position, StateKind.Initial);
            State state2 = new State(doc, 2, "Etat Test 12", position, StateKind.Normal);
            State state3 = new State(doc, 4, "Etat Test 45", position, StateKind.Normal);

            states.Add(state1);
            states.Add(state2);
            states.Add(state3);

            doc.AddStates(states);

            ExportToCsCode export = new ExportToCsCode();
            export.Export(path, doc);
            path = path + "/Automate_sans_nom/Automate/";
            
            // Vérifie que le dossier existe
            Assert.True(Directory.Exists(path));
            
            // Vérifie que les fichiers existent
            Assert.True(File.Exists(path + "Automate.cs"));
            Assert.True(File.Exists(path + "Etat.cs"));
            Assert.True(File.Exists(path + "Event.cs"));
            Assert.True(File.Exists(path + "Etats/EtatTest1.cs"));
            Assert.True(File.Exists(path + "Etats/EtatTest12.cs"));
            Assert.True(File.Exists(path + "Etats/EtatTest45.cs"));

            // Vérifie que les fichiers ne soient pas vides
            FileInfo fileInfo;
            fileInfo = new FileInfo(path + "Automate.cs");
            Assert.True(fileInfo.Length > 1);
            fileInfo = new FileInfo(path + "Etat.cs");
            Assert.True(fileInfo.Length > 1);
            fileInfo = new FileInfo(path + "Event.cs");
            Assert.True(fileInfo.Length > 1);
            fileInfo = new FileInfo(path + "Etats/EtatTest1.cs");
            Assert.True(fileInfo.Length > 1);
            fileInfo = new FileInfo(path + "Etats/EtatTest12.cs");
            Assert.True(fileInfo.Length > 1);
            fileInfo = new FileInfo(path + "Etats/EtatTest45.cs");
            Assert.True(fileInfo.Length > 1);


        }
    }
}
