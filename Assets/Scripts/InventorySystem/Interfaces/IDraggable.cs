using UnityEngine;

namespace InventorySystem
{
    public interface IDraggable
    {
        bool CanDrag();
        void OnDragEvent(bool isDragging);
        void SetOriginalPosition(Vector3 originalPosition);
        void HightlightAvailableForDrag();
    }
}