using System;
using Managers;
using UnityEngine;
using Utils;

namespace InventorySystem
{
    public class InventoryItemController : MonoBehaviour, IDraggable, IBagThrowable
    {
        [SerializeField] private InventoryItemModel model;
        [SerializeField] private Rigidbody rBody;
        private Vector3 originalPosition;

        private void Awake()
        {
            if (model == null)
            {
                model = GetComponent<InventoryItemModel>();
            }

            if (rBody == null)
            {
                rBody = GetComponent<Rigidbody>();
            }
        }

        private void Start()
        {
            gameObject.name = model.Data.name + "_" + model.ID.ToString();
        }

        public void OnDragEvent(bool isDragging)
        {
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
        }

        public void PutInBag()
        {
            EventBusManager.Bus.Publish(new OnBagEnterEvent(this));
        }

        public void RemoveFromBag()
        {
            //PutOnOriginalPosition();
            OnDragEvent(false);
            EventBusManager.Bus.Publish(new OnBagLeaveEvent(this));
        }
    }
}