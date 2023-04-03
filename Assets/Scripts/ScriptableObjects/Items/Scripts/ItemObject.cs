using UnityEngine;

namespace ScriptableObjects.Items.Scripts
{
    public enum ItemType
    {
        Default,
        Food,
        Equipment,
    }

    public abstract class ItemObject : ScriptableObject
    {
        public GameObject prefab;
        public ItemType type;

        [TextArea(15, 20)]
        public string description;
    }
}