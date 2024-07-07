using System;
using Zenject;

public class EnemyStats : Stats
{
    public event Action<float> HPUpdated;

    private Enemy _enemy;
    [Inject]
    public void Construct(Enemy enemy)
    {
        _enemy = enemy;
    }

    private void Start()
    {
        _enemy.EnemyHPUpdated += OnHPUdated;
    }

    private void OnDestroy()
    {
        _enemy.EnemyHPUpdated -= OnHPUdated;
    }

    private void OnHPUdated(float value)
    {
        _currentHP = value;

        HPUpdated?.Invoke(value);
    }
}