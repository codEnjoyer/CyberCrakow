using UnityEngine;

namespace Shooting_range
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]

    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        public int Damage;
        // и тд
    }
}