using AutomateDesign.Core.Documents;
using NomAutomate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Export.ExportToCsCode
{
    public class ExportToCsCode : ICodeExport
    {
        /// <summary>
        /// Export a document to a code structure in C#
        /// </summary>
        /// <param name="path"></param>
        /// <param name="document"></param>
        public void Export(string path, Document document)
        {
            // Creation of the root folder of the export
            string documentFolderName = document.Header.Name;
            documentFolderName.Replace(' ', '_');
            path = path + "/" + documentFolderName;
            makeDirectory(path);
            // Creation of the Automate Folder
            path = path + "/Automate";
            makeDirectory(path);
            // Creation of the states folder
            makeDirectory(path + "/Etats");

            string innerPath = "/CodeTemplateStructure/Automate/";
            // Creation of the Automate class file
            string automate = this.ReadFile(innerPath+"Automate.cs");
            automate.Replace("NomAutomate", documentFolderName);
            this.makeFile(path + "Automate.cs", automate);

            // Creation of the State class file
            string etat = this.ReadFile(innerPath + "Etat.cs");
            etat.Replace("NomAutomate", documentFolderName);
            this.makeFile(path + "Etat.cs", etat);

            // Creation of the Event enum file
            string eventEnum = this.ReadFile(innerPath + "Event.cs");
            eventEnum.Replace("NomAutomate", documentFolderName);
            string events = string.Join(", ", document.Events);
            events.Replace(" ", "_");
            events.ToUpper();
            eventEnum.Replace("//ListeDesEvents", events);
            this.makeFile(path+"Event.cs", events); 

            // Creation of the State class file for each state of the document
            foreach (State state in document.States)
            {
                string statePath = path + "/Etats/";
                string specificEtat = this.ReadFile(statePath + "EtatModel.cs");
                specificEtat.Replace("NomAutomate", documentFolderName);
                string stateName = state.Name;
                stateName.Replace(" ", "");
                specificEtat.Replace("EtatX", state.Name);
                
                // A CONTINUER !
                specificEtat.Replace("//TransitionAutomate", "");
                // A CONTINUER !
            }
            
        }

        /// <summary>
        /// Return the content of a file
        /// </summary>
        /// <param name="path">Path of the file with the name of the file include</param>
        /// <returns>the content of the file</returns>
        private string ReadFile(string path)
        {
            string fileContent = string.Empty;
            try
            {
                string completPath = Path.Combine(Directory.GetCurrentDirectory(), path);
                fileContent = File.ReadAllText(completPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur s'est produite : " + e.Message);
            }
            return fileContent;
        }

        /// <summary>
        /// Make a new directory at the given path
        /// </summary>
        /// <param name="path">the path with the name of the directory at the end of the path</param>
        private void makeDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur s'est produite : " + e.Message);
            }
        }

        /// <summary>
        /// Make a file in the given path with the given content
        /// </summary>
        /// <param name="path">the path of the file with its name include</param>
        /// <param name="content">the content of the file</param>
        private void makeFile(string path, string content)
        {
            try
            {
                File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur s'est produite : " + e.Message);
            }
        }

        /// <summary>
        /// Upper case the first letter of each words and remove space
        /// </summary>
        /// <param name="name">the name to format</param>
        /// <returns>the name format</returns>
        private string formatNameClass(string name)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string newName = textInfo.ToTitleCase(name);
            newName.Replace(" ", "");
            return newName;
        }
    }
}
