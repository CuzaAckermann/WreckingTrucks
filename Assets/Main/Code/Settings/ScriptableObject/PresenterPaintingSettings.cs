using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PaintingSettings", menuName = "Settings/Painting Settings")]
public class PresenterPaintingSettings : ScriptableObject
{
    [SerializeField] private List<PresenterColorSetting> _colorSettings;

    public bool TryGetMaterial(ColorType color, out Material material)
    {
        material = null;

        if (TryGetIndex(color, out int index))
        {
            material = _colorSettings[index].Material;
            return true;
        }

        return false;
    }

    private bool TryGetIndex(ColorType color, out int index)
    {
        index = -1;

        for (int i = 0; i < _colorSettings.Count; i++)
        {
            if (_colorSettings[i].Color == color)
            {
                index = i;
                return true;
            }
        }

        return false;
    }
}