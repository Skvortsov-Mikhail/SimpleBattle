using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] private TMP_Text m_HPText;
    [SerializeField] private TMP_Text m_HelmetArmorText;
    [SerializeField] private TMP_Text m_BodyArmorText;

    [SerializeField] private Image m_Fill;

    private Player _player;
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }

    private PlayerStats _playerStats;
    private int _playerMaxHP;

    private void Start()
    {
        _playerStats = _player.Stats as PlayerStats;
        _playerMaxHP = _playerStats.MaxHP;

        _playerStats.HPUpdated += UpdateHPImageFill;
        _playerStats.HelmetArmorUpdated += UpdateHelmetArmorText;
        _playerStats.BodyArmorUpdated += UpdateBodyArmorText;
        
        UpdateHelmetArmorText(_playerStats.HelmetArmor);
        UpdateBodyArmorText(_playerStats.BodyArmor);
        UpdateHPImageFill(_playerStats.CurrentHP);
    }

    private void OnDestroy()
    {
        _playerStats.HPUpdated -= UpdateHPImageFill;
        _playerStats.HelmetArmorUpdated -= UpdateHelmetArmorText;
        _playerStats.BodyArmorUpdated -= UpdateBodyArmorText;
    }

    private void UpdateHPImageFill(float HPvalue)
    {
        m_HPText.text = HPvalue.ToString();
        m_Fill.fillAmount = HPvalue / _playerMaxHP;
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