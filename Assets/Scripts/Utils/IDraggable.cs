using UnityEngine;

namespace Utils
{
    public interface IDraggable
    {
        void OnDragEvent(bool isDragging);
        void SetOriginalPosition(Vector3 originalPosition);
        void PutOnOriginalPosition();
    }
}