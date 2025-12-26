using System;
using UnityEngine;

public abstract class Presenter : Creatable, IPresenter
{
    [SerializeField] private Renderer _renderer;

    public Transform Transform { get; private set; }

    public Model Model { get; private set; }

    public override void Init()
    {
        Transform = transform;
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void OnDestroy()
    {
        ResetState();
    }

    public virtual void Bind(Model model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));

        Subscribe();
    }

    public void SetMaterial(Material material)
    {
        _renderer.sharedMaterial = material ? material : throw new ArgumentNullException(nameof(material));
    }

    public virtual void ChangePosition()
    {
        if (Model != null)
        {
            Model.SetPosition(Transform.position);
            Model.SetDirectionForward(Transform.forward);
        }
    }

    public void Destroy()
    {
        OnDestroy();
    }

    protected virtual void Subscribe()
    {
        SubscribeToModel();
    }

    protected virtual void Unsubscribe()
    {
        UnsubscribeFromModel();
    }

    protected virtual void OnPositionChanged()
    {
        Transform.position = Model.Position;
    }

    protected virtual void OnRotationChanged()
    {
        if (GetType() == typeof(TurretPresenter))
        {
            //Logger.Log(Model.Forward);
        }

        if (Model.Forward != Vector3.zero)
        {
            Transform.forward = Model.Forward;
        }
    }

    protected void SubscribePositionChanged()
    {
        Model.PositionChanged += OnPositionChanged;
    }

    protected void UnsubscribePositionChanged()
    {
        Model.PositionChanged -= OnPositionChanged;
    }

    protected void SubscribeRotationChanged()
    {
        Model.RotationChanged += OnRotationChanged;
    }

    protected void UnsubscribeRotationChanged()
    {
        Model.RotationChanged -= OnRotationChanged;
    }

    protected void SubscribeDestroyedModel()
    {
        Model.DestroyedModel += OnDestroyed;
    }

    protected void UnsubscribeDestroyedModel()
    {
        Model.DestroyedModel -= OnDestroyed;
    }

    private void SubscribeToModel()
    {
        if (Model != null)
        {
            SubscribePositionChanged();
            SubscribeRotationChanged();
            SubscribeDestroyedModel();

            OnPositionChanged();
            OnRotationChanged();
        }
    }

    private void UnsubscribeFromModel()
    {
        if (Model != null)
        {
            UnsubscribePositionChanged();
            UnsubscribeRotationChanged();
            UnsubscribeDestroyedModel();
        }
    }

    private void ResetState()
    {
        Unsubscribe();
        Model = null;
    }

    private void OnDestroyed(Model _)
    {
        ResetState();
        OnLifeTimeFinished();
    }
}