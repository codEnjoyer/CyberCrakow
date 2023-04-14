using UnityEngine;

namespace Grid_Inventory
{
    public class ItemGrid : MonoBehaviour
    {
        [SerializeField] private int tilesWidthCount = 20;
        [SerializeField] private int tilesHeightCount = 10;

        public const int TileSizeWidth = 32;
        public const int TileSizeHeight = 32;

        private GridInventoryItem[,] _inventoryItemSlot;

        private RectTransform _rectTransform;

        private Vector2 _positionOnGrid;
        private Vector2Int _tileGridPosition;

        // [SerializeField] private GameObject inventoryItemPrefab;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            InitializeGrid(tilesWidthCount, tilesHeightCount);

            // var invItem = Instantiate(inventoryItemPrefab).GetComponent<GridInventoryItem>();
            // PlaceItem(invItem, 3, 3);
        }

        private void InitializeGrid(int widthCount, int heightCount)
        {
            _inventoryItemSlot = new GridInventoryItem[widthCount, heightCount];
            var gridSize = new Vector2(widthCount * TileSizeWidth, heightCount * TileSizeHeight);
            _rectTransform.sizeDelta = gridSize;
        }

        public Vector2Int GetTileGridPosition(Vector2 gridMousePosition)
        {
            var gridPosition = _rectTransform.position;
            _positionOnGrid.x = gridMousePosition.x - gridPosition.x;
            _positionOnGrid.y = gridPosition.y - gridMousePosition.y;

            _tileGridPosition = new Vector2Int(
                (int) _positionOnGrid.x / TileSizeWidth,
                (int) _positionOnGrid.y / TileSizeHeight);

            return _tileGridPosition;
        }

        public bool PlaceItem(GridInventoryItem inventoryItem, int posX, int posY,
            ref GridInventoryItem overlappingItem)
        {
            if (!BoundaryCheck(posX, posY, inventoryItem.Width, inventoryItem.Height))
                return false;

            if (!OverlappingCheck(posX, posY, inventoryItem.Width, inventoryItem.Height,
                    ref overlappingItem))
            {
                overlappingItem = null;
                return false;
            }
            
            if (overlappingItem is not null)
                VacateItemTiles(overlappingItem);

            PlaceItem(inventoryItem, posX, posY);

            return true;
        }

        public void PlaceItem(GridInventoryItem inventoryItem, int posX, int posY)
        {
            var inventoryRectTransform = inventoryItem.GetComponent<RectTransform>();
            inventoryRectTransform.SetParent(_rectTransform);
            FillItemTiles(inventoryItem, posX, posY);
            inventoryItem.PositionOnGrid = new Vector2Int(posX, posY);

            var iconPosition = CalculatePositionOnGrid(inventoryItem, posX, posY);

            inventoryRectTransform.localPosition = iconPosition;
        }

        public void PlaceItem(GridInventoryItem inventoryItem, Vector2Int pos) =>
            PlaceItem(inventoryItem, pos.x, pos.y);
        
        public Vector2 CalculatePositionOnGrid(GridInventoryItem inventoryItem, int posX, int posY)
        {
            return new Vector2(
                posX * TileSizeWidth + TileSizeWidth * inventoryItem.Width / 2f,
                -(posY * TileSizeHeight + TileSizeHeight * inventoryItem.Height / 2f));
        }

        public Vector2 CalculatePositionOnGrid(GridInventoryItem inventoryItem, Vector2Int pos)
        {
            return CalculatePositionOnGrid(inventoryItem, pos.x, pos.y);
        }
        private bool OverlappingCheck(int posX, int posY, int width, int height, ref GridInventoryItem overlappingItem)
        {
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                if (_inventoryItemSlot[posX + x, posY + y] is not null)
                    overlappingItem = _inventoryItemSlot[posX + x, posY + y];
                else
                {
                    if (overlappingItem != _inventoryItemSlot[posX + x, posY + y])
                        return false;
                }
            }

            return true;
        }

        public bool PlaceItem(GridInventoryItem inventoryItem, Vector2Int pos, ref GridInventoryItem overlappingItem) =>
            PlaceItem(inventoryItem, pos.x, pos.y, ref overlappingItem);

        private void FillItemTiles(GridInventoryItem inventoryItem, int posX, int posY)
        {
            for (var x = 0; x < inventoryItem.Width; x++)
            for (var y = 0; y < inventoryItem.Height; y++)
            {
                _inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }


        public GridInventoryItem PickUpItem(int posX, int posY)
        {
            var pickedItem = _inventoryItemSlot[posX, posY];
            if (pickedItem is null)
                return null;
            VacateItemTiles(pickedItem);
            _inventoryItemSlot[posX, posY] = null;
            return pickedItem;
        }

        public GridInventoryItem PickUpItem(Vector2Int pos) => PickUpItem(pos.x, pos.y);

        private void VacateItemTiles(GridInventoryItem inventoryItem)
        {
            for (var x = 0; x < inventoryItem.Width; x++)
            for (var y = 0; y < inventoryItem.Height; y++)
            {
                _inventoryItemSlot[inventoryItem.PositionOnGrid.x + x, inventoryItem.PositionOnGrid.y + y] = null;
            }
        }

        private bool IsCorrectPosition(int posX, int posY)
        {
            var isInsideGrid = 0 <= posX && posX < tilesWidthCount &&
                               0 <= posY && posY < tilesHeightCount;
            // Debug.Log($"Current pos: {posX}, {posY}");
            return isInsideGrid;
        }

        public bool BoundaryCheck(int posX, int posY, int width, int height)
        {
            var res = IsCorrectPosition(posX, posY) &&
                      IsCorrectPosition(posX + width - 1, posY + height - 1);
            // Debug.Log($"Current pos: {posX}, {posY}. Is correct to place: {res}");

            return res;
        }

        public GridInventoryItem GetItem(int posX, int posY)
        {
            return _inventoryItemSlot[posX, posY];
        }

        public GridInventoryItem GetItem(Vector2Int pos)
        {
            return GetItem(pos.x, pos.y);
        }

        public Vector2Int? FindSpaceForItem(GridInventoryItem itemToInsert)
        {
            var maxPossibleX = tilesWidthCount - itemToInsert.Width + 1;
            var maxPossibleY = tilesHeightCount - itemToInsert.Height + 1;
            for(var x = 0; x < maxPossibleX; x++)
            for (var y = 0; y < maxPossibleY; y++)
            {
                if (IsItemFits(itemToInsert, x, y))
                    return new Vector2Int(x, y);
            }

            return null;
        }

        private bool IsItemFits(GridInventoryItem inventoryItem, int posX, int posY)
        {
            for (var x = 0; x < inventoryItem.Width; x++)
            for (var y = 0; y < inventoryItem.Height; y++)
            {
                if (_inventoryItemSlot[posX + x, posY + y] is not null)
                    return false;
            }

            return true;
        }
    }
}