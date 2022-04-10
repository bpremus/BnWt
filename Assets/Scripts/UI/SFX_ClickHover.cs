using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_ClickHover : MonoBehaviour
{

    public void onClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
    }
}
