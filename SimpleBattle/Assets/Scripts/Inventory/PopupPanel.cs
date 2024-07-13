using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class PopupPanel : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private TMP_Text m_Title;
    [SerializeField] private Image m_ItemIcon;
    [SerializeField] private TMP_Text m_AmountText;

    [Header("Property")]
    [SerializeField] private Image m_PropertyIcon;
    [SerializeField] private TMP_Text m_PropertyValueText;
    [SerializeField] private TMP_Text m_WeightText;

    [Header("Buttons")]
    [SerializeField] private Button m_ActionButton;
    [SerializeField] private Button m_DeleteButton;
    [SerializeField] private Button m_MisclickButton;

    private TMP_Text _actionButtonText;
    private Item _currentItem;

    private ItemUsageController _itemUsageController;
    [Inject]
    public void Construct(ItemUsageController itemUsageController)
    {
        _itemUsageController = itemUsageController;
    }

    private void Start()
    {
        _actionButtonText = m_ActionButton.GetComponentInChildren<TMP_Text>();

        m_ActionButton.onClick.AddListener(OnClickActionButton);
        m_DeleteButton.onClick.AddListener(OnClickDeleteButton);
        m_MisclickButton.onClick.AddListener(ClosePopup);

        ClosePopup();
    }

    private void OnDestroy()
    {
        m_ActionButton.onClick.RemoveAllListeners();
        m_DeleteButton.onClick.RemoveAllListeners();
        m_MisclickButton.onClick.RemoveAllListeners();
    }

    public void ResreshPopup(Item item)
    {
        _currentItem = item;

        var config = item.Config;

        m_Title.text = config.Name;
        m_ItemIcon.sprite = config.ItemIcon;
        m_AmountText.text = "x" + item.ParentSlot.ItemAmount.ToString();
        m_PropertyIcon.sprite = config.PropertyIcon;
        m_PropertyValueText.text = "+" + config.PropertyValue.ToString();
        m_WeightText.text = (config.Weigt * item.ParentSlot.ItemAmount).ToString() + " Í„";

        _actionButtonText.text = config.m_PopupActionButtonText;
    }

    private void OnClickActionButton()
    {
        _itemUsageController.UseItem(_currentItem);

        ClosePopup();
    }

    private void OnClickDeleteButton()
    {
        if (_currentItem != null)
        {
            _currentItem.RemoveFromInventory();
        }

        ClosePopup();
    }

    private void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}