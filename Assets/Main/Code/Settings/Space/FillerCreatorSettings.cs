using System;
using UnityEngine;

[Serializable]
public class FillerCreatorSettings
{
    [Header("Row Filler")]
    [SerializeField] private bool _useRowFiller;
    [SerializeField] private FillerSettings _rowFillerSettings;

    [Header("Cascade Filler")]
    [SerializeField] private bool _useCascadeFiller;
    [SerializeField] private FillerSettings _cascadeFillerSettings;

    public bool UseRowFiller => _useRowFiller;

    public bool UseCascadeFiller => _useCascadeFiller;

    public FillerSettings RowFillerSettings => _rowFillerSettings;

    public FillerSettings CascadeFillerSettings => _cascadeFillerSettings;
}