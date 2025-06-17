using System;
using System.Collections.Generic;

public interface IModelsProduction
{
    public List<Model> CreateModels(List<Type> typeModels);

    public Model CreateModel(Type modelType);
}