using UnityEngine;

namespace Utils
{
    public interface IDraggable
    {
        bool CanDrag();
        void OnDragEvent(bool isDragging);
        void SetOriginalPosition(Vector3 originalPosition);
        void PutOnOriginalPosition();
    }
}