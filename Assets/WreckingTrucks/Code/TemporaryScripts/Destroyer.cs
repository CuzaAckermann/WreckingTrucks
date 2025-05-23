using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            TryDestroyBlock();
        }
    }

    private void TryDestroyBlock()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.TryGetComponent(out BlockPresenter blockPresenter))
                {
                    blockPresenter.Destroy();
                }
            }
        }
    }
}