using UnityEngine;

namespace Grid_Inventory
{
    [CreateAssetMenu(fileName = "New ItemData", menuName = "Grid Inventory/Items/ItemData")]
    public class InventoryItemData : ScriptableObject
    {
        [field: SerializeField] public int TilesWidthCount { get; set; } = 1;
        [field: SerializeField] public int TilesHeightCount { get; set; } = 1;
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public GameObject Model { get; set; }
    }
}