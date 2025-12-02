using UnityEngine;

public class BlockPresenterDestroyer : MonoBehaviour
{
    [SerializeField] private BlockPresenterDetector _detector;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_detector.TryGetPresenter(out BlockPresenter blockPresenter))
            {
                blockPresenter.Model.Destroy();
            }
        }
    }
}