using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    /// <summary>
    /// Inventory UI Controller. Responsible for generating cells, showing and hiding UI
    /// </summary>
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private InventoryCellController cellPrefab = null;
        [SerializeField] private RectTransform cellContainer = null;
        [SerializeField] private Canvas canvas = null;
        private List<InventoryCellController> cells = new List<InventoryCellController>();
        
        public void Initialize(int cellNumber)
        {
            for (int i = 0; i < cellNumber; i++)
            {
                var cell = Instantiate(cellPrefab, cellContainer, false);
                cell.transform.localScale = Vector3.one;
                cells.Add(cell);
            }
            cellContainer.gameObject.SetActive(false);
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

        public void ShowUI()
        {
            canvas.enabled = true;
            cellContainer.gameObject.SetActive(true);
        }

        public void HideUI()
        {
            canvas.enabled = false;
            cellContainer.gameObject.SetActive(false);
        }
    }
}