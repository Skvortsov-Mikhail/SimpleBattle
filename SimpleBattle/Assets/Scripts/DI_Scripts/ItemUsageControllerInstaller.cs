using UnityEngine;
using Zenject;

public class ItemUsageControllerInstaller : MonoInstaller
{
    [SerializeField] private ItemUsageController m_ItemUsageController;

    public override void InstallBindings()
    {
        BindItemUsageController();
    }

    private void BindItemUsageController()
    {
        Container.
            Bind<ItemUsageController>().FromInstance(m_ItemUsageController).AsSingle();
    }
}