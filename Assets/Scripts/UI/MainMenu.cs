using UnityEngine;
using UnityEngine.SceneManagement;

namespace FGJ_2022.UI
{
    [AddComponentMenu("FGJ_2022/UI Main Menu")]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject scoreBoard;

        // Start is called before the first frame update
        void Start()
        {

        }

        /// <summary>
        /// Starts the game!
        /// </summary>
        public void StartGameButton()
        {
            SceneManager.LoadSceneAsync("Tutorial");
        }

        /// <summary>
        /// Activate score board.
        /// </summary>
        public void ScoresButton()
        {
            if (scoreBoard.activeSelf == false)
            {
                scoreBoard.SetActive(true);
            }
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
