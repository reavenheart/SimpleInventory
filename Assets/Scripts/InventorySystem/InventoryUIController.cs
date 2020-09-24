using UnityEngine;

namespace InventorySystem
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform cellPrefab;
        [SerializeField] private RectTransform cellContainer;

        public void Initialize(int cellNumber)
        {
            for (int i = 0; i < cellNumber; i++)
            {
                var cell = Instantiate(cellPrefab, cellContainer, false);
                cell.localScale = Vector3.one;
            }
        } 
    }
}