using UnityEngine;

public class SPACEFadeText : FadeTextParticle
{
    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState();
        }
    }
}