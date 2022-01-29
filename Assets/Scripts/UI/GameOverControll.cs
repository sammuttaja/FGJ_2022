using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FGJ_2022.UI
{
    public class GameOverControll : MonoBehaviour
    {
        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quiit()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}