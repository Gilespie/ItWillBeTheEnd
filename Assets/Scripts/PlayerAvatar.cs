using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private Player _parent;

    void Start()
    {
        _parent = GetComponentInParent<Player>();    
    }

    public void Pushing()
    {
        _parent.Pushing();
    }

    public void Pressing()
    {
        _parent.Pressing();
    }

    public void ActivateControl()
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
    }
}