using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeInspectorNamespace.Extensions
{
    public class ContextMenuSystem : MonoBehaviour
    {
        [SerializeField]
        private PropertiesSystem propertiesSystem;

        RectTransform contextMenuTransform;

        Transform target;

        void Awake()
        {
            contextMenuTransform = transform as RectTransform;
            propertiesSystem.Hide();
            Hide();
        }

        public void Show(Vector2 position, Transform item)
        {
            // TODO handle null item, for scene or other items
            //Debug.Log("context menu for item: " + item.name+" pos: "+ position);
            contextMenuTransform.gameObject.SetActive(true);
            contextMenuTransform.position = position;
            target = item;
        }

        public void Hide()
        {
            contextMenuTransform.gameObject.SetActive(false);
            target = null;
        }

        public void OpenProperties()
        {
            if (target != null)
            {
                propertiesSystem.Show(target);
            }
            Hide();
        }
    }

}