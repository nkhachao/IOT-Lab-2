using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TS1989
{
    public class HourglassController : MonoBehaviour
    {
        // Start is called before the first frame update
        public static string Status;
        public TMP_Text status;
        void Start()
        {
            status.text = Status;
        }

        // Update is called once per frame
        void Update()
        {
            status.text = Status;
        }
    }
}

