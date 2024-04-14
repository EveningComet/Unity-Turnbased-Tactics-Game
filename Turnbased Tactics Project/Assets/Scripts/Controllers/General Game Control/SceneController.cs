using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TurnbasedGame.SceneManagement
{
    /// <summary>
    /// Responsible for loading the game's scenes.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// Stores the name of the scene that holds the game's level.
        /// </summary>
        private const string gameLevelSceneName = "Game";

        /// <summary>
        /// Play the game's level.
        /// </summary>
        public void PlayGame()
        {
            LoadScene(gameLevelSceneName);
        }

        /// <summary>
        /// Load the scene with the passed name.
        /// </summary>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Quit the game.
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            // Quit in the editor
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
