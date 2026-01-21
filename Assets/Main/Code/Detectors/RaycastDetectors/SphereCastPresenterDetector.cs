using UnityEngine;

public class SphereCastPresenterDetector : MonoBehaviour, IPresenterDetector<Presenter>
{
    [Header("Detection Settings")]
    [SerializeField] private float _radiusSphereCast = 0.1f;
    [SerializeField] private float _maxDistance = 50;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.Ignore;

    [Header("Camera Reference")]
    [SerializeField] private Camera _camera;

    public bool TryGetPresenter(out Presenter presenter)
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
            Logger.Log(hit.collider.gameObject.name);

            presenter = hit.collider.GetComponent<Presenter>();

            if (presenter == null)
            {
                presenter = hit.collider.GetComponentInParent<Presenter>();
            }

            if (presenter == null)
            {
                presenter = hit.collider.GetComponentInChildren<Presenter>();
            }
        }
        else
        {
            Logger.Log("Don't detect");
        }

        return presenter != null;
    }
}