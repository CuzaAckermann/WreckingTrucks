using UnityEngine;

public class ModelSlot<M> : Model where M : Model
{
    private const int UseAmount = 1;

    private readonly ClampedAmount _remainingUses;

    private M _currentModel;

    public ModelSlot(PositionManipulator positionManipulator,
                     IMover mover,
                     IRotator rotator,
                     Transform position,
                     int initialUseCount)
              : base(positionManipulator,
                     mover,
                     rotator)
    {
        _remainingUses = new ClampedAmount(initialUseCount,
                                           0, initialUseCount);

        PositionManipulator.SetPosition(position.position);
        PositionManipulator.SetForward(position.forward);
    }

    public IAmount RemainingUses => _remainingUses;

    public void SetModel(M model)
    {
        _currentModel = model;

        _currentModel.SetFirstPosition(PositionManipulator.Position + Vector3.right * 10);

        _currentModel.PositionManipulator.SetForward(PositionManipulator.Forward);

        _currentModel.Mover.SetTarget(PositionManipulator.Forward);
    }

    public bool TryGetModel(out M model)
    {
        model = null;

        if (RemainingUses.Value > 0)
        {
            model = _currentModel;

            _remainingUses.Decrease(UseAmount);

            return true;
        }

        return false;
    }
}