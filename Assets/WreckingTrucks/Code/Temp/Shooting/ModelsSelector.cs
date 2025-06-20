using System;
using System.Collections.Generic;

public class ModelsSelector
{
    private readonly Field _field;

    public ModelsSelector(Field field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
    }

    public List<Model> GetBlocksOneType(Type type)
    {
        List<Model> models = _field.GetFirstModels();
        List<Model> selectedModels = new List<Model>();

        for (int i = 0; i < models.Count; i++)
        {
            Block block = models[i] as Block;

            if (block.GetType() == type && block.IsTargetForShooting == false)
            {
                selectedModels.Add(models[i]);
            }
        }

        return selectedModels;
    }
}