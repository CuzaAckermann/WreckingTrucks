using System;
using System.Collections.Generic;

public class LevelSettings
{
    private readonly FillingCard<Type> _fillingCardWithBlocks;
    private readonly FillingCard<Type> _fillingCardWithTrucks;

    public LevelSettings(FillingCard<Type> fillingCardWithBlocks, FillingCard<Type> fillingCardWithTrucks)
    {
        _fillingCardWithBlocks = fillingCardWithBlocks ?? throw new ArgumentNullException(nameof(fillingCardWithBlocks));
        _fillingCardWithTrucks = fillingCardWithTrucks ?? throw new ArgumentNullException(nameof(fillingCardWithTrucks));
    }

    public int WidthBlocksField => _fillingCardWithBlocks.Width;

    public int LengthBlocksField => _fillingCardWithBlocks.Length;

    public int WidthTrucksField => _fillingCardWithTrucks.Width;

    public int LengthTrucksField => _fillingCardWithTrucks.Length;

    public FillingCard<Type> FillingCardWithBlocks => _fillingCardWithBlocks;

    public FillingCard<Type> FillingCardWithTrucks => _fillingCardWithTrucks;
}