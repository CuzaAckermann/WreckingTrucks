using UnityEngine;

public class BlocksSpaceCreator : SpaceCreator<Block, BlockFactory>
{
    [Header("Filler Settings")]

    [Header("Row Filler Settings")]
    [SerializeField] protected float _frequencyForRowFiller = 0.5f;

    [Header("Cascade Filler Settings")]
    [SerializeField] protected float _frequencyForCascadeFiller = 0.05f;

    [Header("Rain Filler Settings")]
    [SerializeField] private float _frequencyForRainFiller = 0.1f;
    [SerializeField] private int _minAmountModelsAtTime = 3;
    [SerializeField] private int _maxAmountModelsAtTime = 5;
    [SerializeField] private int _rainHeight = 20;

    [Header("Presenter Factories")]
    [SerializeField] private GreenBlockPresenterFactory _greenBlockPresenterFactory;
    [SerializeField] private OrangeBlockPresenterFactory _orangeBlockPresenterFactory;
    [SerializeField] private PurpleBlockPresenterFactory _purpleBlockPresenterFactory;

    protected override void InitializePresenterFactories()
    {
        _greenBlockPresenterFactory.Initialize();
        _orangeBlockPresenterFactory.Initialize();
        _purpleBlockPresenterFactory.Initialize();
    }

    protected override void CastomizeModelsProduction(ModelsProduction<Block, BlockFactory> production)
    {
        production.AddFactory<GreenBlock>(new GreenBlockFactory(_factorySettings.InitialPoolSize,
                                                                _factorySettings.MaxPoolCapacity));

        production.AddFactory<OrangeBlock>(new OrangeBlockFactory(_factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));

        production.AddFactory<PurpleBlock>(new PurpleBlockFactory(_factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));
    }

    protected override void CastomizePresentersProduction(PresentersProduction<Block> production)
    {
        production.AddFactory<GreenBlock>(_greenBlockPresenterFactory);
        production.AddFactory<OrangeBlock>(_orangeBlockPresenterFactory);
        production.AddFactory<PurpleBlock>(_purpleBlockPresenterFactory);
    }

    protected override void CastomizeFiller(Filler filler)
    {
        filler.AddFillingStrategy(new RowFiller(_frequencyForRowFiller));
        filler.AddFillingStrategy(new CascadeFiller(_frequencyForCascadeFiller));
        //filler.AddFillingStrategy(new RainFiller(_frequencyForRainFiller,
        //                                         _minAmountModelsAtTime,
        //                                         _maxAmountModelsAtTime,
        //                                         _rainHeight));
    }
}