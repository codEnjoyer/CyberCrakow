using System;
using UnityEngine;
using UnityEngine.Events;

namespace Shooting_range
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class AbstractTarget : MonoBehaviour, ITarget
    {
        public abstract UnityEvent OnHit { get; protected set; }

        public virtual void GetHit()
        {
            throw new NotImplementedException();
        }

        public virtual void Recover()
        {
            throw new NotImplementedException();
        }

    }
}