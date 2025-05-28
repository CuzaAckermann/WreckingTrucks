using UnityEngine;

public class Productions : MonoBehaviour
{
    [Header("Model Production Settings")]

    [Header("Blocks Production Settings")]
    [SerializeField] private int _initialPoolSizeForBlocks = 100;
    [SerializeField] private int _maxPoolCapacityForBlocks = 250;

    private BlocksProduction _blocksProduction;

    [Header("Trucks Production Settings")]
    [SerializeField] private int _initialPoolSizeForTrucks = 10;
    [SerializeField] private int _maxPoolCapacityForTrucks = 20;

    private TrucksProduction _trucksProduction;

    [Header("Presenter Production Settings")]

    [Header("Block Presenters Production Settings")]
    [SerializeField] private GreenBlockPresenterFactory _greenBlockPresenterFactory;
    [SerializeField] private OrangeBlockPresenterFactory _orangeBlockPresenterFactory;
    [SerializeField] private PurpleBlockPresenterFactory _purpleBlockPresenterFactory;

    private BlockPresentersProduction _blockPresentersProduction;

    [Header("Truck Presenters Production Settings")]
    [SerializeField] private GreenTruckPresenterFactory _greenTruckPresenterFactory;
    [SerializeField] private OrangeTruckPresenterFactory _orangeTruckPresenterFactory;
    [SerializeField] private PurpleTruckPresenterFactory _purpleTruckPresenterFactory;

    private TruckPresentersProduction _truckPresentersProduction;

    public BlocksProduction BlocksProduction => _blocksProduction;

    public TrucksProduction TrucksProduction => _trucksProduction;

    public BlockPresentersProduction BlockPresentersProduction => _blockPresentersProduction;

    public TruckPresentersProduction TruckPresentersProduction => _truckPresentersProduction;

    public void Initialize()
    {
        PrepareProductions();
    }

    private void PrepareProductions()
    {
        PrepareBlocksProduction();
        PrepareTrucksProduction();
        PrepareBlockPresentersProduction();
        PrepareTruckPresentersProduction();
    }

    private void PrepareBlocksProduction()
    {
        _blocksProduction = new BlocksProduction();

        _blocksProduction.AddFactory<GreenBlock>(new GreenBlockFactory(_initialPoolSizeForBlocks, _maxPoolCapacityForBlocks));
        _blocksProduction.AddFactory<OrangeBlock>(new OrangeBlockFactory(_initialPoolSizeForBlocks, _maxPoolCapacityForBlocks));
        _blocksProduction.AddFactory<PurpleBlock>(new PurpleBlockFactory(_initialPoolSizeForBlocks, _maxPoolCapacityForBlocks));
    }

    private void PrepareTrucksProduction()
    {
        _trucksProduction = new TrucksProduction();

        _trucksProduction.AddFactory<GreenTruck>(new GreenTruckFactory(_initialPoolSizeForTrucks, _maxPoolCapacityForTrucks));
        _trucksProduction.AddFactory<OrangeTruck>(new OrangeTruckFactory(_initialPoolSizeForTrucks, _maxPoolCapacityForTrucks));
        _trucksProduction.AddFactory<PurpleTruck>(new PurpleTruckFactory(_initialPoolSizeForTrucks, _maxPoolCapacityForTrucks));
    }

    private void PrepareBlockPresentersProduction()
    {
        _blockPresentersProduction = new BlockPresentersProduction();

        _greenBlockPresenterFactory.Initialize();
        _orangeBlockPresenterFactory.Initialize();
        _purpleBlockPresenterFactory.Initialize();

        _blockPresentersProduction.AddFactory<GreenBlock>(_greenBlockPresenterFactory);
        _blockPresentersProduction.AddFactory<OrangeBlock>(_orangeBlockPresenterFactory);
        _blockPresentersProduction.AddFactory<PurpleBlock>(_purpleBlockPresenterFactory);
    }

    private void PrepareTruckPresentersProduction()
    {
        _truckPresentersProduction = new TruckPresentersProduction();

        _greenTruckPresenterFactory.Initialize();
        _orangeTruckPresenterFactory.Initialize();
        _purpleTruckPresenterFactory.Initialize();

        _truckPresentersProduction.AddFactory<GreenTruck>(_greenTruckPresenterFactory);
        _truckPresentersProduction.AddFactory<OrangeTruck>(_orangeTruckPresenterFactory);
        _truckPresentersProduction.AddFactory<PurpleTruck>(_purpleTruckPresenterFactory);
    }
}