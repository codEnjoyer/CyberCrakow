using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Shooting_range
{
    public class PolygonTarget : AbstractTarget
    {
        public override UnityEvent OnHit { get; protected set; }
        public override UnityEvent OnRecover { get; protected set; }
        public override UnityEvent OnDeath { get; protected set; }

        public override int Health { get; protected set; }
        public override int MaxHealth { get; protected set; } = 100;
        public bool IsStanding => Health != 0;

        private Animator _targetAnimator;

        private AudioSource _audioSource;
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private AudioClip _deathSound;
        [SerializeField] private AudioClip _recoverSound;
        [SerializeField] private float _secondToWaitBeforeRecover = 1f;

        private void Start()
        {
            _targetAnimator = GetComponentInParent<Animator>();

            _audioSource = GetComponent<AudioSource>();

            OnHit = new UnityEvent();
            OnDeath = new UnityEvent();
            OnRecover = new UnityEvent();

            Health = MaxHealth;
        }

        private void TakeDamage(int damage = 10)
        {
            Health = Math.Clamp(Health - damage, 0, MaxHealth);
            if (Health == 0)
                Die();
        }

        private void Die()
        {
            OnDeath?.Invoke();
            PlayDeathSound();
            _targetAnimator.Play("TargetDie");
            StartCoroutine(RecoverAfterDie());
        }

        public override void GetHit(int damage)
        {
            OnHit?.Invoke();
            TakeDamage(damage);
            PlayHitSound();
        }

        public override void Recover()
        {
            OnRecover?.Invoke();
            PlayRecoverSound();
            _targetAnimator.Play("TargetRecover");
            Health = MaxHealth;
        }

        private void PlayHitSound() => _audioSource.PlayOneShot(_hitSound);

        private void PlayDeathSound() => _audioSource.PlayOneShot(_deathSound);

        private void PlayRecoverSound() => _audioSource.PlayOneShot(_recoverSound);

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<Bullet>(out var bullet)) return;
            GetHit(MaxHealth);
        }

        private IEnumerator RecoverAfterDie()
        {
            yield return new WaitForSeconds(_secondToWaitBeforeRecover);
            Recover();
        }
    }
}