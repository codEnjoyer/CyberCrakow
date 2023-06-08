using myStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Environment.Location_exiting
{
    [RequireComponent(typeof(Renderer))]
    [RequireComponent(typeof(BoxCollider))]
    public class ExitZone : MonoBehaviour
    {
        private BoxCollider _exitZone;
        private float _timeInsideExitZone;
        [FormerlySerializedAs("_debug")] [SerializeField] private bool _debugMode;
        [SerializeField] private float _requiredTimeInsideExitZone;
        public void Start()
        {
            _exitZone = GetComponent<BoxCollider>();
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

        private void LeaveLocation()
        {
            const int mainMenuBuildIndex = 0;
            SceneManager.LoadScene(mainMenuBuildIndex);
        }

        private void IncreaseTimeInsideExitZone()
        {
            _timeInsideExitZone += Time.fixedDeltaTime;
            if (!_debugMode) return;
            Debug.Log($"Находится в зоне выхода в течение {_timeInsideExitZone} секунд");
        }

        private void CheckForLeaving()
        {
            if (_timeInsideExitZone >= _requiredTimeInsideExitZone)
                LeaveLocation();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Character>(out var player)) return;
            _timeInsideExitZone = 0f;
            if (!_debugMode) return;
            Debug.Log("Вошёл в зону выхода");
        }

        public void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent<Character>(out var player)) return;
            if (!_debugMode) return;
            Debug.Log("Находится в зоне выхода");
            IncreaseTimeInsideExitZone();
            CheckForLeaving();
        }

        public void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Character>(out var player)) return;
            if (!_debugMode) return;
            Debug.Log("Вышёл из зоны выхода");
        }
    }
}