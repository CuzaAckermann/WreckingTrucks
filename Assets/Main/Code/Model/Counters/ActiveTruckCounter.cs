using System;
using System.Collections.Generic;

public class ActiveTruckCounter
{
    private readonly List<Truck> _activedTrucks;

    public ActiveTruckCounter()
    {
        _activedTrucks = new List<Truck>();
    }

    public event Action ActivedTrucksIsEmpty;

    public int Amount => _activedTrucks.Count;

    public void AddActivedTruck(Truck truck)
    {
        if (truck == null)
        {
            throw new ArgumentNullException(nameof(truck));
        }

        if (_activedTrucks.Contains(truck))
        {
            throw new InvalidOperationException($"{nameof(truck)} is contained");
        }

        _activedTrucks.Add(truck);

        SubscribeToActivedTruck(truck);
    }

    private void SubscribeToActivedTruck(Truck truck)
    {
        truck.DestroyedModel += UnsubscribeFromActivedTruck;
        truck.ShootingFinished += UnsubscribeFromActivedTruck;
    }

    private void UnsubscribeFromActivedTruck(Model model)
    {
        model.DestroyedModel -= UnsubscribeFromActivedTruck;

        if (model is Truck truck)
        {
            truck.ShootingFinished -= UnsubscribeFromActivedTruck;

            RemoveActivedTruck(truck);
        }
    }

    private void RemoveActivedTruck(Truck truck)
    {
        _activedTrucks.Remove(truck);

        if (_activedTrucks.Count == 0)
        {
            ActivedTrucksIsEmpty?.Invoke();
        }
    }
}