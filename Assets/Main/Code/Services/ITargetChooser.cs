using System.Collections.Generic;
using UnityEngine;

public interface ITargetChooser
{
    public Model SelectTarget(Vector3 pointReference, List<Model> selectedModel);
}