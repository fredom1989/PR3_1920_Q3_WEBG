using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PR3_1920_Q3_WEBG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabAppSettings : ContentPage
    {
         //  for non static server param
        string okValue="";
        string dangerValue="";

            // Application.Current.properties
            //  for non static server param
        const string KEY_OK_VALUE = "okValue";
        const string KEY_DANGER_VALUE = "dangerValue";
        public TabAppSettings()
        {
            InitializeComponent();

            if(Application.Current.Properties.ContainsKey(KEY_OK_VALUE))
                okValue = (String)Application.Current.Properties[KEY_OK_VALUE];            
            if (Application.Current.Properties.ContainsKey(KEY_DANGER_VALUE))
                dangerValue = (String)Application.Current.Properties[KEY_DANGER_VALUE];                    
            RefreshEntryContent();
        }
        public void RefreshEntryContent()
        {
            OkValueEntry.Text = okValue;
            DangerValueEntry.Text = dangerValue.ToString();
        }

        private void BadValueAlert()
        {
            DisplayAlert("Entrée invalide ou incohérente!", "Le nombres doivent être des entiers et la valeur OK inférieur à la valeur DANGER", "Annuler");
        }
        private void ValidationButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(OkValueEntry.Text.ToString()) < int.Parse(DangerValueEntry.Text.ToString()))
                {
                    Application.Current.Properties[KEY_OK_VALUE] = OkValueEntry.Text;
                    Application.Current.Properties[KEY_DANGER_VALUE] = DangerValueEntry.Text;
                    Application.Current.SavePropertiesAsync();
                }
                else
                {
                    BadValueAlert();
                }                  
            }
            catch (Exception ex)
            {
                BadValueAlert();
            }
        }
    }
}