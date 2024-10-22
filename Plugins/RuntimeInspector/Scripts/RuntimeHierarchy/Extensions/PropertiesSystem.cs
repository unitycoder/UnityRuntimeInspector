using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RuntimeInspectorNamespace.Extensions
{
    public class PropertiesSystem : MonoBehaviour
    {
        public GameObject propertiesRoot;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI propsText;
        public ObjectViewer objectViewer;

        public void Show(Transform item)
        {
            //Debug.Log("show properties for item " + item.name);
            titleText.text = item.name;
            propsText.text = item.position.ToString(); // testing only, take metadata later
            propertiesRoot.SetActive(true);
            objectViewer.Show(item);
        }
        public void Hide()
        {
            propertiesRoot.SetActive(false);
            objectViewer.CloseView();
        }
    }
}