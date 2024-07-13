using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class UI_Enemy : MonoBehaviour
{
    [SerializeField] private TMP_Text m_HPText;
    [SerializeField] private Image m_Fill;

    private Enemy _enemy;
    [Inject]
    public void Construct(Enemy enemy)
    {
        _enemy = enemy;
    }

    private void Start()
    {
        _enemy.EnemyHPUpdated += UpdateHPImageFill;

        UpdateHPImageFill(_enemy.CurrentHP);
    }

    private void OnDestroy()
    {
        _enemy.EnemyHPUpdated -= UpdateHPImageFill;
    }

    private void UpdateHPImageFill(float HPvalue)
    {
        m_HPText.text = HPvalue.ToString();
        m_Fill.fillAmount = HPvalue / _enemy.MaxHP;
    }
}