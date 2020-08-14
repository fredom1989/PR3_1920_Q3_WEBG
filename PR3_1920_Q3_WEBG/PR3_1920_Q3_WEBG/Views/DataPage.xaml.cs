using PR3_1920_Q3_WEBG.Extensions;
using PR3_1920_Q3_WEBG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PR3_1920_Q3_WEBG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataPage : ContentPage
    {
        String okValue = "";
        String dangerValue = "";

            // Les différents tris sur les donnée. Il sera "None" par défaut, en l'absence de configuration enregistrée
        public enum E_TRI
        {
            SORT_NONE,
            SORT_TIME_NEW_OLD,
            SORT_TIME_OLD_NEW,
            SORT_VALUE_MAX_MIN,
            SORT_VALUE_MIN_MAX
        }
        E_TRI tri = E_TRI.SORT_NONE;

        List<HumidityData> humidityData;

            /* Paramètres pour la requête au service swagger. Query_Base est la requêtre sans paramètre de temps spécifié sur le site
             * Querry_Time : combien de temps en arîère l'on veut récupérer les données (1h) sans paramètre.
             */
        string ttnAppAccesKey = "";
        string swagger_Query_Base = "";
        string swagger_Query_Time = "";

            /* Application.Current.properties
             * Ensemble des clés sous lesquels sont enregistrée les propriétés de l'application, conservées au redémarrage.
             */       
        const string KEY_TRI = "tri";
        const string KEY_TTN_APP_ACCES_KEY = "ttnAppAccesKey";
        const string KEY_SWAGGER_QUERY_BASE = "swagger_Quey_Base";
        const string KEY_SWAGGER_QUERY_TIME = "swagger_Quey_Time";
        const string KEY_OK_VALUE = "okValue";
        const string KEY_DANGER_VALUE = "dangerValue";
        public DataPage()
        {
            InitializeComponent();

                // Garnisage des valeurs selon la présence de propriétées sauvegardée
            if (Application.Current.Properties.ContainsKey(KEY_TRI))
            {
                tri = (E_TRI)Application.Current.Properties[KEY_TRI];
                triButton.Source = GetImageSourceFromSort(tri);
            }
            if (Application.Current.Properties.ContainsKey(KEY_TTN_APP_ACCES_KEY))
                ttnAppAccesKey = (string)Application.Current.Properties[KEY_TTN_APP_ACCES_KEY];
            if (Application.Current.Properties.ContainsKey(KEY_SWAGGER_QUERY_BASE))
                swagger_Query_Base = (string)Application.Current.Properties[KEY_SWAGGER_QUERY_BASE];
            if (Application.Current.Properties.ContainsKey(KEY_SWAGGER_QUERY_TIME))
                swagger_Query_Time = (string)Application.Current.Properties[KEY_SWAGGER_QUERY_TIME];
            if (Application.Current.Properties.ContainsKey(KEY_OK_VALUE))
                okValue = (String)Application.Current.Properties[KEY_OK_VALUE];
            if (Application.Current.Properties.ContainsKey(KEY_DANGER_VALUE))
                dangerValue = (String)Application.Current.Properties[KEY_DANGER_VALUE];
                
            
            MyListView.IsVisible = false;
            waitLayout.IsVisible = true;

            MyListView.RefreshCommand = new Command((obj) =>
            {
                TelechargementData((data) =>
                {
                    SetHumidityDataImages();
                    MyListView.ItemsSource = GetHumidityDataFromSort(tri, humidityData);
                    MyListView.EndRefresh();
                });

            });


            TelechargementData((data) =>
            {
                SetHumidityDataImages();
                MyListView.ItemsSource = GetHumidityDataFromSort(tri, humidityData);
                MyListView.IsVisible = true;
                waitLayout.IsVisible = false;
            });

                // Création d'une vue d'alerte à la selection d'un item, on affiche ses détails.
            MyListView.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;
                HumidityData item = MyListView.SelectedItem as HumidityData;
                DisplayAlert("Détails du relevé", "Device ID : " +item.Device_id +"\nValeur : " + item.PayloadBE + "%" + "\nMoment du relevé : " + item.LocalTime.ToString() + "\nInterval de temps avec le relevé : " + item.PayloadTimeSpan, "OK");
                MyListView.SelectedItem = null;
            };
        }
            // Uniquement pour UWP Remplace le pull to refresh pour les apapreil sans écran tactile
        void RefreshButton_Clicked(object sender, EventArgs e)
        {
            TelechargementData((data) =>
            {
                SetHumidityDataImages();
                MyListView.ItemsSource = GetHumidityDataFromSort(tri, humidityData);
            });
        }
            // Au clique sur l'image représentant le tri, on change de tri, rafrachit la liste d'itemss et au sauvegarde la nouveaux paramètre de tri
        private void TriButton_Clicked(object sender, EventArgs e)
        {
            switch (tri)
            {
                case E_TRI.SORT_NONE:
                    tri = E_TRI.SORT_TIME_NEW_OLD;
                    break;
                case E_TRI.SORT_TIME_NEW_OLD:
                    tri = E_TRI.SORT_TIME_OLD_NEW;
                    break;
                case E_TRI.SORT_TIME_OLD_NEW:
                    tri = E_TRI.SORT_VALUE_MAX_MIN;
                    break;
                case E_TRI.SORT_VALUE_MAX_MIN:
                    tri = E_TRI.SORT_VALUE_MIN_MAX;
                    break;
                case E_TRI.SORT_VALUE_MIN_MAX:
                    tri = E_TRI.SORT_NONE;
                    break;
                default: break;
            }
            triButton.Source = GetImageSourceFromSort(tri);
            MyListView.ItemsSource = GetHumidityDataFromSort(tri, humidityData);

            Application.Current.Properties[KEY_TRI] = (int)tri;

            Application.Current.SavePropertiesAsync();
        }
            //Détermine qu'elle image afficher pour représneter le tri en fonction du paramètre de ce dernier.
        public string GetImageSourceFromSort(E_TRI t)
        {
            switch (t)
            {
                case E_TRI.SORT_NONE:
                    return "sort_none.png";
                case E_TRI.SORT_TIME_NEW_OLD:
                    return "sort_time_new_old.png";
                case E_TRI.SORT_TIME_OLD_NEW:
                    return "sort_time_old_new.png";
                case E_TRI.SORT_VALUE_MAX_MIN:
                    return "sort_value_max_min.png";
                case E_TRI.SORT_VALUE_MIN_MAX:
                    return "sort_value_min_max.png";
                default: return "sort_none.png";
            }
        }
            // Envois une liste triée des doonnées sur base du paramètre dy tri
        public List<HumidityData> GetHumidityDataFromSort(E_TRI t, List<HumidityData> l)
        {
            if (l == null)
                return null;

            List<HumidityData> listData = new List<HumidityData>(l);

            switch (t)
            {
                case E_TRI.SORT_NONE:
                    return listData;
                case E_TRI.SORT_TIME_NEW_OLD:
                    {
                        listData.Sort((p1, p2) => { return p2.Time.CompareTo(p1.Time); });
                        return listData;
                    }
                case E_TRI.SORT_TIME_OLD_NEW:
                    {
                        listData.Sort((p1, p2) => { return p1.Time.CompareTo(p2.Time); });
                        return listData;
                    }
                case E_TRI.SORT_VALUE_MAX_MIN:
                    {
                        listData.Sort((p1, p2) => { return double.Parse(p2.Payload).CompareTo(double.Parse(p1.Payload)); });
                        return listData;
                    }
                case E_TRI.SORT_VALUE_MIN_MAX:
                    {
                        listData.Sort((p1, p2) => { return double.Parse(p1.Payload).CompareTo(double.Parse(p2.Payload)); });
                        return listData;
                    }
                default: return listData;
            }

        }
            /* Détermine l'emplecement de l'image de la données selon les paramètres de l'application.
             * En l'absnece de paramètre c'est la verion transparente qui est utilisée
             * !!! BUG constaté avec une avec float.Parse sur les Androïd ARM ==> utilisation de la valeur entière
             */
        public void SetHumidityDataImages()
        {
            if (humidityData != null)
            {
                for (int i = 0; i < humidityData.Count; i++)
                {
                    if (!Application.Current.Properties.ContainsKey(KEY_OK_VALUE) || !Application.Current.Properties.ContainsKey(KEY_DANGER_VALUE))
                        humidityData[i].Image = "transparent_drop.png";
                    else if (int.Parse(humidityData[i].Payload.FloatStringToInt()) < int.Parse(okValue))
                    {
                        humidityData[i].Image = "blue_drop.png";
                    }
                    else if (int.Parse(humidityData[i].Payload.FloatStringToInt()) < int.Parse(dangerValue))
                        humidityData[i].Image = "orange_drop.png";
                    else
                        humidityData[i].Image = "red_drop.png";
                }
            }

        }

        public void TelechargementData(Action<List<HumidityData>> action)
        {
            using (var webClient = new System.Net.WebClient())
            {
                String url = swagger_Query_Base + swagger_Query_Time;

                webClient.DownloadStringCompleted += (object sender, System.Net.DownloadStringCompletedEventArgs e) =>
                {
                    try
                    {
                        String humidityDataJson = e.Result;
                        humidityData = JsonConvert.DeserializeObject<List<HumidityData>>(humidityDataJson);
                        SetHumidityDataImages();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            action.Invoke(humidityData);
                        });
                    }
                    catch (Exception ex)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Erreur", ex.Message, "ok");
                            action.Invoke(null);
                        });
                    }

                };
                webClient.Headers.Add("Accept", "application/json"); 
                webClient.Headers.Add("Authorization", ("key " + ttnAppAccesKey));
                try
                {
                    webClient.DownloadStringAsync(new Uri(url));
                }
                catch (Exception ex)
                {
                    DisplayAlert("Erreur", ex.Message, "ok");
                    action.Invoke(null);
                }

            }
        }

    }
}