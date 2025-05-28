using UnityEngine;

public class TruckSelector : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _radiusSphereCast = 0.25f;
    [SerializeField] private float _maxDistance = 25;
    [SerializeField] private TruckPresenter _truckPresenter;

    private void OnEnable()
    {
        _inputHandler.InteractableButtonPressed += OnInteractableButtonPressed;
    }

    private void OnDisable()
    {
        _inputHandler.InteractableButtonPressed -= OnInteractableButtonPressed;
    }

    private void OnInteractableButtonPressed()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.SphereCast(ray.origin, _radiusSphereCast, ray.direction, out RaycastHit hit, _maxDistance))
        {
            if (hit.collider.TryGetComponent(out TruckPresenter truckPresenter))
            {
                _truckPresenter = truckPresenter;
            }
        }
    }
}