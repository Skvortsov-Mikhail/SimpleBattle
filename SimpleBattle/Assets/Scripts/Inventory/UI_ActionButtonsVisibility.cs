using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_ActionButtonsVisibility : MonoBehaviour
{
    [SerializeField] private Button m_ShootButton;
    [SerializeField] private Button m_ChoosePistolButton;
    [SerializeField] private Button m_ChooseGunButton;

    private BattleController _battleController;
    [Inject]
    public void Construct(BattleController battleController)
    {
        _battleController = battleController;
    }

    private void Start()
    {
        _battleController.ShootingPossibility += (bool isActive) => m_ShootButton.interactable = isActive;
        _battleController.PistolButtonActivated += (bool isActive) => m_ChoosePistolButton.interactable = isActive;
        _battleController.GunButtonActivated += (bool isActive) => m_ChooseGunButton.interactable = isActive;
    }

    private void OnDestroy()
    {
        _battleController.ShootingPossibility -= (bool isActive) => m_ShootButton.interactable = isActive;
        _battleController.PistolButtonActivated -= (bool isActive) => m_ChoosePistolButton.interactable = isActive;
        _battleController.GunButtonActivated -= (bool isActive) => m_ChooseGunButton.interactable = isActive;
    }
}