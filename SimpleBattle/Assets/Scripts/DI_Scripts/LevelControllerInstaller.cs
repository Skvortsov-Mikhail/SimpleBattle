using UnityEngine;
using Zenject;

public class LevelControllerInstaller : MonoInstaller
{
    [SerializeField] private LevelController m_LevelController;

    public override void InstallBindings()
    {
        BindLevelController();
    }

    private void BindLevelController()
    {
        Container.
            Bind<LevelController>().FromInstance(m_LevelController).AsSingle();
    }
}