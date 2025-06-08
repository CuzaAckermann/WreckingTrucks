using System;
using UnityEngine;

public class LevelSettingsGenerator : MonoBehaviour
{
    [Header("Settings Blokcs Generation")]
    [SerializeField, Min(1)] private int _amountRowsForBlocks = 10;
    [SerializeField, Min(1)] private int _amountColumnsForBlocks = 10;

    [Header("Settings Generation Strategies")]
    [SerializeField] private int _intervalForRowWithTwoAlternatingRandomTypesGenerator = 2;

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
        FillingCard<Type> fillingCardWithBlocks = _blocksGenerator.GetFillingCardType(_amountRowsForBlocks, _amountColumnsForBlocks);
        FillingCard<Type> fillingCardWithTrucks = _trucksGenerator.GetFillingCardType(_amountRowsForTrucks, _amountColumnsForTrucks);

        return new LevelSettings(fillingCardWithBlocks, fillingCardWithTrucks);
    }

    private void InitializeBlocksGenerator()
    {
        _blocksGenerator = new Generator<Block>();

        _blocksGenerator.AddType<GreenBlock>();
        _blocksGenerator.AddType<OrangeBlock>();
        _blocksGenerator.AddType<PurpleBlock>();

        _blocksGenerator.AddGenerator(new RowWithOneTypeGenerator());
        _blocksGenerator.AddGenerator(new RowWithTwoTypesWithRandomMiddleGenerator());
        _blocksGenerator.AddGenerator(new RowWithTwoTypesInHalfGenerator());
        _blocksGenerator.AddGenerator(new RowWithTwoPeriodicRandomTypesGenerator(_intervalForRowWithTwoAlternatingRandomTypesGenerator));
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