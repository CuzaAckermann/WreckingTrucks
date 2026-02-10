using System;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Model
{
    private readonly ShootingState _shootingState;

    private readonly int _amountDestroyedRows;

    private Field _field;

    private Road _road;
    private int _currentPoint;

    public Plane(PositionManipulator positionManipulator,
                 IMover mover,
                 IRotator rotator,
                 Trunk trunk,
                 int amountDestroyedRows)
          : base(positionManipulator,
                 mover,
                 rotator)
    {
        if (amountDestroyedRows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountDestroyedRows));
        }

        Trunk = trunk ?? throw new ArgumentNullException(nameof(trunk));

        _amountDestroyedRows = amountDestroyedRows;

        _shootingState = new ShootingState();

        PositionManipulator.PositionChanged += OnPositionChanged;

        IsWork = false;
    }

    public Gun Gun { get; private set; }

    public Trunk Trunk { get; private set; }

    public bool IsWork { get; private set; }

    public override void Destroy()
    {
        PositionManipulator.PositionChanged -= OnPositionChanged;

        _road = null;

        StopShooting();
        Gun.Destroy();
        Trunk.Destroy();
        base.Destroy();
    }

    public void SetGun(Gun gun)
    {
        Gun = gun ?? throw new ArgumentNullException(nameof(gun));
        Gun.PositionManipulator.SetForward(PositionManipulator.Forward);
    }

    public void Prepare(Field field, CartrigeBox cartrigeBox, Road road)
    {
        Trunk.SetCartrigeBox(cartrigeBox);

        _field = field ?? throw new ArgumentNullException(nameof(field));
        _road = road ?? throw new ArgumentNullException(nameof(road));

        Vector3 startPoint = _road.GetFirstPoint();
        _currentPoint = 0;

        Mover.SetTarget(startPoint);
        Rotator.SetTarget(startPoint);

        IsWork = true;
    }

    public override void SetFirstPosition(Vector3 position)
    {
        base.SetFirstPosition(position);
        Gun.SetFirstPosition(position);
    }

    public void StartShooting()
    {
        _shootingState.Enter(_field, DetermineTargets(), Gun);
    }

    public void StopShooting()
    {
        _shootingState.Exit();

        IsWork = false;
    }

    private void OnPositionChanged()
    {
        Gun.PositionManipulator.SetForward(PositionManipulator.Forward);
        Trunk.PositionManipulator.SetForward(PositionManipulator.Forward);
    }

    private Queue<Block> DetermineTargets()
    {
        IReadOnlyList<Model> models = _field.GetModelsOfTopLayer(_amountDestroyedRows);
        Queue<Block> blocks = new Queue<Block>();

        foreach (Model model in models)
        {
            if (model is Block block)
            {
                blocks.Enqueue(block);
            }
        }

        return blocks;
    }
}