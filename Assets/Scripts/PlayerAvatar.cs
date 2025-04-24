using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private Player _parent;

    void Start()
    {
        _parent = GetComponentInParent<Player>();    
    }

    public void Interact()
    {
        _parent.Pushing();
    }
}