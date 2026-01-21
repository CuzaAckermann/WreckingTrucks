using UnityEngine;

public class LevelInformerInitializer : MonoBehaviour
{
    [Header("Informers")]
    [SerializeField] private BlockFieldInformer _blockFieldInformer;
    [SerializeField] private CartrigeBoxFieldInformer _cartrigeBoxFieldInformer;
    [SerializeField] private RoadInformer _roadInformer;
    [SerializeField] private PlaneSlotInformer _planeSlotInformer;

    private Transform _transform;

    public void Init(EventBus eventBus)
    {
        _transform = transform;

        _blockFieldInformer.Init(eventBus, _transform.position.y);
        _cartrigeBoxFieldInformer.Init(eventBus, _transform.position.y);
        _roadInformer.Init(eventBus, _transform.position.y);
        _planeSlotInformer.Init(eventBus, _transform.position.y);
    }

    public BlockFieldInformer BlockFieldInformer => _blockFieldInformer;
}