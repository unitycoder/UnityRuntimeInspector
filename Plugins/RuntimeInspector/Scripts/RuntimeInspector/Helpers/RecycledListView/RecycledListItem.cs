using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace
{
    [RequireComponent(typeof(RectTransform))]
    public class RecycledListItem : MonoBehaviour
    {
        public object Tag { get; set; }
        public int Position { get; set; }

        private IListViewAdapter adapter;

        internal void SetAdapter(IListViewAdapter adapter)
        {
            this.adapter = adapter;
        }

        public void OnClick(PointerEventData eventData)
        {
            adapter.OnItemClicked(this, eventData);
        }
    }
}