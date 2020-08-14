using PR3_1920_Q3_WEBG.Extensions;
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
    public partial class TabServerSettings : ContentPage
    {
            
        string ttn_App_Acces_Key = "";
        string swagger_Query_Base = "";
        string swagger_Query_Time = "";

            /* Application.Current.properties
            * Ensemble des clés sous lesquels sont enregistrée les propriétés de l'application, conservées au redémarrage.
            */
        const string KEY_TTN_APP_ACCES_KEY = "ttnAppAccesKey";
        const string KEY_SWAGGER_QUERY_BASE = "swagger_Quey_Base";
        const string KEY_SWAGGER_QUERY_TIME = "swagger_Quey_Time";


        public TabServerSettings()
        {
            InitializeComponent();
                
            if (Application.Current.Properties.ContainsKey(KEY_TTN_APP_ACCES_KEY))
                ttn_App_Acces_Key = (string)Application.Current.Properties[KEY_TTN_APP_ACCES_KEY];
            if (Application.Current.Properties.ContainsKey(KEY_SWAGGER_QUERY_BASE))
                swagger_Query_Base = (string)Application.Current.Properties[KEY_SWAGGER_QUERY_BASE];
            if (Application.Current.Properties.ContainsKey(KEY_SWAGGER_QUERY_TIME))
            {
                swagger_Query_Time = (string)Application.Current.Properties[KEY_SWAGGER_QUERY_TIME];
                    // ?last= est nécessaire dans les requêtes à Swagger quand il y a un paramètre de temps, à l'affichage on le retire
                swagger_Query_Time = swagger_Query_Time.SuppresBefor('=');
            }
            RefreshEntryContent();
        }

            //Rafraichissement des ENTRY selon les propriétés de l'applications récupéré
        public void RefreshEntryContent()
        {
            TtnAppKey.Text = ttn_App_Acces_Key;
            swaggerQueryBase.Text = swagger_Query_Base;
            swaggerQueryTime.Text = swagger_Query_Time;
        }
            /* Sauvegarde des paramètres
            * ?last= est nécessaire dans les requêtes à Swagger, on l'ajoute au paramètre de l'application relatif au temps voulu par l'utilisateur
             *Si le champs est remit à blanc, on revient à une requête par défaut.*/
        private void ValidationButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties[KEY_TTN_APP_ACCES_KEY] = TtnAppKey.Text;
            Application.Current.Properties[KEY_SWAGGER_QUERY_BASE] = swaggerQueryBase.Text;
            if (swaggerQueryTime.Text != null && swaggerQueryTime.Text != "")
                Application.Current.Properties[KEY_SWAGGER_QUERY_TIME] = "?last=" + swaggerQueryTime.Text;
            else
                Application.Current.Properties[KEY_SWAGGER_QUERY_TIME] = "";
            Application.Current.SavePropertiesAsync();

        }        
    }
}