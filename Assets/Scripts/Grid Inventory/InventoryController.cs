using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using Grid_Inventory;
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

        [SerializeField] private List<InventoryItemData> itemsPool;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private RectTransform canvasRectTransform;
        public PlayerInput input;
        public OpenInventory opener;
        public DropItem dropObject;

        private void Start()
        {
            input = new PlayerInput();
            //Debug.Log(opener.isOpen);
            input.Inventory.CreateRandomItem.performed += CreateRandomItem_performed;
            input.Inventory.InsertRandomItem.performed += InsertRandomItem_performed;
            input.Inventory.Rotate.performed += Rotate_performed;
            input.Inventory.Click.performed += Click_performed;
            input.Inventory.Drop.performed += Drop_performed;
            //input.Enable();
        }

        private void Drop_performed(InputAction.CallbackContext obj)
        {
            //Debug.Log("Drop");
            var tileGridPosition = GetTileGridPosition();
            if (_selectedInventoryItem is null)
            {
                PickUpSelectedItem(tileGridPosition);
            }
            if (_selectedInventoryItem != null)
            {
                dropObject.SpawnItem(_selectedInventoryItem.Data.Model);
                _selectedItemImage = null;
                Destroy(_selectedInventoryItem.gameObject);
                _selectedInventoryItem = null;
            }
        }

        private void Click_performed(InputAction.CallbackContext obj)
        {
            if (_selectedItemGrid != null)
            {
                var tileGridPosition = GetTileGridPosition();
                //Debug.Log(tileGridPosition);

                if (_selectedInventoryItem is null)
                {
                    PickUpSelectedItem(tileGridPosition);
                }
                else
                {
                    PlaceSelectedItem(tileGridPosition);
                }
            }
            else
            {
                if (_selectedInventoryItem is null)
                    return;
                else
                {
                    dropObject.SpawnItem(_selectedInventoryItem.Data.Model);
                    _selectedItemImage = null;
                    Destroy(_selectedInventoryItem.gameObject);
                    _selectedInventoryItem = null;
                }
            }
        }

        private void Rotate_performed(InputAction.CallbackContext obj)
        {
            RotateSelectedItem();
        }

        private void InsertRandomItem_performed(InputAction.CallbackContext obj)
        {
            InsertRandomItem();
        }

        private void CreateRandomItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_selectedInventoryItem is null)
                CreateRandomItem();
        }


        private void Awake()
        {
            _inventoryHighlight = GetComponent<InventoryHighlight>();
        }

        private void Update()
        {
            DragSelectedItemIcon();
            //Debug.Log(SelectedItemGrid);
            if (SelectedItemGrid is null)
            {
                _inventoryHighlight.Switch(false);
                input.Disable();
                if (opener.isOpen)
                    input.Inventory.Click.Enable();
                return;
            }     
            else 
            {
                if(opener.isOpen)
                    input.Enable();
            }

            if (opener.isOpen)
                HandleHighlighting();


        }

        private void RotateSelectedItem()
        {
            if (_selectedInventoryItem is null) return;
            _selectedInventoryItem.Rotate();
        }

        public void InsertRandomItem()
        {
            CreateRandomItem();
            var itemToInsert = _selectedInventoryItem;
            _selectedInventoryItem = null;
            InsertItem(itemToInsert);
        }

        public void InsertItem(GridInventoryItem itemToInsert)
        {
            var pos = _selectedItemGrid.FindSpaceForItem(itemToInsert);
            //Debug.Log(pos);
            if (pos != null)
                _selectedItemGrid.PlaceItem(itemToInsert, pos.Value);
        }

        private Vector2Int GetTileGridPosition()
        {
            var dragPosition = Input.mousePosition;
            //Debug.Log(SelectedItemGrid);
            if (_selectedInventoryItem is null) return SelectedItemGrid.GetTileGridPosition(dragPosition);

            dragPosition.x -= (_selectedInventoryItem.Width - 1) * ItemGrid.TileSizeWidth / 2f;
            dragPosition.y += (_selectedInventoryItem.Height - 1) * ItemGrid.TileSizeHeight / 2f;

            return SelectedItemGrid.GetTileGridPosition(dragPosition);
        }

        private void HandleHighlighting()
        {
            var posOnGrid = GetTileGridPosition();
            if (posOnGrid.x < 0)
                posOnGrid.x = 0;
            if (posOnGrid.y < 0)
                posOnGrid.y = 0;
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

        public void CreateItem(InventoryItemData data, ItemGrid grid)
        {
            var inventoryItem = Instantiate(itemPrefab).GetComponent<GridInventoryItem>();
            _selectedItemImage = inventoryItem.GetComponent<RectTransform>();
            _selectedItemImage.SetParent(grid._rectTransform);
            inventoryItem.SetItemData(data);
            inventoryItem.gameObject.transform.position = grid.transform.position;
            _selectedItemGrid = grid;
            InsertItem(inventoryItem);
            _selectedItemGrid = null;
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
        public void InsertCreatedRandomItem(ItemGrid grid)
        {
            Debug.Log("InsertRandom");
            var inventoryItem = Instantiate(itemPrefab).GetComponent<GridInventoryItem>();
            _selectedItemImage = inventoryItem.GetComponent<RectTransform>();
            _selectedItemImage.SetParent(grid._rectTransform);
            var selectedItemID = Random.Range(0, itemsPool.Count);
            inventoryItem.SetItemData(itemsPool[selectedItemID]);
            inventoryItem.gameObject.transform.position = grid.transform.position;
            _selectedItemGrid = grid;
            InsertItem(inventoryItem);
            _selectedItemGrid = null;
        }
        private void PlaceSelectedItem(Vector2Int tileGridPosition)
        {
            //Debug.Log(_selectedInventoryItem);
            //Debug.Log(tileGridPosition);
            //Debug.Log(_overlappingInventoryItem);
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