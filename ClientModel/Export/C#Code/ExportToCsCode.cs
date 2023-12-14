using AutomateDesign.Core.Documents;
using NomAutomate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Export.ExportToCsCode
{
    public class ExportToCsCode : ICodeExport
    {
        public void Export(string path, IEnumerable<State> states)
        {
            foreach(State state in states)
            {

            }
            
        }

        /// <summary>
        /// Return the content of a file
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>the content of the file</returns>
        private string ReadFile(string path)
        {
            string contenuFichier = string.Empty;
            try
            {
                // Le chemin complet du fichier en utilisant le répertoire de travail actuel
                string cheminComplet = Path.Combine(Directory.GetCurrentDirectory(), path);
                // Lecture du contenu du fichier
                contenuFichier = File.ReadAllText(cheminComplet);
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur s'est produite : " + e.Message);
            }
            return contenuFichier;
        }

        /// <summary>
        /// Replace an occurence with a new text
        /// </summary>
        /// <param name="text">the text</param>
        /// <param name="toReplace">the occurence</param>
        /// <param name="replacement">the replacement</param>
        /// <returns></returns>
        private string ReplaceOccurence(string text, string toReplace, string replacement)
        {
            string newText = text.Replace(toReplace, replacement);
            return newText;
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
    }
}
