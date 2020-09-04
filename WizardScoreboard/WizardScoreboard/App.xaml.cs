using System;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Google.Cloud.Firestore;

namespace WizardScoreboard
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Current.MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
