using UnityEngine;
using Zenject;

public class BattleControllerInstaller : MonoInstaller
{
    [SerializeField] private BattleController m_BattleController;

    public override void InstallBindings()
    {
        BindBattleController();
    }

    private void BindBattleController()
    {
        Container.
            Bind<BattleController>().FromInstance(m_BattleController).AsSingle();
    }
}