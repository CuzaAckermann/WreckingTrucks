using System;
using System.Collections.Generic;

public class OneTypeModelSelector
{
    public List<Model> Select<M>(Field field) where M : Model
    {
        if (field == null)
        {
            throw new ArgumentNullException(nameof(field));
        }

        IReadOnlyList<Model> models = field.GetModels();
        Type requiredType = typeof(M);
        List<Model> requiredModels = new List<Model>();

        foreach (Model model in models)
        {
            if (model.GetType() == requiredType)
            {
                requiredModels.Add(model);
            }
        }

        return requiredModels;
    }
}