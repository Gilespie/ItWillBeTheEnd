using UnityEngine;


public class FallingRock : MonoBehaviour
{
    [SerializeField] private float _damage = 600f;
    [SerializeField] private string _triggerName = "onFall";
    private Animator _animator;

    private void OnEnable()
    {
        Timer.OnTimeOut += SetTrigger;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    private void OnDisable()
    {
        Timer.OnTimeOut -= SetTrigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(_damage);
        }
    }

    public void SetTrigger()
    {
        _animator.SetTrigger(_triggerName);
    }
}