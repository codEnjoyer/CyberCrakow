using UnityEngine;
using UnityEngine.UI;

namespace Grid_Inventory
{
    public class GridInventoryItem : MonoBehaviour
    {
        public InventoryItemData Data { get; private set; }
        public Vector2Int PositionOnGrid { get; set; }

        public int Height => IsRotated ? Data.TilesWidthCount : Data.TilesHeightCount;
        public int Width => IsRotated ? Data.TilesHeightCount : Data.TilesWidthCount;

        public bool IsRotated { get; private set; }

        public void SetItemData(InventoryItemData inventoryItemData)
        {
            Data = inventoryItemData;
            GetComponent<Image>().sprite = inventoryItemData.Icon;
            var iconSize = new Vector2(
                Data.TilesWidthCount * ItemGrid.TileSizeWidth,
                Data.TilesHeightCount * ItemGrid.TileSizeHeight);
            GetComponent<RectTransform>().sizeDelta = iconSize;
        }

        public void Rotate()
        {
            IsRotated = !IsRotated;

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.rotation = Quaternion.Euler(0, 0, IsRotated ? 90f : 0f);
        }
    }
}