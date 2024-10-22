using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeInspectorNamespace.Extensions
{
    public class ObjectViewer : MonoBehaviour
    {
        public GameObject rawImageUI; // The RawImage UI Panel to set active/inactive
        public Transform targetObject; // The object to clone
        public Camera renderCamera; // Camera that renders to RawImage
        public string renderLayerName = "ObjectView"; // Layer name for the cloned object to be visible to the renderCamera

        private GameObject clonedObject; // Holds the cloned object

        public CameraOrbit cameraOrbit;

        void Start()
        {
            // Optional: Hide the UI initially
            //rawImageUI.SetActive(false);
            //OpenView();
        }

        // Call this to open the UI view and clone the object
        public void OpenView()
        {
            CloseView();

            // Enable the UI view
            rawImageUI.SetActive(true);

            // Clone the target object if it's not already cloned
            if (clonedObject == null)
            {
                // Create a dummy parent GameObject to act as the center of rotation
                GameObject dummyParent = new GameObject("RotationPivot");

                // Clone the target object as a child of the dummy parent
                clonedObject = Instantiate(targetObject.gameObject, dummyParent.transform);
                clonedObject.transform.localPosition = Vector3.zero; // Reset the position of the cloned object

                // Center the cloned object within the dummy parent based on its bounds
                CenterObjectOnBounds(clonedObject);

                // Set the layer for the cloned object and its children
                int renderLayer = LayerMask.NameToLayer(renderLayerName);
                if (renderLayer != -1)
                {
                    SetLayerRecursively(clonedObject, renderLayer); // Apply the correct layer to the cloned object and its children
                }
                else
                {
                    Debug.LogError("Layer " + renderLayerName + " does not exist.");
                }
            }
            else
            {
                clonedObject.SetActive(true); // In case it's already cloned, just re-enable it
            }

            // Adjust the camera to fit the object in view
            AdjustCameraToFitObject(clonedObject);

            // Set the targetObject of the camera orbit to the dummy parent for correct rotation
            cameraOrbit.targetObject = clonedObject.transform.parent;
        }

        // Method to center the cloned object within its dummy parent based on its bounds
        private void CenterObjectOnBounds(GameObject obj)
        {
            // Calculate the object's bounds
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            Bounds bounds = new Bounds(renderers[0].bounds.center, Vector3.zero);

            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds); // Encapsulate all child renderers into the bounds
            }

            // Offset the object's position so that its bounds' center is at (0,0,0) in the parent (dummy)
            obj.transform.localPosition = -bounds.center;
        }

        // Method to adjust the camera to fit the object within view
        private void AdjustCameraToFitObject(GameObject obj)
        {
            // Calculate the bounds of the object
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            Bounds bounds = new Bounds(renderers[0].bounds.center, Vector3.zero);

            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            // Calculate the distance needed to fit the object in the camera's view
            float objectSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) * 2f;
            float distance = objectSize / (2.0f * Mathf.Tan(Mathf.Deg2Rad * renderCamera.fieldOfView * 0.5f));

            // Position the camera at a distance where the object fits in view
            renderCamera.transform.position = bounds.center - renderCamera.transform.forward * distance;

            // Make sure the camera is looking at the center of the object's bounds
            renderCamera.transform.LookAt(bounds.center);
        }


        // Call this to close the UI view and disable the cloned object
        public void CloseView()
        {
            // Disable the cloned object
            if (clonedObject != null)
            {
                Destroy(clonedObject.transform.parent.gameObject);
                clonedObject = null;
            }

            // Disable the UI view
            rawImageUI.SetActive(false);
        }

        // Recursively sets the layer of the object and all its children
        private void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        internal void Show(Transform item)
        {
            targetObject = item;
            OpenView();
        }
    }

}