using UnityEngine;

public class InputDisabler : MonoBehaviour
{
    [SerializeField] CharacterInputController _characterInput;

    public void DisableControl()
    {
        _characterInput.enabled = false;
    }

    public void EnableControl()
    {
        _characterInput.enabled = true;
    }
}