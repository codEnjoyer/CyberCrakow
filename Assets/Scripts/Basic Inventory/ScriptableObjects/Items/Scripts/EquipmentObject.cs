using UnityEngine;

namespace ScriptableObjects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
    public class EquipmentObject : ItemObject
    {
        public float attackBonus;
        public float defenseBonus;
        private void Awake()
        {
            type = ItemType.Equipment;
        }
    }
}