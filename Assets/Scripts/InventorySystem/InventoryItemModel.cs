using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryItemModel : MonoBehaviour
    {
        [SerializeField] private System.Guid id = Guid.Empty;
        public Guid ID => id;

        [SerializeField] private InventoryItemData data = null;
        public InventoryItemData Data => data;

        [SerializeField] private InventoryItemState currentState = InventoryItemState.Free;
        public InventoryItemState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }

        private void Awake()
        {
            id = Guid.NewGuid();
        }
        
    }
}
