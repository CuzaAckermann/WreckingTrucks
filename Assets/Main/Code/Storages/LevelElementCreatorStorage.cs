using System.Collections.Generic;

public class LevelElementCreatorStorage
{
    private readonly ColumnCreator _columnCreator;
    private readonly LayerCreator _layerCreator;

    private readonly RowGeneratorCreator _rowGeneratorCreator;
    private readonly ModelColorGeneratorCreator _truckGeneratorCreator;

    private readonly FillingStrategiesCreator _fillingStrategiesCreator;

    public LevelElementCreatorStorage(CommonLevelSettings commonLevelSettings,
                                      EventBus eventBus,
                                      Production production,
                                      BezierCurveSettings additionalRoadSettings)
    {
        _columnCreator = new ColumnCreator();
        _layerCreator = new LayerCreator(_columnCreator);

        _rowGeneratorCreator = new RowGeneratorCreator(new List<ColorType>(commonLevelSettings.NonstopGameSettings.GeneratedColorTypes));
        _truckGeneratorCreator = new ModelColorGeneratorCreator(commonLevelSettings.GlobalSettings.ModelTypeGeneratorSettings);

        _fillingStrategiesCreator = new FillingStrategiesCreator(eventBus,
                                                                 commonLevelSettings.GlobalSettings.FillerSettings,
                                                                 production);

        BlockFieldCreator = new BlockFieldCreator(_layerCreator);
        TruckFieldCreator = new TruckFieldCreator(_layerCreator);
        CartrigeBoxFieldCreator = new CartrigeBoxFieldCreator(_layerCreator);

        BlockFillingCardCreator = new BlockFillingCardCreator();
        RecordStorageCreator = new RecordStorageCreator(_rowGeneratorCreator);

        BlockFillerCreator = new BlockFillerCreator(_fillingStrategiesCreator);

        TruckFillerCreator = new TruckFillerCreator(_fillingStrategiesCreator,
                                                     _truckGeneratorCreator);

        CartrigeBoxFillerCreator = new CartrigeBoxFillerCreator(_fillingStrategiesCreator);

        RoadCreator = new RoadCreator(additionalRoadSettings);
        ModelSlotCreator = new ModelSlotCreator();
        ModelPlacerCreator = new ModelPlacerCreator(production);
        DispencerCreator = new DispencerCreator();

    }

    public BlockFieldCreator BlockFieldCreator { get; }
    public TruckFieldCreator TruckFieldCreator { get; }
    public CartrigeBoxFieldCreator CartrigeBoxFieldCreator { get; }

    public BlockFillingCardCreator BlockFillingCardCreator { get; }
    public RecordStorageCreator RecordStorageCreator { get; }

    public BlockFillerCreator BlockFillerCreator { get; }
    public TruckFillerCreator TruckFillerCreator { get; }
    public CartrigeBoxFillerCreator CartrigeBoxFillerCreator { get; }

    public RoadCreator RoadCreator { get; }
    public ModelSlotCreator ModelSlotCreator { get; }
    public ModelPlacerCreator ModelPlacerCreator { get; }

    public DispencerCreator DispencerCreator { get; }
}