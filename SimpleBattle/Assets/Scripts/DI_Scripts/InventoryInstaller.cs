using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] private Inventory m_Inventory;

    public override void InstallBindings()
    {
        BindInventory();
    }

    private void BindInventory()
    {
        Container.
            Bind<Inventory>().FromInstance(m_Inventory).AsSingle();
    }
}