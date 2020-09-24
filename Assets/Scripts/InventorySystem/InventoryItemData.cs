using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
    public class InventoryItemData : ScriptableObject
    {
        public string name;
        public float mass;
        public InventoryItemType type;
    }
}
