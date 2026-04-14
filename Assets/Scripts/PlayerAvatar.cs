using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAvatar : MonoBehaviour
{
    private Character _parent;

    void Start()
    {
        _parent = GetComponentInParent<Character>();    
    }

    /*public void Pushing()
    {
        _parent.Pushing();
    }*/

    public void Pressing()
    {
        _parent.Pressing();
    }

    /*public void ActivateControl()
    {
        _parent.ActivateControl();
    }

    public void DeactivateControl()
    {
        _parent.DeactivateControl();
    }

    public void PlayFootStep()
    {
        _parent.PlayFootStep();
    }*/

    public void ActivateKinematic()
    {
        _parent.ActivateRBKinematic();
    }

    public void DeactiveKinematic()
    {
        _parent.DeactivateRBKinematic();
    }

    public void TeleportParent()
    {
        _parent.TeleportParent();
    }

    public void ResetClimbing()
    {
        _parent.ResetClimbing();
    }
}