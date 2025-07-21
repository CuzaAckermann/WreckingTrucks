using System;

public interface IModelsProduction<M> where M : Model
{
    public M CreateModel(Type modelType);
}