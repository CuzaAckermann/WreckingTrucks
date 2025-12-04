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

    public void ChangePosition()
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
        if (Model.Forward != Vector3.zero)
        {
            Transform.forward = Model.Forward;
        }
    }

    private void SubscribeToModel()
    {
        if (Model != null)
        {
            Model.PositionChanged += OnPositionChanged;
            Model.RotationChanged += OnRotationChanged;
            Model.DestroyedModel += OnDestroyed;
            UpdateTransform();
        }
    }

    private void UnsubscribeFromModel()
    {
        if (Model != null)
        {
            Model.PositionChanged -= OnPositionChanged;
            Model.RotationChanged -= OnRotationChanged;
            Model.DestroyedModel -= OnDestroyed;
        }
    }

    private void UpdateTransform()
    {
        OnPositionChanged();
        OnRotationChanged();
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