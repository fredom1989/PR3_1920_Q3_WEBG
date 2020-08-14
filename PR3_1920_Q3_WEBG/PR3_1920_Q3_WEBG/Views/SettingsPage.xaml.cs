using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.Xaml;

namespace PR3_1920_Q3_WEBG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : Xamarin.Forms.TabbedPage
    {
    
        public SettingsPage()
        {
            
            this.Title = "Paramètres";
            
            BarBackgroundColor = Color.FromHex("#f4efef");
            BarTextColor = Color.Black;
            
                // Permet d'obtenir les icones dans le TabMenu pour l'application UWP
            On<Windows>().SetHeaderIconsEnabled(true);
            On<Windows>().SetHeaderIconsSize(new Size(30, 30));    

            Children.Add(new TabServerSettings { Title = "SERVEUR", IconImageSource = "server_Black_icon.png" });
            Children.Add(new TabAppSettings() { Title = "APPLICATION", IconImageSource = "app_Black_icon.png" });

        }


    }
}