using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceCats
{
    public static class M_LoadScene
    {
        public static void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public static void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}

