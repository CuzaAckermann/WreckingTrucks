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
        List<Model> blocks = _field.GetFirstModels();
        List<Model> selectedModels = new List<Model>();

        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].GetType() == type)
            {
                selectedModels.Add(blocks[i]);
            }
        }

        return selectedModels;
    }
}