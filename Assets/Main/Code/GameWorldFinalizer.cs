using System;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldFinalizer
{
    private readonly ActiveTruckCounter _activeTruckCounter;
    private readonly ActiveBulletCounter _activeBulletCounter;

    private readonly Dispencer _dispencer;

    private bool _isEnabled;

    private bool _isWaitingDispencer;
    private bool _isWaitingTruckCounter;
    private bool _isWaitingBulletCounter;

    public GameWorldFinalizer(Dispencer dispencer)
    {
        _dispencer = dispencer ?? throw new ArgumentNullException(nameof(dispencer));

        _activeTruckCounter = new ActiveTruckCounter();
        _activeBulletCounter = new ActiveBulletCounter();

        _isWaitingDispencer = false;
        _isWaitingTruckCounter = false;
        _isWaitingBulletCounter = false;
    }

    public event Action LevelContinued;
    public event Action LevelFinished;

    public void AddTruck(Truck truck)
    {
        _activeTruckCounter.AddActivedTruck(truck);
        _activeBulletCounter.SubscribeToGun(truck.Gun);
    }

    public void Enable()
    {
        if (_isEnabled)
        {
            return;
        }

        _isEnabled = true;

        if (_activeTruckCounter.Amount == 0 && _activeBulletCounter.Amount == 0)
        {
            LevelFinished?.Invoke();
        }

        if (_activeTruckCounter.Amount > 0)
        {
            SubscribeToTruckCounter();
        }
        else if (_activeBulletCounter.Amount > 0)
        {
            SubscribeToBulletCounter();
        }

        SubscribeToDispencer();
    }

    public void Disable()
    {
        UnsubscribeFromDispencer();

        UnsubscribeFromTruckCounter();
        UnsubscribeFromBulletCounter();

        _isEnabled = false;
    }

    private void SubscribeToTruckCounter()
    {
        if (_isWaitingTruckCounter == false)
        {
            _activeTruckCounter.ActivedTrucksIsEmpty += OnActivedTrucksIsEmpty;

            _isWaitingTruckCounter = true;
        }
        else
        {
            Logger.Log("Already subscribed");
        }
    }

    private void UnsubscribeFromTruckCounter()
    {
        if (_isWaitingTruckCounter)
        {
            _activeTruckCounter.ActivedTrucksIsEmpty -= OnActivedTrucksIsEmpty;

            _isWaitingTruckCounter = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }

    private void OnActivedTrucksIsEmpty()
    {
        UnsubscribeFromTruckCounter();

        if (_activeBulletCounter.Amount > 0)
        {
            SubscribeToBulletCounter();
        }
        else if (_dispencer.Amount == 0)
        {
            UnsubscribeFromDispencer();

            LevelFinished?.Invoke();
        }
    }

    private void SubscribeToBulletCounter()
    {
        if (_isWaitingBulletCounter == false)
        {
            _activeBulletCounter.ActivedBulletIsEmpty += OnActivedBulletIsEmpty;

            _isWaitingBulletCounter = true;
        }
        else
        {
            Logger.Log("Already subscribed");
        }
    }

    private void UnsubscribeFromBulletCounter()
    {
        if (_isWaitingBulletCounter)
        {
            _activeBulletCounter.ActivedBulletIsEmpty -= OnActivedBulletIsEmpty;

            _isWaitingBulletCounter = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }

    private void OnActivedBulletIsEmpty()
    {
        UnsubscribeFromBulletCounter();

        if (_dispencer.Amount == 0)
        {
            UnsubscribeFromDispencer();

            LevelFinished?.Invoke();
        }
    }

    private void SubscribeToDispencer()
    {
        if (_isWaitingDispencer == false)
        {
            _dispencer.RecordAppeared += OnRecordAppeared;

            _isWaitingDispencer = true;
        }
        else
        {
            Logger.Log("Already subscribed");
        }
    }

    private void UnsubscribeFromDispencer()
    {
        if (_isWaitingDispencer)
        {
            _dispencer.RecordAppeared -= OnRecordAppeared;

            _isWaitingDispencer = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }

    private void OnRecordAppeared()
    {
        UnsubscribeFromDispencer();

        UnsubscribeFromTruckCounter();
        UnsubscribeFromBulletCounter();

        LevelContinued?.Invoke();
    }
}