using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
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
            // Création d'un document vide
            Document importDocument = new Document();

            // Récupèration du nom de la solution et l'attribut comme nom du document
            string[] folders = path.Split('\\');
            importDocument.Header.Name = folders[folders.Length-1];
            
            // Test si le chemin vers les états existe
            string statePath = path + "/Model/Automate/Etats/";
            try
            {
                string[] files = GetAllFilesOfFolder(statePath);
                int i = 1;
                Dictionary<String, int> states = new Dictionary<String, int>();

                // Parcours de tous les fichiers pour récupérer les états
                foreach (string file in files)
                {
                    Position position = new Position(10*i, 10);
                    
                    // Récupération du nom de la classe
                    string className = Path.GetFileNameWithoutExtension(path+file);

                    // Création de l'état
                    State state = importDocument.CreateState(className, position);
                    states.Add(className, state.Id);
                    i++;
                }

                // Création des Events


                // Parcours de tous les fichiers pour récupérer les transitions
                // Afin de créer une transition nous avons besoin de l'état de départ et de l'état d'arrivée
                foreach (string file in files)
                {
                    string startStateName = Path.GetFileName(path+file);
                    State startState = importDocument.FindState(states[startStateName]);
                    string endStateName = "" ;
                    State endState = importDocument.FindState(states[]);
                    
                    importDocument.CreateTransition(startState, endState, );
                    
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

        /// <summary>
        /// Renvoie le nom de la class dont le chemin d'accès est fournit en paramètre
        /// </summary>
        /// <param name="classPath">le chemin d'accès de la classe</param>
        /// <returns>le nom de la classe</returns>
        private string GetClassName(string classPath)
        {
            TextReader reader = new StreamReader(classPath);
            string fileContent = reader.ReadToEnd();
            reader.Close();
            int classNameIndex = fileContent.IndexOf("public class");
            string className = fileContent.Substring(classNameIndex + 11, fileContent.IndexOf(";", classNameIndex) - classNameIndex - 11);
            return className;
        }

        /// <summary>
        /// Renvoie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        private State? GetState(string name, Document doc)
        {
            State? returnState = null;
            foreach(State state in doc.States)
            {
                if (state.Name == name)
                {
                    returnState = state;
                    break;
                }
            }
            return returnState;
        }
    }
}
