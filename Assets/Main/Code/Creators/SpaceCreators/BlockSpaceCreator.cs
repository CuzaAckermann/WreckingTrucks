using System;
using UnityEngine;

public class BlockSpaceCreator
{
    private readonly BlockFieldCreator _fieldCreator;
    private readonly MoverCreator _moverCreator;
    private readonly FillerCreator _fillerCreator;
    private readonly ModelFinalizerCreator _modelFinalizerCreator;

    public BlockSpaceCreator(BlockFieldCreator fieldCreator,
                             MoverCreator moverCreator,
                             FillerCreator fillerCreator,
                             ModelFinalizerCreator modelFinalizerCreator)
    {
        _fieldCreator = fieldCreator ?? throw new ArgumentNullException(nameof(fieldCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _fillerCreator = fillerCreator ?? throw new ArgumentNullException(nameof(fillerCreator));
        _modelFinalizerCreator = modelFinalizerCreator ?? throw new ArgumentNullException(nameof(modelFinalizerCreator));
    }

    public BlockSpace Create(Transform fieldTransform, BlockSpaceSettings blockSpaceSettings)
    {
        Field field = _fieldCreator.Create(fieldTransform, blockSpaceSettings.FieldSettings.FieldSize);

        return new BlockSpace(field,
                              _moverCreator.Create(field, blockSpaceSettings.MoverSettings),
                              _fillerCreator.Create(field),
                              _modelFinalizerCreator.Create());
    }
}