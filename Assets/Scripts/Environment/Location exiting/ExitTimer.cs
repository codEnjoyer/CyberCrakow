using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Environment.Location_exiting
{
    [RequireComponent(typeof(ExitZone))]
    public class ExitTimer : MonoBehaviour
    {
        private float _timeInsideExitZone;
        public UnityEvent OnTimeElapsed { get; private set; }
        [SerializeField] private bool _debugMode;
        [SerializeField] private float _requiredTimeInsideExitZone = 3f;
        [SerializeField] private TMP_Text _timerText;

        private void Start()
        {
            OnTimeElapsed = new UnityEvent();
        }

        public void Restart() => _timeInsideExitZone = 0f;

        public void IncreaseTime(float time)
        {
            _timeInsideExitZone += time;
            var remainingTime = Mathf.Max(0f, _requiredTimeInsideExitZone - _timeInsideExitZone);
            // Mathf.Max() используется для того, чтобы при последнем тике не отображалось отрицательное значение
            _timerText.text = remainingTime.ToString("F", CultureInfo.InvariantCulture);
            CheckForElapsedTime();
            if (_debugMode)
                Debug.Log($"Находится в зоне выхода в течение {_timeInsideExitZone} секунд");
        }

        private void CheckForElapsedTime()
        {
            if (_timeInsideExitZone < _requiredTimeInsideExitZone) return;
            OnTimeElapsed?.Invoke();
        }
    }
}