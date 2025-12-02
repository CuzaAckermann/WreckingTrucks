using UnityEngine;

[CreateAssetMenu(fileName = "NonstopGameSettings", menuName = "Settings/Nonstop Game Settings")]
public class NonstopGameSettings : ScriptableObject
{
    [Header("Block Field")]
    [SerializeField] private NonstopGameBlockFieldSettings _blockFieldSettings;

    [Header("Truck Field")]
    [SerializeField] private TruckFieldSettings _truckFieldSettings;

    [Header("Cartridge Box")]
    [SerializeField] private CartrigeBoxFieldSettings _cartrigeBoxSettings;

    public NonstopGameBlockFieldSettings BlockFieldSettings => _blockFieldSettings;

    public TruckFieldSettings TruckFieldSettings => _truckFieldSettings;

    public CartrigeBoxFieldSettings CartrigeBoxSettings => _cartrigeBoxSettings;
}