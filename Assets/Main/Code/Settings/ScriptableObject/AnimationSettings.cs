using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSettings", menuName = "Settings/Animation Settings")]
public class AnimationSettings : ScriptableObject
{
    [SerializeField] private float _animationSpeedForWindows = 2f;

    public float AnimationSpeedForWindows => _animationSpeedForWindows;
}