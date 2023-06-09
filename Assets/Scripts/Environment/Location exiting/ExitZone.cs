using myStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment.Location_exiting
{
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(ExitTimer))]
    public class ExitZone : MonoBehaviour
    {
        private BoxCollider _exitZone;
        private ExitTimer _exitTimer;
        [SerializeField] private bool _debugMode;

        public void Start()
        {
            _exitZone = GetComponent<BoxCollider>();
            _exitTimer = GetComponent<ExitTimer>();

            InitializeListenersOnExiting();
            InitializeExitZone();
            DisableMeshInRuntime();
        }

        private void DisableMeshInRuntime()
        {
            GetComponent<Renderer>().enabled = false;
        }

        private void InitializeExitZone()
        {
            if (!_exitZone.isTrigger)
                _exitZone.isTrigger = true;
        }

        private void InitializeListenersOnExiting()
        {
            _exitTimer.OnTimeElapsed.AddListener(LeaveLocation);
        }

        private void LeaveLocation()
        {
            const int mainMenuBuildIndex = 0;
            SceneManager.LoadScene(mainMenuBuildIndex);
        }


        public void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Character>(out var player)) return;
            _exitTimer.Restart();
            if (!_debugMode) return;
            Debug.Log("Вошёл в зону выхода");
        }

        public void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent<Character>(out var player)) return;
            _exitTimer.IncreaseTime(Time.fixedDeltaTime);
            if (!_debugMode) return;
            Debug.Log("Находится в зоне выхода");
        }

        public void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Character>(out var player)) return;
            if (!_debugMode) return;
            Debug.Log("Вышёл из зоны выхода");
        }
    }
}