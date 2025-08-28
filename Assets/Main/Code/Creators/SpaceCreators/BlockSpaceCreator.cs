using System;
using UnityEngine;

public class BlockSpaceCreator
{
    private readonly BlockFieldCreator _fieldCreator;
    private readonly MoverCreator _moverCreator;
    private readonly FillerCreator _fillerCreator;
    private readonly BlockFillingCardCreator _fillingCardCreator;

    public BlockSpaceCreator(BlockFieldCreator fieldCreator,
                             MoverCreator moverCreator,
                             FillerCreator fillerCreator,
                             BlockFillingCardCreator blockFillingCardCreator)
    {
        _fieldCreator = fieldCreator ?? throw new ArgumentNullException(nameof(fieldCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _fillerCreator = fillerCreator ?? throw new ArgumentNullException(nameof(fillerCreator));
        _fillingCardCreator = blockFillingCardCreator ?? throw new ArgumentNullException(nameof(blockFillingCardCreator));
    }

    public BlockSpace Create(Transform fieldTransform, BlockSpaceSettings blockSpaceSettings)
    {
        Field field = _fieldCreator.Create(fieldTransform,
                                           blockSpaceSettings.FieldSettings.FieldSize,
                                           blockSpaceSettings.FieldIntervals);

        return new BlockSpace(field,
                              _moverCreator.Create(field, blockSpaceSettings.MoverSettings),
                              _fillerCreator.Create(blockSpaceSettings.FillerSettings,
                                                    field,
                                                    _fillingCardCreator.Create(blockSpaceSettings.FieldSettings)));
    }
}