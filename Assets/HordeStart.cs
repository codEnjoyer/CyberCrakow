using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawner;
public class HordeStart : ActionScript
{
    // Start is called before the first frame update
    public SpawnerScript[] spawners;
    public override void Activate()
    {
        SpawnHorde();
    }
    
    public void SpawnHorde()
    {
        for (int i =0;i< spawners.Length;i++)
            spawners[i].SpawnTank();
    }
}
