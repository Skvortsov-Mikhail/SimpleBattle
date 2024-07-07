using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Item : MonoBehaviour
{
    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_AmountText;

    private Item _item;
    private ItemConfiguration _configuration;

    private void Start()
    {
        _item = GetComponentInParent<Item>();
        _configuration = _item.Config;

        _item.ParentSlotChanged += RefreshItemUI;

        RefreshItemUI();
    }

    private void OnDestroy()
    {
        _item.ParentSlotChanged -= RefreshItemUI;
    }

    private void RefreshItemUI()
    {
        m_Icon.sprite = _configuration.ItemIcon;

        if (_configuration.MaxAmountInSlot == 1)
        {
            m_AmountText.text = string.Empty;
        }
        else
        {
            m_AmountText.text = _item.ParentSlot.ItemAmount.ToString();
        }
    }
}