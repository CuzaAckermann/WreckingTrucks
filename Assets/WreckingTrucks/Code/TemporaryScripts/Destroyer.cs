using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private bool _isOneClick = true;

    private void Update()
    {
        if (_isOneClick)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                TryDestroyBlock();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                TryDestroyBlock();
            }
        }
    }

    private void TryDestroyBlock()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.TryGetComponent(out Presenter blockPresenter))
                {
                    blockPresenter.Destroy();
                }
            }
        }
    }
}