using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldEFadeText : FadeTextParticle
{
    protected override void Update()
    {
        if (!_isPlayerInside || _isUsed || _collider.enabled == false) return;

        if (Input.GetKey(KeyCode.E))
        {
            _isUsed = true;
            ChangeState();
        }
    }
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
