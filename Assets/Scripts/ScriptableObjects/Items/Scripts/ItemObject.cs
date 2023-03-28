using UnityEngine;

namespace ScriptableObjects.Items.Scripts
{
    internal enum ItemType
    {
        Food,
        Equipment,
        Default
    }

    internal abstract class ItemObject : ScriptableObject
    {
        internal GameObject Prefab;
        internal ItemType Type;
        [TextArea(15, 20)]
        internal string Description;
    }
}