using System;
using System.Collections.Generic;
using Random = System.Random;

public class BlockTypeSwaperTEMP /* : IModelPositionObserver */
{
    //private readonly int _amountReplaceableRows;
    //private readonly Random _random;

    //private Field _field;
    //private ModelFinalizer _modelFinalizer;
    //private List<Model> _legacyModels;
    //private int _currentIndexLegacyModel;

    //private List<Type> _typeOfBlocks = new List<Type>()
    //{
    //    typeof(GreenBlock),
    //    typeof(OrangeBlock),
    //    typeof(PurpleBlock)
    //};

    //public BlockTypeSwaperTEMP(int amountReplaceableRows)
    //{
    //    if (amountReplaceableRows <= 0)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(amountReplaceableRows));
    //    }

    //    _amountReplaceableRows = amountReplaceableRows;
    //    _modelFinalizer = new ModelFinalizer();
    //    _random = new Random();
    //}

    //public event Action<List<Model>> TargetPositionsModelsChanged;
    //public event Action SwapFinished;
    //public event Action ExtractionLegacyModelsCompleted;
    //public event Action<Model> PositionChanged;

    //public void SetField(Field field)
    //{
    //    _field = field ?? throw new ArgumentNullException(nameof(field));
    //    _field.StopShiftModels();
    //}

    //public List<RecordModelToPosition<Type>> GetRecordOfTypes(Type legacyType)
    //{
    //    _legacyModels = GetListSwapedType(legacyType);
    //    _currentIndexLegacyModel = 0;

    //    List<Type> tempTypes = new List<Type>(_typeOfBlocks);
    //    tempTypes.Remove(legacyType);
    //    Type modern = tempTypes[_random.Next(0, tempTypes.Count)];

    //    return CreateModernTypeRecords(_legacyModels, modern);
    //}

    //public void RemoveCurrentSwapedElements()
    //{
    //    if (_currentIndexLegacyModel < _legacyModels.Count)
    //    {
    //        _legacyModels[_currentIndexLegacyModel].SetTargetPosition(GetTargetPosition(_legacyModels[_currentIndexLegacyModel].Position));
    //        TargetPositionsModelsChanged?.Invoke(new List<Model> { _legacyModels[_currentIndexLegacyModel] });
    //        _legacyModels[_currentIndexLegacyModel].TargetPositionReached += OnTargetPositionReached;
    //        _legacyModels[_currentIndexLegacyModel].Destroyed += OnDestroyed;
    //        _currentIndexLegacyModel++;
    //    }
    //}

    //public List<RecordModelToPosition<Type>> CreateModernTypeRecords(List<Model> swapedBlocks,
    //                                                                 Type modern)
    //{
    //    if (swapedBlocks == null)
    //    {
    //        throw new ArgumentNullException(nameof(swapedBlocks));
    //    }

    //    List<RecordModelToPosition<Type>> records = new List<RecordModelToPosition<Type>>();

    //    for (int i = 0; i < swapedBlocks.Count; i++)
    //    {
    //        _field.TryGetNumberOfPositionModel(swapedBlocks[i],
    //                                           out int numberOfRow,
    //                                           out int numberOfColumn);
    //        records.Add(new RecordModelToPosition<Type>(modern,
    //                                                    numberOfRow,
    //                                                    numberOfColumn));
    //    }

    //    return records;
    //}

    //public List<Model> GetListSwapedType(Type swapedType)
    //{
    //    if (swapedType == null)
    //    {
    //        throw new ArgumentNullException(nameof(swapedType));
    //    }

    //    IReadOnlyList<Model> blocks = _field.GetModels(_amountReplaceableRows);
    //    List<Model> swapedBlocks = new List<Model>();

    //    for (int i = 0; i < blocks.Count; i++)
    //    {
    //        if (blocks[i] is Block block)
    //        {
    //            if (block.GetType() == swapedType)
    //            {
    //                swapedBlocks.Add(block);
    //            }
    //        }
    //    }

    //    return swapedBlocks;
    //}

    //private void OnTargetPositionReached(Model model)
    //{
    //    model.TargetPositionReached -= OnTargetPositionReached;
    //    _legacyModels.Remove(model);
    //    _modelFinalizer.FinishModel(model);

    //    if (_legacyModels.Count == 0)
    //    {
    //        _field.ContinueShiftModels();
    //        ExtractionLegacyModelsCompleted?.Invoke();
    //    }
    //}

    //private void OnDestroyed(Model model)
    //{
    //    model.Destroyed -= OnDestroyed;
    //}
}