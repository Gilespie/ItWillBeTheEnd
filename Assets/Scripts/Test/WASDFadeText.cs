using UnityEngine;

public class WASDFadeText : FadeTextParticle
{
    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeState();
        }
    }
}