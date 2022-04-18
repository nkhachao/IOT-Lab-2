using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TS1989
{
    public class Navigation : MonoBehaviour
    {
        static private string currentScene;

        public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            if (currentScene != null)
            {
                UnloadSceneAsync(currentScene);
            }
            
            SceneManager.LoadScene(sceneName, loadSceneMode);
            currentScene = sceneName;
        }

        public static void UnloadSceneAsync(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            currentScene = null;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

    public class SceneNames
    {
        public const string Login = "Login";
        public const string ConnectionError = "ConnectionError";
        public const string Connecting = "Hourglass";
        public const string Dashboard = "Dashboard";
    }
}
