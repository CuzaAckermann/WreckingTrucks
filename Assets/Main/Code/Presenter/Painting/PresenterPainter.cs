using UnityEngine;

public class PresenterPainter : MonoBehaviour
{
    [SerializeField] private PresenterPaintingSettings _paintingSettings;
    
    public void Paint(Presenter presenter)
    {
        if (_paintingSettings.TryGetMaterial(presenter.Model.Color, out Material material))
        {
            presenter.SetMaterial(material);
        }
    }
}