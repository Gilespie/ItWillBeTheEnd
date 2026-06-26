using UnityEngine;

public class SkipLogo : MonoBehaviour
{
    [SerializeField] KeyCode _skipLogoKey;
    [SerializeField] string _levelName;
    bool _isSkipped = false;

    void Update()
    {
        if(_isSkipped) return;

        if(Input.GetKeyDown(_skipLogoKey))
        {
            EventManager.Trigger(EventType.OnSceneTransition, _levelName);
            _isSkipped = true;
        }
    }
}