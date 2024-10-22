using RuntimeInspectorNamespace.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace RuntimeInspectorNamespace
{
    public class HierarchyDataRootSearch : HierarchyDataRoot
    {
        public override string Name { get { return reference.Name; } }
        public override int ChildCount { get { return searchResult.Count; } }
        internal Transform RootTransform { get { return (reference is HierarchyDataRootPseudoScene) ? (reference as HierarchyDataRootPseudoScene).rootTransform : null; } }

        //private readonly List<Transform> searchResult = new List<Transform>();
        private List<Transform> searchResult = new List<Transform>();

        private readonly HierarchyDataRoot reference;

        private string searchTerm;

        public HierarchyDataRootSearch(RuntimeHierarchy hierarchy, HierarchyDataRoot reference) : base(hierarchy)
        {
            this.reference = reference;
        }

        Transform[] cachedTransforms;
        string[] cachedTransformNames;
        bool isCached = false;
        ConcurrentBag<Transform> results = new ConcurrentBag<Transform>();


        public override void RefreshContent()
        {
            if (!Hierarchy.IsInSearchMode) return;

            searchResult.Clear();
            searchTerm = Hierarchy.SearchTerm;

            results.Clear();

            if (isCached == false)
            {
                var target = GameObject.FindObjectOfType<RootObject>();
                if (target == null)
                {
                    Debug.LogError("Missing rootobject..");
                    return;
                }
                cachedTransforms = target.GetComponentsInChildren<Transform>(true);
                cachedTransformNames = new string[cachedTransforms.Length];
                for (int i = 0, length = cachedTransforms.Length; i < length; i++)
                {
                    cachedTransformNames[i] = cachedTransforms[i].name;
                }

                isCached = true;
            }


            SearchFixedList();

            //for (int i = 0, childCount = reference.ChildCount; i < childCount; i++)
            //{
            //    Transform obj = reference.GetChild(i);
            //    //Debug.Log("reference: " + obj.name);
            //    if (obj != null)
            //    {
            //        //SearchTransformRecursively(obj);
            //        SearchFixedList(obj);
            //    }
            //}
        }

        public override bool Refresh()
        {
            m_depth = 0;
            bool result = base.Refresh();

            // Scenes with no matching search results should be hidden in search mode
            if (searchResult.Count == 0)
            {
                m_height = 0;
                m_depth = -1;
            }

            return result;
        }

        public override HierarchyDataTransform FindTransformInVisibleChildren(Transform target, int targetDepth = -1)
        {
            if (m_depth < 0 || targetDepth > 1 || !IsExpanded)
                return null;

            for (int i = children.Count - 1; i >= 0; i--)
            {
                if (ReferenceEquals(children[i].BoundTransform, target))
                    return children[i];
            }

            return null;
        }

        private void SearchTransformRecursively(Transform obj)
        {
            if (RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Contains(obj)) return;

            if (RuntimeInspectorUtils.caseInsensitiveComparer.IndexOf(obj.name, searchTerm, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0) searchResult.Add(obj);

            for (int i = 0, childCount = obj.childCount; i < childCount; i++)
            {
                SearchTransformRecursively(obj.GetChild(i));
            }
        }

        private void SearchFixedList()
        {
            //Parallel.For(0, cachedTransforms.Length, i =>
            //{
            //    if (cachedTransformNames[i].IndexOf(searchTerm, System.StringComparison.OrdinalIgnoreCase) >= 0)
            //    {
            //        results.Add(cachedTransforms[i]);
            //    }
            //});

            //// TODO make results not list?
            //searchResult = results.ToList();

            for (int i = 0, len = cachedTransforms.Length; i < len; i++)
            {
                //Transform obj = cachedTransforms[i];
                // TODO move to initial search
                // if (RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Contains(obj)) continue;
                // TODO use fixed array for results?
                //if (RuntimeInspectorUtils.caseInsensitiveComparer.IndexOf(obj.name, searchTerm, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0) searchResult.Add(obj);
                if (cachedTransformNames[i].IndexOf(searchTerm, System.StringComparison.OrdinalIgnoreCase) > -1) searchResult.Add(cachedTransforms[i]);
                //if (cachedTransformNames[i].IndexOf(searchTerm, System.StringComparison.InvariantCultureIgnoreCase) > -1) searchResult.Add(cachedTransforms[i]);
            }
        }

        public override Transform GetChild(int index)
        {
            return searchResult[index];
        }

        public override Transform GetNearestRootOf(Transform target)
        {
            return searchResult.Contains(target) ? target : null;
        }
    }
}