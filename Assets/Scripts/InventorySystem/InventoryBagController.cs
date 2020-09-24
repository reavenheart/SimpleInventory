using DG.Tweening;
using Managers;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryBagController : MonoBehaviour, IInventoryUICheckable
    {
        [SerializeField][Range(1,5)] private int size = 3;
        [SerializeField] private InventoryUIController uiController = null;
        [SerializeField][Range(0.0f,1.0f)] private float maxOffset = 0.15f;
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
                itemGO.transform.DOScale(Vector3.one * 0.25f, 0.5f).SetEase(Ease.InOutCubic);
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