using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace FGJ_2022.UI
{
    [AddComponentMenu("FGJ_2022/UI/ Main Menu")]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject scoreBoard;

        [SerializeField]
        private GameObject continueBoard;

        [SerializeField]
        private GameObject continueButton;

        [SerializeField]
        private GameObject menuButtonContainer;

        /// <summary>
        /// Starts the game!
        /// </summary>
        public void PressAnyKeyButton()
        {
            SceneManager.LoadSceneAsync("Tutorial");
        }

        /// <summary>
        /// Opens the press any key to continue board.
        /// </summary>
        public void StartGameButton()
        {
            menuButtonContainer.SetActive(false);
            scoreBoard.SetActive(false);
            continueBoard.SetActive(true);
            EventSystem.current.SetSelectedGameObject(continueButton);
        }

        /// <summary>
        /// Activate score board.
        /// </summary>
        public void ScoresButton()
        {
            if (scoreBoard.activeSelf == false)
                scoreBoard.SetActive(true);
            else if (scoreBoard.activeSelf == true)
                scoreBoard.SetActive(false);
        }

        /// <summary>
        /// Quit the game.
        /// </summary>
        public void ExitGameButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
