using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Document = AutomateDesign.Core.Documents.Document;

namespace AutomateDesign.Client.Model.Export.FromCsCode
{
    public class ImportFromCsCode
    {
        /// <summary>
        /// Import un document depuis le code C# d'un automate
        /// L'automate doit respecté la même synthax que les automates exportés avec l'application
        /// Le dossier fournit doit être le dossier racine du projet avec dedans le chemin suivant :
        ///     /Model/Automate/Etats avec dans /Etats les états à convertir
        /// </summary>
        /// <param name="path">Chemin d'accès de l'automate a importé</param>
        /// <returns>Le document créé à partir de l'automate forunit</returns>
        public Document Import(string path)
        {
            // Création du document
            Document importDocument = new Document();

            // Récupère le nom de la solution et l'attribut comme nom du document
            string[] folders = path.Split('\\');
            importDocument.Header.Name = folders[folders.Length-1];
            
            // Test si le chemin vers les états existe
            string statePath = path + "/Model/Automate/Etats/";
            DirectoryInfo directoryInfo = new DirectoryInfo(statePath);
            try
            {
                string[] files = GetAllFilesOfFolder(statePath);

                int i = 1;
                foreach (string file in files)
                {
                    string name = string.Empty;
                    Position position = new Position(10*i, 10*i);
                    StateKind kind = StateKind.Normal;
                    State state = new State(importDocument, i, name,position, kind);

                    i++;
                }

            } 
            catch  
            {
                throw new Exception("Le dossier n'existe pas");
            }

            return importDocument;
        }

        /// <summary>
        /// Retourne tous les fichiers du dossier passé en paramètre
        /// </summary>
        /// <param name="path">le dossier à analyser</param>
        /// <returns>les fichiers du dossier</returns>
        private string[] GetAllFilesOfFolder(string path)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            FileInfo[] files = folder.GetFiles();
            string[] result = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i].Name;
                string fileExtension = Path.GetExtension(fileName);
                result[i] = fileName + "." + fileExtension;
            }

            return result;
        }
    }
}
