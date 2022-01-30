using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FGJ_2022.UI
{
    [AddComponentMenu("FGJ_2022/UI/ Quit Control")]
    public class QuitControl : MonoBehaviour
    {
        [SerializeField]
        private Button yesButton;

        [SerializeField]
        private Button noButton;

        // Start is called before the first frame update
        void Start()
        {
            yesButton.onClick.AddListener(ToMenu);
            noButton.onClick.AddListener(HideQuitUI);
        }

        /// <summary>
        /// Moves to the main menu.
        /// </summary>
        private void ToMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        /// <summary>
        /// Hides the quit game UI.
        /// </summary>
        private void HideQuitUI()
        {
            gameObject.SetActive(false);
        }
    }
}
