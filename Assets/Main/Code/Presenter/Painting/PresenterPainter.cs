using System;
using UnityEngine;

public class PresenterPainter
{
    private readonly PresenterPaintingSettings _paintingSettings;
    
    public PresenterPainter(PresenterPaintingSettings paintingSettings)
    {
        Validator.ValidateNotNull(paintingSettings);

        _paintingSettings = paintingSettings;
    }

    public void Paint(Presenter presenter)
    {
        Validator.ValidateNotNull(presenter);
        Validator.ValidateNotNull(presenter.Model);

        //if (presenter.Model.TryGetTrait<Colorable>() == false)
        //{
        //    return;
        //}

        if (_paintingSettings.TryGetMaterial(presenter.Model.Color, out Material material) == false)
        {
            throw new InvalidOperationException("No material for this color");
        }

        presenter.SetMaterial(material);
    }
}