using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace
{
	public interface IListViewAdapter
	{
		int Count { get; }
		float ItemHeight { get; }

		RecycledListItem CreateItem( Transform parent );

		void SetItemContent( RecycledListItem item );
		void OnItemClicked( RecycledListItem item, PointerEventData eventData);
	}
}