using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Settings/Level Settings")]
public class LevelSettings : ScriptableObject
{
    [Header("Block Field")]
    [SerializeField] private BlockFieldSettings _blockFieldSettings;

    //[Header("Truck Field")]
    //[SerializeField] private TruckFieldSettings _truckFieldSettings;

    [Header("Cartridge Box")]
    [SerializeField, Min(1)] private int _amountCartrigeBoxes;
    //[SerializeField] private CartrigeBoxFieldSettings _cartrigeBoxSettings;

    public BlockFieldSettings BlockFieldSettings => _blockFieldSettings;

    //public TruckFieldSettings TruckFieldSettings => _truckFieldSettings;

    public int AmountCartrigeBoxes => _amountCartrigeBoxes;

    //public CartrigeBoxFieldSettings CartrigeBoxSettings => _cartrigeBoxSettings;


#if UNITY_EDITOR
    private void OnValidate()
    {
        _blockFieldSettings?.Validate();
        EditorUtility.SetDirty(this);
    }
#endif
}