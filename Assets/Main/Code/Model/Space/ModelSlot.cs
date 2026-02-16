using UnityEngine;

public class ModelSlot<M> : Model where M : Model
{
    private const int UseAmount = 1;

    private readonly Placer _placer;
    private readonly ClampedAmount _remainingUses;

    private M _currentModel;

    public ModelSlot(Placeable positionManipulator,
                     IMover mover,
                     IRotator rotator,
                     Transform position,
                     Placer placer,
                     int initialUseCount)
              : base(positionManipulator,
                     mover,
                     rotator)
    {
        Validator.ValidateNotNull(placer);

        _placer = placer;

        _remainingUses = new ClampedAmount(initialUseCount,
                                           0, initialUseCount);

        Placeable.SetPosition(position.position);
        Placeable.SetForward(position.forward);
    }

    public IAmount RemainingUses => _remainingUses;

    public void SetModel(M model)
    {
        _currentModel = model;

        _placer.Place(_currentModel, Placeable.Position + Vector3.right * 10);

        _currentModel.Placeable.SetForward(Placeable.Forward);

        _currentModel.Mover.SetTarget(Placeable.Forward);
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