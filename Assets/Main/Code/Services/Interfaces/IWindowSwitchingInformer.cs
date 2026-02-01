using System;

public interface IWindowSwitchingInformer
{
    public event Action HideMainMenuActivated;

    public event Action MainMenuActivated;

    public event Action OptionsMenuActivated;

    public event Action ShopMenuActivated;

    public event Action PlayingMenuActivated;

    public event Action SwapAbilityMenuActivated;

    public event Action PauseMenuActivated;

    public event Action ReturnActivated;
}