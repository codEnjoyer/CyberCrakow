using UnityEngine;

namespace ScriptableObjects.Items.Scripts
{
    [CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
    internal class DefaultObject : ItemObject
    {
        private void Awake()
        {
            Type = ItemType.Default;
        }
    }
}