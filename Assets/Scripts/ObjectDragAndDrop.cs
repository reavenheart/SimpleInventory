using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityTemplateProjects;
using Utils;

[RequireComponent(typeof(SimpleCameraController))]
public class ObjectDragAndDrop : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask draggableLayerMask;
    [SerializeField] private LayerMask inventoryLayerMask;
    [SerializeField][Range(0,5)] private float maxRayDistance;
    private bool isMouseDragging = false;
    private bool isOverBag = false;
    private Vector3 offsetValue;
    private Vector3 positionOfScreen;
    private Transform draggingTransform;
    private Vector3 originalPosition;
    private IDraggable draggingObject;

    [SerializeField] private Transform bag;
    
    void Awake()
    {
        if (camera == null)
        {
            camera = GetComponent<Camera>();

            if (camera == null)
            {
                camera = Camera.main;
            }
        }    
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, maxRayDistance, draggableLayerMask))
            {
                Debug.Log(hit.transform.gameObject.name);
                
                draggingObject = hit.transform.GetComponent<IDraggable>();
                if (draggingObject != null)
                {
                    isMouseDragging = true;
                    draggingObject.OnDragEvent(isMouseDragging);
                    draggingTransform = hit.transform;
                    originalPosition = draggingTransform.position;
                    positionOfScreen = camera.WorldToScreenPoint(originalPosition);
                    offsetValue = originalPosition - camera.ScreenToWorldPoint(
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
            draggingTransform.position = originalPosition;
            if (isOverBag)
            {
                var bagThrowable = draggingTransform.GetComponent<IBagThrowable>();
                bagThrowable?.PutInBag();
            }
            else
            {
                draggingObject?.OnDragEvent(isMouseDragging);
            }
            draggingTransform = null;
        }

        if (isMouseDragging && draggingObject != null)
        {
            if (draggingTransform != null)
            {
                Vector3 currentScreenSpace;
                
                if (Physics.Raycast(ray, out hit, maxRayDistance, inventoryLayerMask))
                {
                    isOverBag = true;
                    Debug.Log(hit.transform.gameObject.name);
                    var bagScreenPosition = camera.WorldToScreenPoint(bag.position);

                    currentScreenSpace =
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, bagScreenPosition.z);
                    draggingTransform.position = Vector3.Lerp(
                        draggingTransform.position,
                        camera.ScreenToWorldPoint(currentScreenSpace) + offsetValue,
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
                    draggingTransform.position = camera.ScreenToWorldPoint(currentScreenSpace) + offsetValue;
                    
                    draggingTransform.localScale = Vector3.Lerp(
                        draggingTransform.localScale,
                        Vector3.one,
                        0.5f);
                }
            }
        }
    }
}
