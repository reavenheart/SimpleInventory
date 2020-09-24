using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using Utils;

namespace InventorySystem
{
    /// <summary>
    /// Inventory Item controller. Responsible with its interation logic and its events
    /// </summary>
    public class InventoryItemController : MonoBehaviour, IDraggable, IBagThrowable
    {
        [SerializeField] private InventoryItemModel model = new InventoryItemModel();
        [SerializeField] private Rigidbody rBody = null;
        private Vector3 originalPosition;

        private void Awake()
        {
            if (model != null)
            {
                model.ID = Guid.NewGuid();
            }
            
            if (rBody == null)
            {
                rBody = GetComponent<Rigidbody>();
            }
        }

        private void Start()
        {
            gameObject.name = model.Data.itemName + "_" + model.ID.ToString();
        }

        public bool CanDrag()
        {
            if (model.CurrentState == InventoryItemState.Free || model.CurrentState == InventoryItemState.Holding)
            {
                return true;
            }
            else
                return false;
        }

        public void OnDragEvent(bool isDragging)
        {
            model.CurrentState = InventoryItemState.Holding;
            rBody.useGravity = !isDragging;
            rBody.isKinematic = isDragging;
            if (!isDragging)
            {
                PutOnOriginalPosition();
            }
        }

        public void SetOriginalPosition(Vector3 originalPosition)
        {
            this.originalPosition = originalPosition;
        }

        public void PutOnOriginalPosition()
        {
            transform.SetParent(null);
            transform.position = originalPosition;
            transform.localScale = Vector3.one;
            model.CurrentState = InventoryItemState.Free;
        }

        public void PutInBag()
        {
            model.CurrentState = InventoryItemState.InBag;
            EventBusManager.Bus.Publish(new OnBagEnterEvent(this));
        }

        public void RemoveFromBag()
        {
            transform.SetParent(null);
            
            transform.DOMove(transform.position + Vector3.up * 0.2f, 0.25f).OnComplete(() =>
            {
                rBody.velocity = (transform.up * 0.25f + transform.forward * 1.25f + transform.right * 1.25f).normalized * 0.5f;
                rBody.useGravity = true;
                rBody.isKinematic = false;
                transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    model.CurrentState = InventoryItemState.Free;
                });
            });
            
            
            EventBusManager.Bus.Publish(new OnBagLeaveEvent(this));
        }

        public string GetID()
        {
            return model.ID.ToString();
        }
    }
}