﻿using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using AutomateDesign.Client.View.Navigation;
using AutomateDesign.Client.ViewModel;
using AutomateDesign.Client.ViewModel.Documents;
using AutomateDesign.Client.ViewModel.Users;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace AutomateDesign.Client.View.Pages
{
    public partial class HomeView : NavigablePage
    {
        private SessionViewModel? sessionVM;
        private DocumentCollectionViewModel documentsVM;

        public Observable<string> CurrentUserEmail { get; }


        public ObservableCollection<DocumentBaseViewModel> Documents => this.documentsVM;

        public override WindowPreferences Preferences => new(
            WindowPreferences.WindowSize.Large,
            WindowPreferences.ResizeMode.Resizeable
        );

        public HomeView()
        {
            this.CurrentUserEmail = new Observable<string>(string.Empty);
            this.documentsVM = new DocumentCollectionViewModel();
            DataContext = this;

            InitializeComponent();
        }

        public override void OnNavigatedToThis(bool clearedHistory)
        {
            if (this.Navigator.Session is Session session)
            {
                this.sessionVM = new SessionViewModel(session);
                this.CurrentUserEmail.Value = this.sessionVM.UserEmail.Split('@', 2)[0];
                this.documentsVM.Session = session;
                this.menuUser.SessionVM = this.sessionVM;
                this.menuUser.Navigator = this.Navigator;
                Task.Run(ErrorMessageBox.HandleAsyncActionErrors(this.documentsVM.Reload));
            }
        }

        private void NewDocumentClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Go(new EditAutomateView(this.documentsVM.NewDocument(), sessionVM!));
        }

        private async void ExistingDocumentClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button { CommandParameter: ExistingDocumentViewModel param })
            {
                ProgressDialog popup = new(
                    title: "Chargement",
                    progressMessage: "Ouverture du document..."
                );

                Task backgroundLoading = param.Load(popup);

                if (popup.ShowDialog() == true)
                {
                    await backgroundLoading;
                    this.Navigator.Go(new EditAutomateView(param, sessionVM!));
                }
            }
        }

        private async void SignOut(object sender, RoutedEventArgs e)
        {
            await this.sessionVM!.SignOutAsync();
            this.Navigator.Go(new SignInView(),true);
        }
        
    }
}
