using System;

[Serializable]
public class CartrigeBoxSpaceSettings : SpaceSettings<CartrigeBoxFieldSettings>
{
    public void SetAmountCartrigeBox(int amountCartrigeBox)
    {
        _fieldSettings.SetAmountCartrigeBoxes(amountCartrigeBox);
    }
}