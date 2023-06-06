using Shooting_range;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawner;

public class NPCHealth : MonoBehaviour
    {
        public float enemyHealth = 100;

        public SpawnerScript spawner;
        public void KillNPC()
        {
            Debug.Log("killNPC");
        }
        private void GetNPCDamage(float damage)
        {
            enemyHealth -= damage;
            if (enemyHealth <= 0)
            {
                KillNPC();
                Destroy(gameObject);
                spawner.SpawnTank();
            }
        }
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<Bullet>(out var bullet)) return;
            {
                GetNPCDamage(bullet.Damage);
                //Debug.Log("hit");
            }
        }
    }

