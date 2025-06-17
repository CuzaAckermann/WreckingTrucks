using System;
using System.Collections.Generic;

public class FillingCardModelCreator
{
    private readonly IModelsProduction _modelsProduction;

    public FillingCardModelCreator(IModelsProduction modelsProduction)
    {
        _modelsProduction = modelsProduction ?? throw new ArgumentNullException(nameof(modelsProduction));
    }

    public FillingCard<Model> CreateFillingCard(FillingCard<Type> fillingCard)
    {
        List<Model> models = _modelsProduction.CreateModels(CreateProductionPlan((fillingCard)));
        FillingCard<Model> fillingCardModels = new FillingCard<Model>(fillingCard.Length, fillingCard.Width);

        for (int i = 0; i < models.Count; i++)
        {
            RecordModelToPosition<Type> recordModelToPosition = fillingCard.GetRecord(i);

            fillingCardModels.Add(new RecordModelToPosition<Model>(models[i],
                                                                   recordModelToPosition.NumberOfRow,
                                                                   recordModelToPosition.NumberOfColumn));
        }

        return fillingCardModels;
    }

    private List<Type> CreateProductionPlan(FillingCard<Type> fillingCard)
    {
        var types = new List<Type>(fillingCard.Amount);

        for (int i = 0; i < fillingCard.Amount; i++)
        {
            types.Add(fillingCard.GetRecord(i).PlaceableModel);
        }

        return types;
    }
}