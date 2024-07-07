using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class UI_EnemyStats : MonoBehaviour
{
    [SerializeField] private TMP_Text m_HPText;

    [SerializeField] private Image m_Fill;

    private Enemy _enemy;
    [Inject]
    public void Construct(Enemy enemy)
    {
        _enemy = enemy;
    }

    private EnemyStats _enemyStats;
    private int _enemyMaxHP;

    private void Start()
    {
        _enemyStats = _enemy.Stats as EnemyStats;

        _enemyMaxHP = _enemyStats.MaxHP;

        _enemyStats.HPUpdated += UpdateHPImageFill;

        UpdateHPImageFill(_enemyStats.CurrentHP);
    }

    private void OnDestroy()
    {
        _enemyStats.HPUpdated -= UpdateHPImageFill;
    }

    private void UpdateHPImageFill(float HPvalue)
    {
        m_HPText.text = HPvalue.ToString();
        m_Fill.fillAmount = HPvalue / _enemyMaxHP;
    }
}