using UnityEngine;

public class CTRLFadeText : FadeTextParticle
{
    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ChangeState();
        }
    }
}