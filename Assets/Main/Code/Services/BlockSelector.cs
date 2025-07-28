using System;
using System.Collections.Generic;

public class BlockSelector
{
    private readonly Field _field;

    public BlockSelector(Field field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
    }

    public List<Block> GetModelsType(Type type)
    {
        List<Model> models = _field.GetFirstModels();
        List<Block> selectedModels = new List<Block>();

        for (int i = 0; i < models.Count; i++)
        {
            if (models[i] is Block block)
            {
                if (block.GetType() != type)
                {
                    continue;
                }

                if (block.IsTargetForShooting)
                {
                    continue;
                }

                selectedModels.Add(block);
            }
        }

        return selectedModels;
    }
}