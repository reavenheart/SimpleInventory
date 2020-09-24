using System;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class InventoryCellModel
    {
        public IBagThrowable bagItem;
        public GameObject bagCopy;

        public bool IsFree => bagItem == null && bagCopy == null;
    }
}