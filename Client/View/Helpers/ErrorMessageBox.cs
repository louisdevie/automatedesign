using Grpc.Core;
using System;
using System.Windows;

namespace AutomateDesign.Client.View.Helpers
{
    internal class ErrorMessageBox
    {
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
            MessageBox.Show(message, "Erreur", MessageBoxButton.OK);
        }
    }
}
