using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpDash : MonoBehaviour
{
    public void StartHuinya(DashState state)
    {
        StartCoroutine(state.StartDash(state.direction));
    }

}
