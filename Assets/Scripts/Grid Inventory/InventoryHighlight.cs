using UnityEngine;

namespace Grid_Inventory
{
    public class InventoryHighlight : MonoBehaviour
    {
        [SerializeField] private RectTransform highlighter;

        public void Switch(bool isActive)
        {
            highlighter.gameObject.SetActive(isActive);
        }
        
        public void SetHighlighterSize(GridInventoryItem targetItem)
        {
            var size = new Vector2(
                targetItem.Width * ItemGrid.TileSizeWidth,
                targetItem.Height * ItemGrid.TileSizeHeight);
            highlighter.sizeDelta = size;
        }

        public void SetHighlighterPosition(ItemGrid targetGrid, GridInventoryItem targetItem)
        {
            SetParent(targetGrid);

            var pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.PositionOnGrid);
            highlighter.localPosition = pos;
        }

        public void SetParent(ItemGrid targetGrid)
        {
            if (targetGrid is null)
            {
                return;
            }
            highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
        }

        public void SetHighlighterPosition(ItemGrid targetGrid, GridInventoryItem targetItem, int posX, int posY)
        {
            var pos = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);
            highlighter.localPosition = pos;
        }
    }
}
