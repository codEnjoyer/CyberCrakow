using UnityEngine.Events;

namespace Shooting_range
{
    public interface ITarget
    {
        public UnityEvent OnHit { get; }
        public UnityEvent OnRecover { get; }
        public UnityEvent OnDeath { get; }
        public void GetHit(int damage);
        public void Recover();
    }
}