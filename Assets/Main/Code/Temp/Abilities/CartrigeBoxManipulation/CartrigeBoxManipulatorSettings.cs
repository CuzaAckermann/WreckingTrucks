using UnityEngine;

public class CartrigeBoxManipulatorSettings : MonoBehaviour
{
    [SerializeField, Range(0.1f, 2)] private float _timeForTaking = 1;
    [SerializeField, Range(0.1f, 2)] private float _timeForAdd = 1;

    [SerializeField, Range(1, 100)] private int _amountForTaking = 1;
    [SerializeField, Range(1, 100)] private int _amountForAdd = 1;

    public float TimeForTaking => _timeForTaking;

    public float TimeForAdd => _timeForAdd;

    public int AmountForTaking => _amountForTaking;

    public int AmountForAdd => _amountForAdd;
}