using System;
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
    }
}