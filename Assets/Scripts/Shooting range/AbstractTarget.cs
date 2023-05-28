using System;
using UnityEngine;
using UnityEngine.Events;

namespace Shooting_range
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class AbstractTarget : MonoBehaviour, ITarget
    {
        public abstract UnityEvent OnHit { get; protected set; }
        public abstract UnityEvent OnRecover { get; protected set; }
        public abstract UnityEvent OnDeath { get; protected set; }
        public abstract int Health { get; protected set; }
        public abstract int MaxHealth { get; protected set; }

        public virtual void GetHit(int damage)
        {
            throw new NotImplementedException();
        }

        public virtual void Recover()
        {
            throw new NotImplementedException();
        }
    }
}