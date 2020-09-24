using UnityEngine;

namespace InventorySystem
{
    public class InventoryCellController : MonoBehaviour
    {
        [SerializeField] private InventoryCellModel model = null;
        [SerializeField] private float itemScale = 100;
        public bool IsFree => model.IsFree;

        private void Awake()
        {
            model = new InventoryCellModel();
        }

        public void AddItemToCell(IBagThrowable item, GameObject copy)
        {
            model.bagItem = item;
            model.bagCopy = copy;

            copy.transform.SetParent(transform, false);
            copy.transform.localScale = Vector3.one * itemScale;
        }

        public void ClearCell()
        {
            Destroy(model.bagCopy);
            model.bagItem = null;
            model.bagCopy = null;
        }

        public void ThrowItem()
        {
            if (!IsFree)
            {
                model.bagItem.RemoveFromBag();
            }
        }

        public bool HasItem(IBagThrowable item)
        {
            if (model.bagItem == null)
                return false;
            return model.bagItem.Equals(item);
        }
    }
}