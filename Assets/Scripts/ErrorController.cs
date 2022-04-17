using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TS1989
{
    public class ErrorController : MonoBehaviour
    {
        public static string ErrorMessage;
        public TMP_Text errorMessage;
        // Start is called before the first frame update
        void Start()
        {
            errorMessage.text = ErrorMessage;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
