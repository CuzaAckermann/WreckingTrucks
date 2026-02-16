using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FillingStrategy<M> : ICompletionNotifier,
                                           ICommandCreator where M : Model
{
    private readonly SpawnDetectorWaitingState _spawnDetectorWaitingState;
    private readonly ModelFactory<M> _modelFactory;
    private readonly Placer _placer;

    private readonly int _spawnDistance;
    private readonly float _frequency;

    private IFillable _field;
    private IRecordStorage _recordStorage;

    private Command _currentCommand;

    //private RecordPlaceableModel _waitingRecord;

    //private bool _isWaitingDetector;

    private bool _isRecordStorageIsEmpty;

    private bool _needSubscribeToRecordStorage;
    private bool _isSubscribedToRecordStorage;

    public FillingStrategy(float frequency,
                           SpawnDetector spawnDetector,
                           ModelFactory<M> modelFactory,
                           Placer placer,
                           int spawnDistance)
    {
        Validator.ValidateNotNull(modelFactory, placer);

        _modelFactory = modelFactory;
        _placer = placer;

        _frequency = frequency > 0 ? frequency : throw new ArgumentOutOfRangeException(nameof(frequency));
        _spawnDetectorWaitingState = new SpawnDetectorWaitingState(spawnDetector);

        _spawnDistance = spawnDistance > 0 ? spawnDistance : throw new ArgumentOutOfRangeException(nameof(spawnDistance));

        //_isWaitingDetector = false;

        _isRecordStorageIsEmpty = false;

        _needSubscribeToRecordStorage = false;
        _isSubscribedToRecordStorage = false;
    }

    public event Action FillingFinished;
    public event Action Completed;

    public event Action<IDestroyable> Destroyed;

    public event Action<Command> CommandCreated;

    public void ActivateNonstopFilling()
    {
        _needSubscribeToRecordStorage = true;
    }

    public void Destroy()
    {
        _recordStorage?.Clear();

        Destroyed?.Invoke(this);
    }

    public void Clear()
    {
        _recordStorage?.Clear();

        Destroyed?.Invoke(this);
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
            SendCommand();
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
        CancelCommand();

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

        _placer.Place(model, spawnPosition);

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
        //_recordStorage.RecordAppeared -= OnRecordAppeared;

        CancelCommand();

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

            if (_isSubscribedToRecordStorage == false)
            {
                _recordStorage.RecordAppeared += OnRecordAppeared;
                _isSubscribedToRecordStorage = true;
            }
        }
    }

    private Vector3 GetGlobalSpawnPosition(RecordPlaceableModel record)
    {
        return GetLocalSpawnPosition(record) + _field.Position;
    }

    private void ExecuteFillingStep()
    {
        Fill(_recordStorage);

        if (_recordStorage.Amount > 0)
        {
            SendCommand();
        }
        else
        {
            OnFillingFinished();
        }
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

        SendCommand();
    }

    private void SendCommand()
    {
        _currentCommand = new Command(ExecuteFillingStep, _frequency);

        CommandCreated?.Invoke(_currentCommand);
    }

    private void CancelCommand()
    {
        _currentCommand?.Cancel();

        _currentCommand = null;
    }
}