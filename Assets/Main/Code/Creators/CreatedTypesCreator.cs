using System;
using System.Collections.Generic;

public class CreatedTypesCreator<TC> where TC : ITypeConverter
{
    private readonly TC _typeConverter;

    public CreatedTypesCreator(TC typeConverter)
    {
        _typeConverter = typeConverter ?? throw new ArgumentNullException(nameof(typeConverter));
    }

    public List<Type> Create(IReadOnlyList<ColorType> colorType)
    {
        List<Type> types = new List<Type>(colorType.Count);

        for (int i = 0; i < colorType.Count; i++)
        {
            types.Add(_typeConverter.GetModelType(colorType[i]));
        }

        return types;
    }
}