using System;
using System.Collections.Generic;
using UnityEngine;

public class BarrelPresenter : Presenter
{
    private Barrel _barrel;

    public override void Bind(Model model)
    {
        if (model is Barrel barrel)
        {
            _barrel = barrel;

            _barrel.SetPosition(Transform.position);
            _barrel.SetDirectionForward(Transform.forward);
            _barrel.SetRight(Transform.right);
        }

        base.Bind(model);
    }

    public override void ChangePosition()
    {
        if (Model != null)
        {
            Model.SetPosition(Transform.position);
            Model.SetDirectionForward(Transform.forward);
            _barrel.SetRight(Transform.right);
        }
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