using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class TruckSelector
{
    private readonly EventBus _eventBus;
    private readonly TypesCalculator _typesCalculator;

    private BlockField _blockField;
    private TruckField _truckField;

    public TruckSelector(EventBus eventBus, TypesCalculator typesCalculator)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _typesCalculator = typesCalculator ?? throw new ArgumentNullException(nameof(typesCalculator));

        _eventBus.Subscribe<ClearedSignal<Level>>(Clear);

        _eventBus.Subscribe<CreatedSignal<BlockField>>(SetBlockField);
        _eventBus.Subscribe<CreatedSignal<TruckField>>(SetTruckField);
    }

    public bool TrySelectTruck(out Truck truck)
    {
        return TrySelectBestTruck(out truck) || TrySelectRandomTruck(out truck);
    }

    private bool TrySelectBestTruck(out Truck truck)
    {
        Dictionary<ColorType, int> amountElementsOfTypes = _typesCalculator.Calculate(_blockField.GetFirstModels());

        var sortedByValueDesc = amountElementsOfTypes.OrderByDescending(pair => pair.Value);

        truck = null;

        foreach (var targetType in sortedByValueDesc)
        {
            for (int i = 0; i < _truckField.AmountColumns; i++)
            {
                if (_truckField.TryGetFirstModel(0, i, out Model model) == false)
                {
                    continue;
                }

                if (model is Truck selectedTruck == false)
                {
                    continue;
                }

                if (selectedTruck.DestroyableColor != targetType.Key)
                {
                    continue;
                }

                truck = selectedTruck;

                return true;
            }
        }

        return false;
    }

    private bool TrySelectRandomTruck(out Truck truck)
    {
        truck = null;

        int randomIndex = Random.Range(0, _truckField.AmountColumns);
        _truckField.TryGetFirstModel(0, randomIndex, out Model model);

        if (model is Truck randomTruck)
        {
            truck = randomTruck;

            return true;
        }

        return false;
    }

    private void Clear(ClearedSignal<Level> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<Level>>(Clear);

        _eventBus.Unsubscribe<CreatedSignal<BlockField>>(SetBlockField);
        _eventBus.Unsubscribe<CreatedSignal<TruckField>>(SetTruckField);
    }

    private void SetBlockField(CreatedSignal<BlockField> blockFieldCreatedSignal)
    {
        _eventBus.Unsubscribe<CreatedSignal<BlockField>>(SetBlockField);

        _blockField = blockFieldCreatedSignal.Creatable;
    }

    private void SetTruckField(CreatedSignal<TruckField> createdTruckFieldSignal)
    {
        _eventBus.Unsubscribe<CreatedSignal<TruckField>>(SetTruckField);

        _truckField = createdTruckFieldSignal.Creatable;
    }
}