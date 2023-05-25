using UnityEngine.Events;

namespace Shooting_range
{
    public interface ITarget
    {
        public UnityEvent OnHit { get; }
        public void GetHit();
        public void Recover();
    }
}