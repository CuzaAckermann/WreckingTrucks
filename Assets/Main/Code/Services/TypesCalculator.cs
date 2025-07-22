using System;
using System.Collections.Generic;

public class TypesCalculator
{
    public Dictionary<Type, int> Calculate(IReadOnlyList<Model> calculatedModels)
    {
        Dictionary<Type, int> amountElementsOfTypes = new Dictionary<Type, int>();

        for (int i = 0; i < calculatedModels.Count; i++)
        {
            Type typeOfModel = calculatedModels[i].GetType();

            if (amountElementsOfTypes.ContainsKey(typeOfModel) == false)
            {
                amountElementsOfTypes.Add(typeOfModel, 0);
            }

            amountElementsOfTypes[typeOfModel]++;
        }

        return amountElementsOfTypes;
    }
}