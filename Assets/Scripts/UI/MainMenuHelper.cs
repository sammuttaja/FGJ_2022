using UnityEngine;
using UnityEngine.EventSystems;

namespace FGJ_2022.UI
{
    [AddComponentMenu("FGJ_2022/UI/ Main Menu Helper")]
    public class MainMenuHelper : MonoBehaviour
    {
        [SerializeField]
        private GameObject startButton;

        // Start is called before the first frame update
        void Start()
        {
            EventSystem.current.SetSelectedGameObject(startButton);
        }
    }
}