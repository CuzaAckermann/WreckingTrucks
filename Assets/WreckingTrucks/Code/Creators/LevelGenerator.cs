using System;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Settings Blokcs Generation")]
    [SerializeField, Min(1)] private int _amountRowsForBlocks = 10;
    [SerializeField, Min(1)] private int _amountColumnsForBlocks = 10;

    [Header("Settings Generation Strategies")]

    [Header("Settings Fixed Period Generator")]
    [SerializeField] private int _lengthPeriod = 2;
    [SerializeField] private int _amountTypes = 2;

    [Header("Settings Random Period Generator")]
    [SerializeField] private int _amountTypesForRandomPeriodGenerator;
    [SerializeField] private int _minPeriod;

    [Header("Settings Trucks Generation")]
    [SerializeField, Min(1)] private int _amountRowsForTrucks = 3;
    [SerializeField, Min(1)] private int _amountColumnsForTrucks = 3;

    private Generator<Block> _blocksGenerator;
    private Generator<Truck> _trucksGenerator;

    public void Initialize()
    {
        InitializeBlocksGenerator();
        InitializeTrucksGenerator();
    }

    public LevelSettings GetLevelSettings()
    {
        SpaceSettings blocksSpaceSettings = new SpaceSettings(_blocksGenerator.GetFillingCardType(_amountRowsForBlocks, _amountColumnsForBlocks));
        SpaceSettings trucksSpaceSettings = new SpaceSettings(_trucksGenerator.GetFillingCardType(_amountRowsForTrucks, _amountColumnsForTrucks));

        return new LevelSettings(blocksSpaceSettings, trucksSpaceSettings);
    }

    private void InitializeBlocksGenerator()
    {
        _blocksGenerator = new Generator<Block>();

        _blocksGenerator.AddType<GreenBlock>();
        _blocksGenerator.AddType<OrangeBlock>();
        _blocksGenerator.AddType<PurpleBlock>();

        _blocksGenerator.AddGenerator(new RowWithFixedPeriodGenerator(1)); // одноцветный ряд
        _blocksGenerator.AddGenerator(new RowWithFixedPeriodGenerator(2)); // половинки
        _blocksGenerator.AddGenerator(new RowWithPeriodicTypesGenerator(_lengthPeriod,  // цикличные
                                                                        _amountTypes)); // периоды
        _blocksGenerator.AddGenerator(new RowWithRandomPeriodGenerator(_amountTypesForRandomPeriodGenerator,
                                                                       _minPeriod));
    }

    private void InitializeTrucksGenerator()
    {
        _trucksGenerator = new Generator<Truck>();

        _trucksGenerator.AddType<GreenTruck>();
        _trucksGenerator.AddType<OrangeTruck>();
        _trucksGenerator.AddType<PurpleTruck>();

        _trucksGenerator.AddGenerator(new RowWithRandomTypesGenerator());
    }
}