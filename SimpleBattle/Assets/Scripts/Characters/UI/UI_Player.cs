using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class UI_Player : MonoBehaviour
{
    [SerializeField] private TMP_Text m_HPText;
    [SerializeField] private Image m_Fill;

    [SerializeField] private TMP_Text m_HelmetArmorText;
    [SerializeField] private TMP_Text m_BodyArmorText;

    private Player _player;
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }

    private void Start()
    {
        _player.PlayerHPUpdated += UpdateHPImageFill;
        _player.HelmetArmorUpdated += UpdateHelmetArmorText;
        _player.BodyArmorUpdated += UpdateBodyArmorText;

        UpdateHPImageFill(_player.CurrentHP);
        UpdateHelmetArmorText(_player.HelmetArmor);
        UpdateBodyArmorText(_player.BodyArmor);
    }

    private void OnDestroy()
    {
        _player.PlayerHPUpdated -= UpdateHPImageFill;
        _player.HelmetArmorUpdated -= UpdateHelmetArmorText;
        _player.BodyArmorUpdated -= UpdateBodyArmorText;
    }

    private void UpdateHPImageFill(float HPvalue)
    {
        m_HPText.text = HPvalue.ToString();
        m_Fill.fillAmount = HPvalue / _player.MaxHP;
    }

    private void UpdateHelmetArmorText(int helmetArmorValue)
    {
        m_HelmetArmorText.text = helmetArmorValue.ToString();
    }

    private void UpdateBodyArmorText(int bodyArmorValue)
    {
        m_BodyArmorText.text = bodyArmorValue.ToString();
    }
}