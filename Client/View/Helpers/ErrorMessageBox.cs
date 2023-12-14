using Grpc.Core;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AutomateDesign.Client.View.Helpers
{
    /// <summary>
    /// Permet d'afficher une boîte de dialogue en cas d'erreur.
    /// </summary>
    internal static class ErrorMessageBox
    {
        /// <summary>
        /// Affiche une boîte de dialogue pour une erreur.
        /// </summary>
        public static void Show(Exception? error)
        {
            string message;
            if (error is RpcException rpce)
            {
                switch (rpce.StatusCode)
                {
                    case StatusCode.Unavailable:
                        message = "Le serveur n'est pas disponible.";
                        break;

                    default:
                        message = rpce.Status.Detail;
                        break;
                }
            }
            else if (error is Exception e)
            {
                message = $"Une erreur innatendue est survenue ({e.Message})";
            }
            else
            {
                message = "Une erreur inconnue est survenue";
            }
            MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Gère les erreurs levée dans une méthode asynchrone.
        /// </summary>
        /// <param name="asyncAction">La méthode asyncrhone à encapsuler.</param>
        /// <returns>Une tâche représentant l'opération à gérer.</returns>
        public static Action HandleAsyncActionErrors(Func<Task?> asyncAction)
        {
            return () =>
            {
                try
                {
                    asyncAction()?.Wait();
                }
                catch (Exception ex)
                {
                    Show(ex);
                }
            };
        }
    }
}
