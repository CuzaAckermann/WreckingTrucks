using System.Collections.Generic;

public class TypesCalculator
{
    public Dictionary<ColorType, int> Calculate(IReadOnlyList<Model> calculatedModels)
    {
        Dictionary<ColorType, int> amountElementsOfTypes = new Dictionary<ColorType, int>();

        for (int i = 0; i < calculatedModels.Count; i++)
        {
            ColorType typeOfModel = calculatedModels[i].Color;

            if (amountElementsOfTypes.ContainsKey(typeOfModel) == false)
            {
                amountElementsOfTypes.Add(typeOfModel, 0);
            }

            amountElementsOfTypes[typeOfModel]++;
        }

        return amountElementsOfTypes;
    }
}