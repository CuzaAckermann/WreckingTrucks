using System;
using UnityEngine;

[Serializable]
public class PresenterColorSetting
{
    [SerializeField] private ColorType _color;
    [SerializeField] private Material _material;

    public ColorType Color => _color;

    public Material Material => _material;
}