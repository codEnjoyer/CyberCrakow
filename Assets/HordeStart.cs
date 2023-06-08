using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawner;
public class HordeStart : MonoBehaviour
{
    // Start is called before the first frame update
    public SpawnerScript[] spawners;
    public void Activate()
    {
        SpawnHorde();
    }
    
    public void SpawnHorde()
    {
        for (int i =0;i< spawners.Length;i++)
            spawners[i].SpawnTank();
    }
}
