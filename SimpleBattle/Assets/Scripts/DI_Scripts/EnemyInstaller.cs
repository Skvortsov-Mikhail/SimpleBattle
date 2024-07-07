using UnityEngine;
using Zenject;

public class EnemyInstaller : MonoInstaller
{
    [SerializeField] private Enemy m_Enemy;

    public override void InstallBindings()
    {
        BindEnemy();
    }

    private void BindEnemy()
    {
        Container.
            Bind<Enemy>().FromInstance(m_Enemy).AsSingle();
    }
}