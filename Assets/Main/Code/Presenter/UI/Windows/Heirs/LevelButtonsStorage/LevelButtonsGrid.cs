using System.Collections.Generic;
using UnityEngine;

public class LevelButtonsGrid : MonoBehaviour
{
    [SerializeField] private List<ButtonWithNumber> _buttonsWithNumber;

    public int AmountButtons => _buttonsWithNumber.Count;

    public bool TryGetByIndex(int index, out ButtonWithNumber buttonWithNumber)
    {
        buttonWithNumber = null;

        if (index >= 0 && index < _buttonsWithNumber.Count)
        {
            buttonWithNumber = _buttonsWithNumber[index];
        }

        return buttonWithNumber != null;
    }

    public bool TryGetByNumber(int number, out ButtonWithNumber buttonWithNumber)
    {
        buttonWithNumber = null;

        for (int currentButton = 0; currentButton < _buttonsWithNumber.Count; currentButton++)
        {
            if (_buttonsWithNumber[currentButton].Number == number)
            {
                buttonWithNumber = _buttonsWithNumber[currentButton];

                break;
            }
        }

        return buttonWithNumber != null;
    }
}