using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grid_Inventory
{
    [RequireComponent(typeof(InventoryHighlight))]
    public class InventoryController : MonoBehaviour
    {
        private ItemGrid _selectedItemGrid;

        public ItemGrid SelectedItemGrid
        {
            get => _selectedItemGrid;
            set
            {
                _selectedItemGrid = value;
                _inventoryHighlight.SetParent(value);
            }
        }

        private GridInventoryItem _overlappingInventoryItem;
        private GridInventoryItem _selectedInventoryItem;
        private RectTransform _selectedItemImage;

        private InventoryHighlight _inventoryHighlight;
        private GridInventoryItem _itemToHighlight;
        private Vector2Int _previousPosition;


        [Header("For testing purposes")]
        
        [SerializeField] private KeyCode createRandomItemKey = KeyCode.Q;
        [SerializeField] private KeyCode insertRandomItemKey = KeyCode.W;
        [SerializeField] private KeyCode rotateItemKey = KeyCode.R;

        [SerializeField] private List<InventoryItemData> itemsPool;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private RectTransform canvasRectTransform;

        private void Awake()
        {
            _inventoryHighlight = GetComponent<InventoryHighlight>();
        }

        private void Update()
        {
            DragSelectedItemIcon();

            if (SelectedItemGrid is null)
            {
                _inventoryHighlight.Switch(false);
                return;
            }

            if (Input.GetKeyDown(createRandomItemKey))
            {
                if (_selectedInventoryItem is null)
                    CreateRandomItem();
            }

            if (Input.GetKeyDown(insertRandomItemKey))
                InsertRandomItem();
            
            if (Input.GetKeyDown(rotateItemKey))
                RotateSelectedItem();
            
            HandleHighlighting();

            if (Input.GetMouseButtonDown(0))
            {
                var tileGridPosition = GetTileGridPosition();

                if (_selectedInventoryItem is null)
                {
                    PickUpSelectedItem(tileGridPosition);
                }
                else
                {
                    PlaceSelectedItem(tileGridPosition);
                }
            }
        }

        private void RotateSelectedItem()
        {
            if (_selectedInventoryItem is null) return;
            _selectedInventoryItem.Rotate();
        }

        private void InsertRandomItem()
        {
            CreateRandomItem();
            var itemToInsert = _selectedInventoryItem;
            _selectedInventoryItem = null;
            InsertItem(itemToInsert);
        }

        private void InsertItem(GridInventoryItem itemToInsert)
        {
            var pos = _selectedItemGrid.FindSpaceForItem(itemToInsert);

            if (pos is null) return;

            _selectedItemGrid.PlaceItem(itemToInsert, pos.Value);

        }

        private Vector2Int GetTileGridPosition()
        {
            var dragPosition = Input.mousePosition;
            if (_selectedInventoryItem is null) return SelectedItemGrid.GetTileGridPosition(dragPosition);

            dragPosition.x -= (_selectedInventoryItem.Width - 1) * ItemGrid.TileSizeWidth / 2f;
            dragPosition.y += (_selectedInventoryItem.Height - 1) * ItemGrid.TileSizeHeight / 2f;

            return SelectedItemGrid.GetTileGridPosition(dragPosition);
        }

        private void HandleHighlighting()
        {
            var posOnGrid = GetTileGridPosition();
            if (posOnGrid == _previousPosition)
            {
                return;
            }

            if (_selectedInventoryItem is null)
            {
                _itemToHighlight = SelectedItemGrid.GetItem(posOnGrid);
                if (_itemToHighlight is not null)
                {
                    _inventoryHighlight.Switch(true);
                    _inventoryHighlight.SetParent(SelectedItemGrid);
                    _inventoryHighlight.SetHighlighterSize(_itemToHighlight);
                    _inventoryHighlight.SetHighlighterPosition(SelectedItemGrid, _itemToHighlight);
                }
                else
                {
                    _inventoryHighlight.Switch(false);
                }
            }
            else
            {
                _inventoryHighlight.Switch(SelectedItemGrid.BoundaryCheck(posOnGrid.x, posOnGrid.y,
                    _selectedInventoryItem.Width, _selectedInventoryItem.Height));
                _inventoryHighlight.SetParent(SelectedItemGrid);
                _inventoryHighlight.SetHighlighterSize(_selectedInventoryItem);
                _inventoryHighlight.SetHighlighterPosition(SelectedItemGrid, _selectedInventoryItem, posOnGrid.x,
                    posOnGrid.y);
            }
        }

        private void CreateRandomItem()
        {
            var inventoryItem = Instantiate(itemPrefab).GetComponent<GridInventoryItem>();
            _selectedInventoryItem = inventoryItem;
            _selectedItemImage = inventoryItem.GetComponent<RectTransform>();
            _selectedItemImage.SetParent(canvasRectTransform);
            _selectedItemImage.SetAsLastSibling();
            var selectedItemID = Random.Range(0, itemsPool.Count);
            inventoryItem.SetItemData(itemsPool[selectedItemID]);
        }

        private void PlaceSelectedItem(Vector2Int tileGridPosition)
        {
            var isItemPlaced = SelectedItemGrid.PlaceItem(_selectedInventoryItem,
                tileGridPosition, ref _overlappingInventoryItem);
            if (isItemPlaced)
            {
                _selectedInventoryItem = null;
                if (_overlappingInventoryItem is not null)
                {
                    _selectedInventoryItem = _overlappingInventoryItem;
                    _overlappingInventoryItem = null;
                    _selectedItemImage = _selectedInventoryItem.GetComponent<RectTransform>();
                    _selectedItemImage.SetAsLastSibling();
                }
            }
        }

        private void PickUpSelectedItem(Vector2Int tileGridPosition)
        {
            _selectedInventoryItem = SelectedItemGrid.PickUpItem(tileGridPosition);
            if (_selectedInventoryItem is not null)
            {
                _selectedItemImage = _selectedInventoryItem.GetComponent<RectTransform>();
            }
        }

        private void DragSelectedItemIcon()
        {
            if (_selectedInventoryItem is not null)
                _selectedItemImage.position = Input.mousePosition;
        }
    }
}