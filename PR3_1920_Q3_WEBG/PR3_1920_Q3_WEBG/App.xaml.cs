using PR3_1920_Q3_WEBG.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PR3_1920_Q3_WEBG
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navigationPage = new NavigationPage(new MenuPage());
            MainPage = navigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("#1abbd4"); 
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
