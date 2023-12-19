using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Export.CsCode
{
    public class ExportToCsCode : ICodeExport
    {
        public ExportToCsCode()
        {
        }

        /// <summary>
        /// Exporte un document en structure de code C#
        /// </summary>
        /// <param name="path"></param>
        /// <param name="document"></param>
        public void Export(string path, Document document)
        {
            // Création du dossier racine de l'exportation
            string documentFolderName = document.Header.Name;
            documentFolderName = documentFolderName.Replace(" ", "_");
            path = path + "/" + documentFolderName;
            makeDirectory(path);
            // Création du dossier Model
            path = path + "/Model";
            makeDirectory(path);
            // Création du dossier Automate
            path = path + "/Automate/";
            makeDirectory(path);
            // Création du dossier Etats
            makeDirectory(path + "Etats/");

            // Récupération du répertoire du projet afin d'aller les documents sources à modifier
            string currentFolder = Directory.GetCurrentDirectory();
            DirectoryInfo parentFolder = new DirectoryInfo(currentFolder);
            for (int i = 0; i < 4; i++)
            {
                parentFolder = parentFolder.Parent;
            }
            string innerPath = parentFolder + "/CodeTemplateStructure/Automate/";

            // Création du fichier de la classe Automate
            string automate = this.ReadFile(innerPath+"Automate.cs");
            automate = automate.Replace("NomAutomate", documentFolderName);
            this.makeFile(path + "Automate.cs", automate);

            // Création du fichier de la classe State
            string etat = this.ReadFile(innerPath + "Etat.cs");
            etat = etat.Replace("NomAutomate", documentFolderName);
            this.makeFile(path + "Etat.cs", etat);

            // Création du fichier de l'énumération Evenement
            string eventEnum = this.ReadFile(innerPath + "Evenement.cs");
            eventEnum = eventEnum.Replace("NomAutomate", documentFolderName);
            string events = string.Join(", ", document.Events);
            events = events.Replace(" ", "_");
            events = events.ToUpper();
            eventEnum = eventEnum.Replace("//ListeDesEvents", events);
            this.makeFile(path+ "Evenement.cs", eventEnum);

            // Creéation du fichier de la classe Etat correspondant à chaque état du document
            path += "Etats/";
            string stateTemplate = this.ReadFile(innerPath + "Etats/EtatModels.cs");
            foreach (State state in document.States)
            {
                string specificEtat = stateTemplate;
                specificEtat = specificEtat.Replace("NomAutomate", documentFolderName);
                string stateName = this.formatNameClass(state.Name);
                specificEtat = specificEtat.Replace("EtatX", stateName);

                // Transition de l'Automate
                string cases = string.Empty;
                string caseTemplate = this.ReadFile(innerPath + "Etats/cases.cs");
                foreach (Transition transi in document.Transitions)
                {
                    if(transi.Start == state)
                    {
                        string caseCopy = caseTemplate;
                        caseCopy = caseCopy.Replace("etatARemplacer", transi.End.Name);
                        cases += caseCopy;
                    }
                }
                specificEtat = specificEtat.Replace("//cases", cases);
                this.makeFile(path + stateName +".cs", specificEtat);
            }

        }

        /// <summary>
        /// Retourne le contenue du fichier
        /// </summary>
        /// <param name="path">Chemin d'accès du fichier avec nom du fichier inclut</param>
        /// <returns>contenue du fichier</returns>
        private string ReadFile(string path)
        {
            string fileContent = "";
            try
            {
                StreamReader reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    fileContent += reader.ReadLine() + "\n";
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur s'est produite : " + e.Message);
            }
            return fileContent;
        }

        /// <summary>
        /// Créer un dossier avec le chemin fournit
        /// </summary>
        /// <param name="path">le chemin du dossier à créer avec le nom du dossier inclut</param>
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
        /// Créer un fichier avec le chemin fournit et écrit le contenue souhaité dedans
        /// </summary>
        /// <param name="path">le chemin du fichier avec le nom du fichier inclut</param>
        /// <param name="content">le contenue du fichier à écrire</param>
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
        /// Formate la chaine fourit en mettant en majuscule la première lettre de chaque mots et enlevant les espaces
        /// </summary>
        /// <param name="name">la chaine à formater</param>
        /// <returns>la chaine formatée</returns>
        private string formatNameClass(string name)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string newName = textInfo.ToTitleCase(name);
            newName = newName.Replace(" ", "");
            return newName;
        }
    }
}
