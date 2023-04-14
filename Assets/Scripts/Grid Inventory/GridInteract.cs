using UnityEngine;
using UnityEngine.EventSystems;

namespace Grid_Inventory
{
    [RequireComponent(typeof(ItemGrid))]
    public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private InventoryController inventoryController;
        private ItemGrid _itemGrid;

        private void Awake()
        {
            _itemGrid = GetComponent<ItemGrid>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            inventoryController.SelectedItemGrid = _itemGrid;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryController.SelectedItemGrid = null;
        }
    }
}