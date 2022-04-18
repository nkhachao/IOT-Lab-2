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
        private static bool IsLedOn = false;
        
        public Toggle pumpToggle;
        private static bool IsPumpOn = false;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public static void ProcessMessage(string message)
        {
            LastUpdate = DateTime.Now.ToString("HH:mm:ss");
        }

        // Update is called once per frame
        void Update()
        {
            lastUpdate.text = "Last update: " + LastUpdate;
            temperature.text = Temperature + "°C";
            humidity.text = Humidity + "%";
            ledToggle.isOn = IsLedOn;
            pumpToggle.isOn = IsPumpOn;
        }
    }
}
