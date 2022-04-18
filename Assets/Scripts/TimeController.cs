using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TS1989
{
    public class TimeController : MonoBehaviour
    {
        public TMP_Text text;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            text.text = System.DateTime.Now.ToString("HH:mm:ss");
        }
    }
}

