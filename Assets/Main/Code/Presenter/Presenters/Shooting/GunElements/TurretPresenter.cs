using System;
using System.Collections.Generic;
using UnityEngine;

public class TurretPresenter : Presenter
{
    private Turret _turret;

    public override void Bind(Model model)
    {
        if (model is Turret turret)
        {
            _turret = turret;

            _turret.PositionManipulator.SetPosition(Transform.position);
            _turret.PositionManipulator.SetForward(Transform.forward);
        }

        base.Bind(model);
    }

    protected override void Subscribe()
    {
        if (Model != null)
        {
            SubscribeRotationChanged();
            SubscribeDestroyedModel();
        }
    }

    protected override void Unsubscribe()
    {
        if (Model != null)
        {
            UnsubscribeRotationChanged();
            UnsubscribeDestroyedModel();
        }
    }
}