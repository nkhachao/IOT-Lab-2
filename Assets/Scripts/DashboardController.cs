using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TS1989
{
    public class DashboardController : MonoBehaviour
    {
        public TMP_Text lastUpdate;
        public static string LastUpdate = "";
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            lastUpdate.text = "Last update: " + LastUpdate;
        }
    }
}
