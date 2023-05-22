using UnityEngine;
using UnityEngine.Events;

namespace Shooting_range
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Target : MonoBehaviour
    {
        public UnityEvent OnHit { get; private set; }
        private Rigidbody _rigidbody;
        private Animator _targetAnimator;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;

            _targetAnimator = GetComponentInParent<Animator>();

            OnHit = new UnityEvent();
            OnHit.AddListener(GetHit);
        }

        public void GetHit()
        {
            _targetAnimator.Play("TargetHitted");
            Debug.Log("I got hit!");
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<Bullet>(out var bullet)) return;
            Debug.Log("Collided");
            OnHit?.Invoke();
        }
    }
}