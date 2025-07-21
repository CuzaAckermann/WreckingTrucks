using System;
using System.Collections.Generic;

public class SwapAbilityStateTEMP : GameState
{
    //private readonly IPresenterDetector<BlockPresenter> _blockPresenterDetector;
    //private readonly SwapAbilityInputHandler _inputHandler;
    //private readonly BlockTypeSwaperTEMP _blockTypeSwaper;
    //private readonly Mover _mover;

    //private RainFiller _rainFiller;

    //private List<RecordModelToPosition<Type>> _recordOfTypes;
    //private List<RecordModelToPosition<Model>> _recordsOfModels;
    //private Field _field;

    //private bool _isActivated;

    //public SwapAbilityStateTEMP(IPresenterDetector<BlockPresenter> blockPresenterDetector,
    //                        SwapAbilityInputHandler inputHandler,
    //                        BlockTypeSwaperTEMP typeToTypeSwaper,
    //                        Mover mover,
    //                        RainFiller rainFiller)
    //{
    //    _blockPresenterDetector = blockPresenterDetector ?? throw new ArgumentNullException(nameof(blockPresenterDetector));
    //    _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
    //    _blockTypeSwaper = typeToTypeSwaper ?? throw new ArgumentNullException(nameof(typeToTypeSwaper));
    //    _mover = mover ?? throw new ArgumentNullException(nameof(mover));
    //    _rainFiller = rainFiller ?? throw new ArgumentNullException(nameof(rainFiller));
    //}

    //public event Action AbilityStarting;
    //public event Action AbilityFinished;

    //public void Prepare(Field blocksField)
    //{
    //    _blockTypeSwaper.SetField(blocksField);
    //    _field = blocksField ?? throw new ArgumentNullException(nameof(blocksField));
    //}

    //public override void Enter()
    //{
    //    base.Enter();

    //    _blockTypeSwaper.SwapFinished += OnAbilityFinished;
    //    _inputHandler.InteractPressed += OnInteractPressed;
    //    _mover.Enable();
    //}

    //public override void Update(float deltaTime)
    //{
    //    _inputHandler.Update();

    //    if (_isActivated)
    //    {
    //        _rainFiller.Tick(deltaTime);
    //    }

    //    _mover.Tick(deltaTime);
    //}

    //public override void Exit()
    //{
    //    _inputHandler.InteractPressed -= OnInteractPressed;
    //    _blockTypeSwaper.SwapFinished -= OnAbilityFinished;
    //    _mover.Disable();

    //    base.Exit();
    //}

    //private void OnInteractPressed()
    //{
    //    if (_blockPresenterDetector.TryGetPresenter(out BlockPresenter blockPresenter))
    //    {
    //        if (blockPresenter.Model is Block block)
    //        {
    //            _inputHandler.InteractPressed -= OnInteractPressed;
                
    //            AbilityStarting?.Invoke();
    //            _recordOfTypes = _blockTypeSwaper.GetRecordOfTypes(block.GetType());
    //            _blockTypeSwaper.ExtractionLegacyModelsCompleted += OnExtractionLegacyModelsCompleted;

    //            _isActivated = true;
    //        }
    //    }
    //}

    //private void OnExtractionLegacyModelsCompleted()
    //{
    //    _blockTypeSwaper.ExtractionLegacyModelsCompleted -= OnExtractionLegacyModelsCompleted;

    //    List<Type> types = CreateProductionPlan(_recordOfTypes);
    //    _recordsOfModels = CreateRecordsOfModels(types);
    //    _mover.Disable();
    //    _mover.SetNotifier(_field);
    //    _mover.Enable();
    //}

    //private void OnAbilityFinished()
    //{
    //    AbilityFinished?.Invoke();
    //}

    //private List<RecordModelToPosition<Model>> CreateRecordsOfModels(List<Type> types)
    //{
    //    List<RecordModelToPosition<Model>> recordsOfModel = new List<RecordModelToPosition<Model>>();

    //    for (int i = 0; i < types.Count; i++)
    //    {
    //        recordsOfModel.Add(new RecordModelToPosition<Model>(_blockProduction.CreateModel(types[i]),
    //                                                            _recordOfTypes[i].NumberOfRow,
    //                                                            _recordOfTypes[i].NumberOfColumn));
    //    }

    //    return recordsOfModel;
    //}

    //private List<Type> CreateProductionPlan(List<RecordModelToPosition<Type>> recordOfTypes)
    //{
    //    List<Type> types = new List<Type>(recordOfTypes.Count);

    //    for (int i = 0; i < recordOfTypes.Count; i++)
    //    {
    //        types.Add(recordOfTypes[i].PlaceableModel);
    //    }

    //    return types;
    //}

    //private void OnTargetPositionReached(Model model)
    //{
    //    if (_recordsOfModels.Count > 0)
    //    {
    //        for (int i = 0; i < _recordsOfModels.Count; i++)
    //        {
    //            if (_recordsOfModels[i].PlaceableModel == model)
    //            {
    //                _recordsOfModels[i].PlaceableModel.TargetPositionReached -= OnTargetPositionReached;
    //                _recordsOfModels.RemoveAt(i);
    //            }
    //        }

    //        if (_recordsOfModels.Count == 0)
    //        {
    //            AbilityFinished?.Invoke();
    //        }
    //    }
    //}
}