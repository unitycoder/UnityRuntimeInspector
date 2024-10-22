using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace RuntimeInspectorNamespace.Extensions
{
    public class CameraOrbit : MonoBehaviour, IDragHandler, IScrollHandler
    {
        public Camera renderCamera; // Camera that renders to RawImage
        public Transform targetObject; // Object that the camera will orbit around
        public RawImage rawImageUI; // The RawImage UI element for detecting interactions

        public float rotationSpeed = 360f; // Full 360 degrees rotation for full drag
        public float zoomSpeed = 10f; // Speed for zooming the camera
        public float minZoom = 5f; // Minimum distance to zoom in
        public float maxZoom = 20f; // Maximum distance to zoom out

        private float currentZoom;

        private void Start()
        {
            // Initialize the zoom distance based on the initial distance from the object
            currentZoom = Vector3.Distance(renderCamera.transform.position, targetObject.position);
        }

        // Handle drag input for rotating the camera
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localCursor;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImageUI.rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
            {
                // Normalize localCursor to get 0-1 range in both X and Y directions
                float normalizedX = Mathf.InverseLerp(-rawImageUI.rectTransform.rect.width / 2, rawImageUI.rectTransform.rect.width / 2, localCursor.x);
                float normalizedY = Mathf.InverseLerp(-rawImageUI.rectTransform.rect.height / 2, rawImageUI.rectTransform.rect.height / 2, localCursor.y);

                // Calculate the rotation based on the normalized coordinates
                float rotationX = (normalizedX - 0.5f) * rotationSpeed; // Horizontal rotation (0-1 normalized)
                float rotationY = (normalizedY - 0.5f) * rotationSpeed; // Vertical rotation (0-1 normalized)

                // Rotate the object instantly based on normalized input
                targetObject.localRotation = Quaternion.Euler(rotationY, -rotationX, 0f);
            }
        }


        // Handle zooming with scroll wheel
        public void OnScroll(PointerEventData eventData)
        {
            float scrollDelta = eventData.scrollDelta.y;

            // Adjust the zoom distance
            currentZoom -= scrollDelta * zoomSpeed * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            // Move the camera closer or further from the target based on zoom level
            renderCamera.transform.position = targetObject.position - renderCamera.transform.forward * currentZoom;
        }
    }
}