using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FillingStrategy<M> : ICompletionNotifier where M : Model
{
    private readonly StopwatchWaitingState _stopwatchWaitingState;
    private readonly SpawnDetectorWaitingState _spawnDetectorWaitingState;

    private readonly ModelFactory<M> _modelFactory;
    
    private readonly int _spawnDistance;

    private IFillable _field;
    private IRecordStorage _recordStorage;

    //private RecordPlaceableModel _waitingRecord;

    //private bool _isWaitingDetector;

    private bool _isRecordStorageIsEmpty;

    private bool _needSubscribeToRecordStorage;
    private bool _isSubscribedToRecordStorage;

    public FillingStrategy(Stopwatch stopwatch,
                           float frequency,
                           SpawnDetector spawnDetector,
                           ModelFactory<M> modelFactory,
                           int spawnDistance)
    {
        _modelFactory = modelFactory ?? throw new ArgumentNullException(nameof(modelFactory));

        _stopwatchWaitingState = new StopwatchWaitingState(stopwatch, frequency);
        _spawnDetectorWaitingState = new SpawnDetectorWaitingState(spawnDetector);

        _spawnDistance = spawnDistance > 0 ? spawnDistance : throw new ArgumentOutOfRangeException(nameof(spawnDistance));

        //_isWaitingDetector = false;

        _isRecordStorageIsEmpty = false;

        _needSubscribeToRecordStorage = false;
        _isSubscribedToRecordStorage = false;
    }

    public event Action FillingFinished;
    public event Action Completed;

    public void ActivateNonstopFilling()
    {
        _needSubscribeToRecordStorage = true;
    }

    public void Clear()
    {
        _recordStorage?.Clear();
    }

    public List<ColorType> GetUniqueStoredColors()
    {
        return new List<ColorType>(_recordStorage.GetUniqueStoredColors());
    }

    public void PrepareFilling(IFillable field, IRecordStorage recordStorage)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _recordStorage = recordStorage ?? throw new ArgumentNullException(nameof(recordStorage));
    }

    public void Enable()
    {
        if (_isRecordStorageIsEmpty == false)
        {
            _stopwatchWaitingState.Enter(ExecuteFillingStep);
        }

        //if (_isWaitingDetector && _isRecordStorageIsEmpty == false)
        //{
        //    _spawnDetectorWaitingState.Enter(GetGlobalSpawnPosition(_waitingRecord), -_field.Forward, OnEmpty);
        //}

        if (_isSubscribedToRecordStorage == false && _needSubscribeToRecordStorage && _isRecordStorageIsEmpty)
        {
            Logger.Log(4);

            _recordStorage.RecordAppeared += OnRecordAppeared;
            _isSubscribedToRecordStorage = true;
        }
    }

    public void Disable()
    {
        _stopwatchWaitingState.Exit();

        _spawnDetectorWaitingState.Exit();
        
        if (_isSubscribedToRecordStorage)
        {
            _recordStorage.RecordAppeared -= OnRecordAppeared;
            _isSubscribedToRecordStorage = false;
        }
    }

    protected abstract void Fill(IRecordStorage recordStorage);

    protected virtual bool TryGetRecord(IRecordStorage recordStorage, out RecordPlaceableModel record)
    {
        //record = null;

        //if (recordStorage.Amount > 0)
        //{
        //    recordStorage.TryGetNextRecord(out record);
        //}

        recordStorage.TryGetNextRecord(out record);

        return record != null;
    }

    protected virtual Vector3 GetLocalSpawnPosition(RecordPlaceableModel record)
    {
        return _field.Right * record.IndexOfColumn * _field.IntervalBetweenColumns +
               _field.Up * record.IndexOfLayer * _field.IntervalBetweenLayers +
               _field.Forward * (Mathf.Max(_spawnDistance, _field.AmountRows + 1)) * _field.IntervalBetweenRows;
    }

    protected void PlaceModel(RecordPlaceableModel record)
    {
        Vector3 spawnPosition = GetGlobalSpawnPosition(record);

        M model = _modelFactory.Create();
        model.SetColor(record.Color);
        model.SetFirstPosition(spawnPosition);

        //_field.AddModel(model,
        //                record.IndexOfLayer,
        //                record.IndexOfColumn);

        _field.InsertModel(model,
                           record.IndexOfLayer,
                           record.IndexOfColumn,
                           record.IndexOfRow);
    }

    protected void OnFillingFinished()
    {
        _stopwatchWaitingState.Exit();

        FillingFinished?.Invoke();
        Completed?.Invoke();

        _isRecordStorageIsEmpty = true;

        if (_needSubscribeToRecordStorage)
        {
            //Logger.Log(_recordStorage.Amount);

            //if (_recordStorage.Amount > 0)
            //{
            //    _stopwatchWaitingState.Enter(ExecuteFillingStep);
            //}
            //else
            //{
            //    _recordStorage.RecordAppeared += OnRecordAppeared;
            //    _isSubscribedToRecordStorage = true;
            //}

            _recordStorage.RecordAppeared += OnRecordAppeared;
            _isSubscribedToRecordStorage = true;
        }
    }

    private Vector3 GetGlobalSpawnPosition(RecordPlaceableModel record)
    {
        return GetLocalSpawnPosition(record) + _field.Position;
    }

    private void ExecuteFillingStep()
    {
        Fill(_recordStorage);
    }

    //private void OnEmpty()
    //{
    //    _spawnDetectorWaitingState.Exit();

    //    _isWaitingDetector = false;

    //    _stopwatchWaitingState.Enter(ExecuteFillingStep);
    //}

    private void OnRecordAppeared()
    {
        _isRecordStorageIsEmpty = false;

        _stopwatchWaitingState.Enter(ExecuteFillingStep);
    }
}