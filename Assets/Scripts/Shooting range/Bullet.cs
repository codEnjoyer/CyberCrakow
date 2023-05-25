using UnityEngine;

namespace Shooting_range
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]

    public class Bullet : MonoBehaviour
    {
        public int Damage { get; private set; }
        // и тд
    }
}