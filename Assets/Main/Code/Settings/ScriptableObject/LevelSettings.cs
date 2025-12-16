using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Settings/Level Settings")]
public class LevelSettings : ScriptableObject
{
    [Header("Block Field")]
    [SerializeField] private BlockFieldSettings _blockFieldSettings;

    [Header("Cartridge Box")]
    [SerializeField, Min(1)] private int _amountCartrigeBoxes;

    public BlockFieldSettings BlockFieldSettings => _blockFieldSettings;

    public int AmountCartrigeBoxes => _amountCartrigeBoxes;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _blockFieldSettings?.Validate();
        EditorUtility.SetDirty(this);
    }
#endif
}