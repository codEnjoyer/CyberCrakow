using UnityEngine;
using UnityEngine.Events;

namespace Shooting_range
{
    
    public class PolygonTarget : AbstractTarget
    {
        public override UnityEvent OnHit { get; protected set; }
        private Rigidbody _rigidbody;
        private Animator _targetAnimator;
        [SerializeField] private AudioClip _hitSound;
        private AudioSource _audioSource;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;

            _targetAnimator = GetComponentInParent<Animator>();

            if (_hitSound is not null)
                _audioSource = gameObject.AddComponent<AudioSource>();
            
            OnHit = new UnityEvent();
            OnHit.AddListener(GetHit);
            OnHit.AddListener(PlayHitSound);
        }

        public override void GetHit()
        {
            _targetAnimator.Play("TargetHitted");
            Debug.Log("I got hit!");
        }

        private void PlayHitSound()
        {
            _audioSource.PlayOneShot(_hitSound);
        }

        public override void Recover()
        {
            _targetAnimator.Play("TargetRecover");
            Debug.Log("I got recover!");
        }
        
        private void OnCollisionEnter(Collision other)
        {
            // TODO: Прикрутить нормальный bullet
            if (!other.gameObject.TryGetComponent<Bullet>(out var bullet)) return;
            Debug.Log("Collided");
            OnHit?.Invoke();
        }
    }
}