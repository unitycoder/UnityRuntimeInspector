using RuntimeInspectorNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSystem : MonoBehaviour
{
    [Header("Scene References")]
    public RuntimeHierarchy runtimeHierarchy;

    [Header("Settings")]
    public LayerMask selectionLayerMask;

    void Start()
    {
        
    }

    void Update()
    {
        //if hit UI
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectionLayerMask))
            {
                //Debug.Log("Hit: " + hit.transform.name);
               runtimeHierarchy.Select(hit.transform, RuntimeHierarchy.SelectOptions.FocusOnSelection | RuntimeHierarchy.SelectOptions.ForceRevealSelection);
            }
        }
    }
}
