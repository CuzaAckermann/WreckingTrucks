using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchedApplicationStateIndicator : MonoBehaviour
{
    private ISwitchedApplicationState _switchedApplicationState;

    public void Init(ISwitchedApplicationState switchedApplicationState)
    {
        Validator.ValidateNotNull(switchedApplicationState);

        _switchedApplicationState = switchedApplicationState;
    }
}
