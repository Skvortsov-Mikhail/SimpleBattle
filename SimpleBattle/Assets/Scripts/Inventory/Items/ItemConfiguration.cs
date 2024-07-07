using UnityEngine;

[CreateAssetMenu]
public class ItemConfiguration : ScriptableObject
{
    [Header("General")]
    [SerializeField] public string Name;
    [SerializeField] public Sprite ItemIcon;

    [Header("Property")]
    [SerializeField] public ItemType ItemType;
    [SerializeField] public Sprite PropertyIcon;
    [SerializeField] public int PropertyValue;
    [SerializeField] public int RequiredAmountToUse;
    [SerializeField] public int MaxAmountInSlot;
    [SerializeField] public float Weigt;

    [Header("ActionButton")]
    [SerializeField] public string m_PopupActionButtonText;
}