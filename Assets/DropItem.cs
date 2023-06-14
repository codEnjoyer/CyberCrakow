using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid_Inventory
{
    public class DropItem : MonoBehaviour
    {
        public ItemGrid inventory;
        PickUpItem item;
        public void SpawnItem(GameObject prefab)
        {
            item = prefab.GetComponent<PickUpItem>();
            item.grid = inventory;
            Instantiate(prefab, transform.position, Quaternion.identity);        
        }
    }
}
