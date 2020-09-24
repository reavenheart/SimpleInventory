using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private InventoryCellController cellPrefab;
        [SerializeField] private RectTransform cellContainer;
        private List<InventoryCellController> cells = new List<InventoryCellController>();
        
        public void Initialize(int cellNumber)
        {
            for (int i = 0; i < cellNumber; i++)
            {
                var cell = Instantiate(cellPrefab, cellContainer, false);
                cell.transform.localScale = Vector3.one;
                cells.Add(cell);
            }
        }

        public bool HasFreeSpace()
        {
            return cells.Exists(x => x.IsFree == true);
        }

        public void AddItem(IBagThrowable item, GameObject copy)
        {
            var cell = cells.FirstOrDefault(x => x.IsFree);
            if (cell != null)
            {
                cell.AddItemToCell(item, copy);
            }
        }

        public void RemoveItem(IBagThrowable item)
        {
            var cell = cells.FirstOrDefault(x => x.HasItem(item));
            if (cell != null)
            {
                cell.ClearCell();
            }
        }
    }
}