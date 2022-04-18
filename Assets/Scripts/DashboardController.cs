using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TS1989
{
    public class DashboardController : MonoBehaviour
    {
        public TMP_Text lastUpdate;
        private static string LastUpdate = "";
        
        public TMP_Text temperature;
        private static string Temperature = "0";

        public TMP_Text humidity;
        private static string Humidity = "0";
        
        public Toggle ledToggle;
        private static bool? IsLedOn;
        
        public Toggle pumpToggle;
        private static bool? IsPumpOn;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }
        
        public void PublishLedStatus(bool status)
        {
            var ledStatus = status ? "ON" : "OFF";
            var message = "{\"device\":\"LED\",\"status\":\"" + ledStatus + "\"}";

            ConnectionController.ToBePublished.Add(new Message("/bkiot/1852346/led", message));
        }
        
        public void PublishPumpStatus(bool status)
        {
            IsPumpOn = status;
            var pumpStatus = status ? "ON" : "OFF";
            var message = "{\"device\":\"PUMP\",\"status\":\"" + pumpStatus + "\"}";

            ConnectionController.ToBePublished.Add(new Message("/bkiot/1852346/pump", message));
        }

        public static void ProcessMessage(Message message)
        {
            LastUpdate = DateTime.Now.ToString("HH:mm:ss");
        }

        // Update is called once per frame
        void Update()
        {
            lastUpdate.text = "Last update: " + LastUpdate;
            temperature.text = Temperature + "°C";
            humidity.text = Humidity + "%";

            if (IsLedOn != null)
            {
                ledToggle.isOn = IsLedOn.Value;
                IsLedOn = null;
            }

            if (IsPumpOn != null)
            {
                pumpToggle.isOn = IsPumpOn.Value;
                IsPumpOn = null;
            }
        }
    }
}
