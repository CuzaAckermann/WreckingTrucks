using System;
using UnityEngine;

public abstract class Presenter : MonoBehaviour, IPresenter, IDestroyable
{
    [SerializeField] private Renderer _renderer;

    public event Action<IDestroyable> Destroyed;

    public Transform Transform { get; private set; }

    public Model Model { get; private set; }

    public Type BoundModelType => Model.GetType();

    public virtual void Init()
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
        OnDestroyed();
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
            Model.Placeable.SetPosition(Transform.position);
            Model.Placeable.SetForward(Transform.forward);
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
        Transform.position = Model.Placeable.Position;
    }

    protected virtual void OnRotationChanged()
    {
        if (GetType() == typeof(TurretPresenter))
        {
            //Logger.Log(Model.Forward);
        }

        if (Model.Placeable.Forward != Vector3.zero)
        {
            Transform.forward = Model.Placeable.Forward;
        }
    }

    protected void SubscribePositionChanged()
    {
        Model.Placeable.PositionChanged += OnPositionChanged;
    }

    protected void UnsubscribePositionChanged()
    {
        Model.Placeable.PositionChanged -= OnPositionChanged;
    }

    protected void SubscribeRotationChanged()
    {
        Model.Placeable.RotationChanged += OnRotationChanged;
    }

    protected void UnsubscribeRotationChanged()
    {
        Model.Placeable.RotationChanged -= OnRotationChanged;
    }

    protected void SubscribeDestroyedModel()
    {
        Model.Destroyed += OnDestroyed;
    }

    protected void UnsubscribeDestroyedModel()
    {
        Model.Destroyed -= OnDestroyed;
    }

    protected virtual void ResetState()
    {
        Unsubscribe();

        Model = null;
    }

    private void OnDestroyed()
    {
        ResetState();

        Destroyed?.Invoke(this);
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

    private void OnDestroyed(IDestroyable _)
    {
        OnDestroyed();
    }
}