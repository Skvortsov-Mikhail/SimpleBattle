using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEventHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Item _item;
    private RectTransform _rectTransform;
    private Canvas _mainCanvas;
    private CanvasGroup _canvasGroup;
    private bool _isDragging;

    private void Start()
    {
        _item = GetComponent<Item>();
        _rectTransform = GetComponent<RectTransform>();
        _mainCanvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isDragging == true) return;

        _item.ShowPopupWindow();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var slotTransform = _rectTransform.parent;
        slotTransform.SetAsLastSibling();
        _canvasGroup.blocksRaycasts = false;

        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        _canvasGroup.blocksRaycasts = true;

        _isDragging = false;
    }
}