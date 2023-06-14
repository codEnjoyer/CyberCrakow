using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Interface
{
    [RequireComponent(typeof(Canvas))]
    public class PauseMenu : MonoBehaviour
    {
        /// <summary>
        /// Должен находиться в Canvas
        /// </summary>
        public static bool GameIsPaused { get; private set; }
        private GameObject _pauseMenuUI;
        private PlayerInput _controls;

        private void Awake()
        {
            _controls = new PlayerInput();
        }

        private void Start()
        {
            _pauseMenuUI = transform.Find("Pause Menu").gameObject;
            _controls.Player.Pause.performed += OnPauseKeyPressed;
        }

        private void OnPauseKeyPressed(InputAction.CallbackContext obj) => SwitchPauseMenu();

        private void SwitchPauseMenu()
        {
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
            Time.timeScale = enable ? 0f : 1f;
            GameIsPaused = enable;
        }

        private void OnEnable() => _controls.Enable();

        private void OnDisable() => _controls.Disable();
    }
}