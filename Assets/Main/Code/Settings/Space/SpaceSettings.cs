using System;
using UnityEngine;

[Serializable]
public class SpaceSettings<FS> where FS : FieldSettings
{
    [SerializeField] private MoverSettings _moverSettings;

    private FS _fieldSettings;

    public void SetFieldSettings(Transform transform, FS fieldSettings)
    {
        _fieldSettings = fieldSettings ?? throw new ArgumentNullException(nameof(fieldSettings));
        _fieldSettings.SetTransform(transform);
    }

    public FS FieldSettings => _fieldSettings;

    public MoverSettings MoverSettings => _moverSettings;
}