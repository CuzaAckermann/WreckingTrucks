using UnityEngine;

public class BlocksSpaceInitializer : SpaceInitializer<Block, BlockFactory>
{
    [Header("Presenter Factories")]
    [SerializeField] private GreenBlockPresenterFactory _greenBlockPresenterFactory;
    [SerializeField] private OrangeBlockPresenterFactory _orangeBlockPresenterFactory;
    [SerializeField] private PurpleBlockPresenterFactory _purpleBlockPresenterFactory;

    protected override ModelsProduction<Block, BlockFactory> CreateModelsProduction()
    {
        BlocksProduction production = new BlocksProduction();

        production.AddFactory<GreenBlock>(new GreenBlockFactory(_factorySettings.InitialPoolSize,
                                                                _factorySettings.MaxPoolCapacity));

        production.AddFactory<OrangeBlock>(new OrangeBlockFactory(_factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));

        production.AddFactory<PurpleBlock>(new PurpleBlockFactory(_factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));

        return production;
    }

    protected override Field CreateField(LevelSettings levelSettings)
    {
        return new Field(_position.position,
                        _position.forward,
                        _position.right,
                        _intervalBetweenModels,
                        _distanceBetweenModels,
                        levelSettings.WidthBlocksField,
                        levelSettings.LengthBlocksField);
    }

    protected override PresentersProduction<Block> CreatePresentersProduction()
    {
        InitializePresenterFactories();

        PresentersProduction<Block> production = new PresentersProduction<Block>();

        production.AddFactory<GreenBlock>(_greenBlockPresenterFactory);
        production.AddFactory<OrangeBlock>(_orangeBlockPresenterFactory);
        production.AddFactory<PurpleBlock>(_purpleBlockPresenterFactory);

        return production;
    }

    protected override void InitializePresenterFactories()
    {
        _greenBlockPresenterFactory.Initialize();
        _orangeBlockPresenterFactory.Initialize();
        _purpleBlockPresenterFactory.Initialize();
    }
}