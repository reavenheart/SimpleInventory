using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace InventorySystem
{
    [System.Serializable]
    public class BagItemEvent : UnityEvent<string>
    {
    }
    
    /// <summary>
    /// A bag controller, which is responsible for working with items and updating UI
    /// </summary>
    public class InventoryBagController : MonoBehaviour, IInventoryUICheckable
    {
        [SerializeField][Range(1,5)] private int size = 3;
        [SerializeField] private InventoryUIController uiController = null;
        [SerializeField][Range(0.0f,1.0f)] private float maxOffset = 0.15f;
        [SerializeField] private BagItemEvent OnObjectAdded = null;
        [SerializeField] private BagItemEvent OnObjectRemoved = null;
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
            if (uiController.HasFreeSpace())
            {
                GameObject copy = null;
                CreateBagCopy(e.Item, out copy);
                if (copy != null)
                {
                    uiController.AddItem(e.Item, copy);
                    PlaceInTheBag(e.Item);
                }
            }
            else
            {
                e.Item.RemoveFromBag();
            }
        }

        private void PlaceInTheBag(IBagThrowable item)
        {
            var itemGO = item as MonoBehaviour;
            if (itemGO != null)
            {
                Vector3 randOffset = new Vector3(Random.Range(-maxOffset, maxOffset),Random.Range(-maxOffset, maxOffset),Random.Range(-maxOffset, maxOffset));
                itemGO.transform.DOLocalMove(randOffset, 0.5f).SetEase(Ease.InOutCubic);
                itemGO.transform.DORotate(transform.rotation.eulerAngles, 0.5f).SetEase(Ease.InOutCubic);
                itemGO.transform.DOScale(Vector3.one * 0.25f, 0.5f).SetEase(Ease.InOutCubic);
                OnObjectAdded?.Invoke(item.GetID());
            }
        }

        /// <summary>
        /// Create a copy of the original item for UI
        /// </summary>
        /// <param name="item"></param>
        /// <param name="copy"></param>
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
            uiController.RemoveItem(e.Item);
            OnObjectRemoved?.Invoke(e.Item.GetID());
        }

        public void ShowUI()
        {
            uiController.ShowUI();
        }

        public void HideUI()
        {
            uiController.HideUI();
        }
    }
}