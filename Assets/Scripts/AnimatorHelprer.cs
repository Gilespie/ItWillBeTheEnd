using UnityEngine;

public class AnimatorHelprer : MonoBehaviour
{
    [SerializeField] private Door _parent;

    public void PlaySound()
    {
        _parent.PlaySFX();
    }
}