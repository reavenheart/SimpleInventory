using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityTemplateProjects;
using Utils;

[RequireComponent(typeof(SimpleCameraController))]
public class ObjectDragAndDrop : MonoBehaviour
{
    [SerializeField] private Camera renderCamera;
    [SerializeField] private LayerMask draggableLayerMask = 0;
    [SerializeField] private LayerMask inventoryLayerMask = 0;
    [SerializeField][Range(0,5)] private float maxRayDistance = 1;
    private bool isMouseDragging = false;
    private bool isOverBag = false;
    private Vector3 offsetValue;
    private Vector3 positionOfScreen;
    private Transform draggingTransform;
    private Vector3 originalPosition;
    private IDraggable draggingObject;
    private IInventoryUICheckable bagUI;
    private bool isCheckingUI = false;

    [SerializeField] private Transform bag = null;
    
    void Awake()
    {
        if (renderCamera == null)
        {
            renderCamera = GetComponent<Camera>();

            if (renderCamera == null)
            {
                renderCamera = Camera.main;
            }
        }    
    }

    private void Update()
    {
        Ray ray = renderCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            CheckIfCanDrag(ray);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isMouseDragging)
            {
                ReleaseDraggingItem();
            }
            else
            {
                ReleaseBagUI();
            }
        }
        
        if (isMouseDragging && draggingObject != null)
        {
            DragItem(ray);
        }
    }

    private void CheckIfCanDrag(Ray ray)
    {
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, maxRayDistance, draggableLayerMask))
        {
            draggingObject = hit.transform.GetComponent<IDraggable>();
            if (draggingObject != null && draggingObject.CanDrag())
            {
                isMouseDragging = true;
                draggingObject.OnDragEvent(isMouseDragging);
                draggingTransform = hit.transform;
                originalPosition = draggingTransform.position;
                positionOfScreen = renderCamera.WorldToScreenPoint(originalPosition);
                offsetValue = originalPosition - renderCamera.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
            }
        }
        if (Physics.Raycast(ray, out hit, maxRayDistance, inventoryLayerMask) && !isMouseDragging)
        {
            bagUI = hit.transform.GetComponent<IInventoryUICheckable>();
            if (bagUI != null)
            {
                bagUI.ShowUI();
                isCheckingUI = true;
            }
        }
    }

    private void ReleaseBagUI()
    {
        if (isCheckingUI)
        {
            var pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                // Apply Custom Logic Here
                var foo = result.gameObject.GetComponent<InventoryCellController>();
                if (foo != null)
                {
                    foo.ThrowItem();
                }
            }
            if (bagUI != null)
            {
                bagUI.HideUI();
            }
            isCheckingUI = false;
            bagUI = null;
        }
    }

    private void DragItem(Ray ray)
    {
        if (draggingTransform != null)
        {
            RaycastHit hit;
            Vector3 currentScreenSpace;
                
            if (Physics.Raycast(ray, out hit, maxRayDistance, inventoryLayerMask))
            {
                isOverBag = true;
                var bagScreenPosition = renderCamera.WorldToScreenPoint(bag.position);

                currentScreenSpace =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, bagScreenPosition.z);
                draggingTransform.position = Vector3.Lerp(
                    draggingTransform.position,
                    renderCamera.ScreenToWorldPoint(currentScreenSpace) + offsetValue,
                    0.5f);

                draggingTransform.localScale = Vector3.Lerp(
                    draggingTransform.localScale,
                    new Vector3(0.2f, 0.2f, 0.2f),
                    0.5f);
            }
            else
            {
                isOverBag = false;
                currentScreenSpace =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);
                draggingTransform.position = renderCamera.ScreenToWorldPoint(currentScreenSpace) + offsetValue;
                    
                draggingTransform.localScale = Vector3.Lerp(
                    draggingTransform.localScale,
                    Vector3.one,
                    0.5f);
            }
        }
    }

    private void ReleaseDraggingItem()
    {
        isMouseDragging = false;
        draggingObject?.SetOriginalPosition(originalPosition);
        if (isOverBag)
        {
            if (draggingTransform != null)
            {
                var bagThrowable = draggingTransform.GetComponent<IBagThrowable>();
                bagThrowable?.PutInBag();
            }
        }
        else
        {
            draggingObject?.OnDragEvent(isMouseDragging);
        }

        draggingTransform = null;
    }
}
