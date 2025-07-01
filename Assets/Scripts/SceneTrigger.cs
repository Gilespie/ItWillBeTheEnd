using UnityEngine;

public class SceneTrigger : Trigger
{
    [SerializeField] private string _levelName = "";

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        LoadSceneManager.Instance.LoadScene(_levelName);
    }
}