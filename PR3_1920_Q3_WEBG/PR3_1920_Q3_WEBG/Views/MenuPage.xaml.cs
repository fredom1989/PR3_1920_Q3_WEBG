using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PR3_1920_Q3_WEBG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void DataButtonClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new DataPage());
        }
        private void SettingButtonClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new SettingsPage());
        }
    }

}