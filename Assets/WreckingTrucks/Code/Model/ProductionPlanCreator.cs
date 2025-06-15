using System;
using System.Collections.Generic;

public class ProductionPlanCreator
{
    public List<Type> CreateProductionPlan(FillingCard<Type> fillingCard)
    {
        var types = new List<Type>(fillingCard.Amount);

        for (int i = 0; i < fillingCard.Amount; i++)
        {
            types.Add(fillingCard.GetRecord(i).PlaceableModel);
        }

        return types;
    }
}