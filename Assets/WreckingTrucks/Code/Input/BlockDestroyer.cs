using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _radiusSphereCast = 0.1f;
    [SerializeField] private float _maxDistance = 50;

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
            if (hit.collider.TryGetComponent(out BlockPresenter blockPresenter))
            {
                blockPresenter.Destroy();
            }
        }
    }
}