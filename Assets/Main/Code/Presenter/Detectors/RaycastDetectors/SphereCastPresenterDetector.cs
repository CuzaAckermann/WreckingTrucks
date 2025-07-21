using UnityEngine;

public abstract class SphereCastPresenterDetector<T> : MonoBehaviour, IPresenterDetector<T> where T : Presenter
{
    [Header("Detection Settings")]
    [SerializeField] private float _radiusSphereCast = 0.1f;
    [SerializeField] private float _maxDistance = 50;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.Ignore;

    [Header("Camera Reference")]
    [SerializeField] private Camera _camera;

    public bool TryGetPresenter(out T presenter)
    {
        presenter = null;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.SphereCast(ray.origin,
                               _radiusSphereCast,
                               ray.direction,
                               out RaycastHit hit,
                               _maxDistance,
                               _layerMask,
                               _triggerInteraction))
        {
            presenter = hit.collider.GetComponent<T>();

            if (presenter == null)
            {
                presenter = hit.collider.GetComponentInParent<T>();
            }
        }

        return presenter != null;
    }
}