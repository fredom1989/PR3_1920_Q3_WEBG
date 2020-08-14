using PR3_1920_Q3_WEBG.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PR3_1920_Q3_WEBG.Model
{
    public class HumidityData
    {
        public string Device_id { get; set; }
        public string Raw { get; set; }

        public string Payload { get; set; }
        public string PayloadBE { get { return Payload.FloatStringBEFormat(); } }

        // public double payloadFloat { get { return double.Parse(payload); } }
        public string Time { get; set; }

        public DateTime LocalTime { get
            {
                DateTime payloadTime;
                DateTime.TryParse(Time, out payloadTime);
                return payloadTime.ToLocalTime();
            } }

        // Ecart de temps entre la mesure et le moment de son "enregistrement" dans l'application. Affichage selon la norme  ISO 8601
        public String PayloadTimeSpan
        {
            get
            {
                DateTime actuel = DateTime.Now;
                DateTime payloadTime;
                DateTime.TryParse(Time, out payloadTime);
                TimeSpan spanTime = actuel.Subtract(payloadTime);
                String span = payloadTime.Year.ToString() + "-" + payloadTime.Month.ToString() + "-" + payloadTime.Day.ToString() + "/P" +
                spanTime.Days.ToString() + "DT" + spanTime.Hours.ToString() + "H" + spanTime.Minutes.ToString() + "M";
                return span;
            }
        }

        public String Image { get; set; }

    }
}
