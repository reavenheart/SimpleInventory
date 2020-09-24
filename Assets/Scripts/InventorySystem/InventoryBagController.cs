using System;
using Managers;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryBagController : MonoBehaviour
    {
        [SerializeField][Range(1,5)] private int size = 3;
        [SerializeField] private InventoryUIController uiController;

        private void Awake()
        {
            uiController.Initialize(size);
        }

        private void OnEnable()
        {
            EventBusManager.Bus.Subscribe<OnBagEnterEvent>(OnBagEnterHandler); 
            EventBusManager.Bus.Subscribe<OnBagLeaveEvent>(OnBagLeaveHandler); 
        }
        
        private void OnDisable()
        {
            EventBusManager.Bus.Unsubscribe<OnBagEnterEvent>(OnBagEnterHandler); 
        }

        private void OnBagEnterHandler(OnBagEnterEvent e)
        {
            Debug.Log("OnBagEnterHandler called");
            if (uiController.HasFreeSpace())
            {
                GameObject copy = null;
                CreateBagCopy(e.Item, out copy);
                if (copy != null)
                {
                    uiController.AddItem(e.Item, copy);
                }
            }
            else
            {
                e.Item.RemoveFromBag();
            }
        }

        private void CreateBagCopy(IBagThrowable item, out GameObject copy)
        {
            var itemGO = item as MonoBehaviour;
            if (itemGO != null)
            {
                copy = new GameObject("Copy");
                var mF = copy.AddComponent<MeshFilter>();
                var mR = copy.AddComponent<MeshRenderer>();

                mF.sharedMesh = itemGO.GetComponent<MeshFilter>().sharedMesh;
                mR.material = itemGO.GetComponent<MeshRenderer>().material;

                itemGO.transform.SetParent(transform, true);
                copy.transform.position = itemGO.transform.position;
                copy.transform.rotation = itemGO.transform.rotation;
                copy.transform.localScale = itemGO.transform.localScale / transform.localScale.x;
            }
            else
            {
                Debug.LogError("Item is not a MonoBehavior");
                copy = null;
            }
        }

        private void OnBagLeaveHandler(OnBagLeaveEvent e)
        {
            Debug.Log("OnBagLeaveHandler called");
            uiController.RemoveItem(e.Item);
        }
    }
}