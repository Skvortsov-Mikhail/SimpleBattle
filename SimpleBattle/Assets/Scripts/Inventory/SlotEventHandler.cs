using UnityEngine;
using UnityEngine.EventSystems;

public class SlotEventHandler : MonoBehaviour, IDropHandler
{
    private Slot _slot;

    private void Start()
    {
        _slot = GetComponent<Slot>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var otherItemTransform = eventData.pointerDrag.transform;
        var item = otherItemTransform.GetComponent<Item>();

        if (_slot.IsEmpty || _slot.Item.Config.ItemType == item.Config.ItemType)
        {
            if (_slot.TryFillSlot(item, item.ParentSlot.ItemAmount) == true)
            {
                otherItemTransform.SetParent(transform);
                otherItemTransform.localPosition = Vector3.zero;
            }
        }
    }
}