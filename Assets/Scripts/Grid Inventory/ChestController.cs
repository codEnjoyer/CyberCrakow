using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid_Inventory {
    public class ChestController : MonoBehaviour
    {
        public ItemGrid chestGrid;
        public InventoryController inventory;
        public OpenInventory opener;
        [SerializeField] public ItemGrid[] chests;
    }
}
