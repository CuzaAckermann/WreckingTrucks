using System;
using UnityEngine;

[Serializable]
public class SpaceSettings<FS> where FS : FieldSettings
{
    [SerializeField] private MoverSettings _moverSettings;

    private FS _fieldSettings;

    public void SetFieldSettings(FS fieldSettings)
    {
        _fieldSettings = fieldSettings ?? throw new ArgumentNullException(nameof(fieldSettings));
    }

    public FS FieldSettings => _fieldSettings;

    public MoverSettings MoverSettings => _moverSettings;
}