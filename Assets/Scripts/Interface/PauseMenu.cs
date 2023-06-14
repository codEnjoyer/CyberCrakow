using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using myStateMachine;
namespace Interface
{
    [RequireComponent(typeof(Canvas))]
    public class PauseMenu : MonoBehaviour
    {
        /// <summary>
        /// Должен находиться в Canvas
        /// </summary>
        public static bool GameIsPaused { get; private set; }
        [SerializeField]private GameObject _pauseMenuUI;
        [SerializeField] public Character character;
        PlayerInput pauseInput;

        private void Awake()
        {
            pauseInput = new PlayerInput();
        }

        private void Start()
        {
            _pauseMenuUI = transform.Find("Pause Menu").gameObject;
            pauseInput.Player.Pause.performed += OnPauseKeyPressed;
        }

        private void OnPauseKeyPressed(InputAction.CallbackContext obj) => SwitchPauseMenu();

        private void SwitchPauseMenu()
        {
            Debug.Log("switch");
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }

        private void Pause()
        {
            OpenPauseMenu(true);
        }

        public void Resume()
        {
            OpenPauseMenu(false);
        }

        public void Quit()
        {
            const int mainMenuIndex = 0;
            SceneManager.LoadScene(mainMenuIndex);
        }

        private void OpenPauseMenu(bool enable)
        {
            _pauseMenuUI.SetActive(enable);
            Cursor.visible = enable;

                Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
             

            Time.timeScale = enable ? 0f : 1f;
            GameIsPaused = enable;
            if (enable)
                character.input.Disable();
            else character.input.Enable();
        }

        private void OnEnable() => pauseInput.Enable();

        private void OnDisable() => pauseInput.Disable();
    }
}