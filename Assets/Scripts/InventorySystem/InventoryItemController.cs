using System;
using UnityEngine;
using Utils;

namespace InventorySystem
{
    public class InventoryItemController : MonoBehaviour, IDraggable, IBagThrowable
    {
        [SerializeField] private InventoryItemModel model;
        [SerializeField] private Rigidbody rBody;

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
        }

        public void PutInBag()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromBag()
        {
            throw new NotImplementedException();
        }
    }
}