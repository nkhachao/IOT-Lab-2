using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

        public void Exit()
        {
            Navigation.UnloadSceneAsync("ConnectionError");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
