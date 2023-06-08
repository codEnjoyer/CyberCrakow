using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawner
{
    public class SpawnerScript : MonoBehaviour
    {
        public GameObject tankPrefab;
        public Transform playerPosition;
        public Transform[] waypoints;
        public NPCMovement tank;
        private NPCHealth health;
        public void SpawnTank()
        {
            tank = tankPrefab.GetComponent<NPCMovement>();
            tank.target = playerPosition;
            tank.wayPoints = waypoints;
            //tank.agent.destination = waypoints[0].position;
            health = tank.GetComponent<NPCHealth>();
            health.spawner = this;
            tank.currentState = NPCMovement.States.Patrol;
            Instantiate(tankPrefab, transform.position, Quaternion.identity);
        }
    }
}
