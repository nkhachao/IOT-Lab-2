using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TS1989
{
    // Everything in this class is static
    public class Navigation : MonoBehaviour
    {
        static private List<string> activeScenes = new List<string> { SceneNames.Login };

        public static void LoadScene(string newScene, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            if (IsPopup(newScene))  // When adding a new popup, remove all current active popup
            {
                var activePopups = activeScenes.FindAll(scene => IsPopup(scene));
                foreach (string scene in activePopups)
                {
                    UnloadSceneAsync(scene);
                }
            }
            else   // When adding a new scene, remove all other scene except root scene
            {
                var nonRootScenes = activeScenes.FindAll(scene => !IsRootScene(scene));
                foreach (string scene in nonRootScenes)
                {
                    UnloadSceneAsync(scene);
                }
            }

            SceneManager.LoadScene(newScene, loadSceneMode);
            activeScenes.Add(newScene);
        }

        public static void UnloadSceneAsync(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            activeScenes.Remove(sceneName);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private static bool IsPopup(string sceneName)
        {
            return sceneName == SceneNames.Connecting || sceneName == SceneNames.Error;
        }
        
        private static bool IsRootScene(string sceneName)
        {
            return sceneName == SceneNames.Login;
        }
    }

    public class SceneNames
    {
        public const string Login = "Login";
        public const string Error = "Error";
        public const string Connecting = "Hourglass";
        public const string Dashboard = "Dashboard";
    }
}
