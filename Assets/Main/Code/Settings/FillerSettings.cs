using System;
using UnityEngine;

[Serializable]
public class FillerSettings
{
    [Header("Strategies")]
    [SerializeField] private StrategyFillingSettings _rowFillerSettings;
    [SerializeField] private StrategyFillingSettings _cascadeFillerSettings;

    public StrategyFillingSettings RowFillerSettings => _rowFillerSettings;

    public StrategyFillingSettings CascadeFillerSettings => _cascadeFillerSettings;
}