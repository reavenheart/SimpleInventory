using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class InventoryItemModel
    {
        [SerializeField] private System.Guid id = Guid.Empty;
        public Guid ID
        {
            get => id;
            set => id = value;
        }

        [SerializeField] private InventoryItemData data = null;
        public InventoryItemData Data => data;

        [SerializeField] private InventoryItemState currentState = InventoryItemState.Free;
        public InventoryItemState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }
    }
}
