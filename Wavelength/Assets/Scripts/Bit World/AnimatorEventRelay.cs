using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventRelay : MonoBehaviour
{
    DirectionChoice dc;

    private void Start()
    {
        dc = FindObjectOfType<DirectionChoice>();
    }

    public void ResetUpValue()
    {
        dc.ResetParameter(Direction.up);
    }
}
