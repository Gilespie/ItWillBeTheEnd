using UnityEngine;


public class FallingCeiling : MonoBehaviour
{
    [SerializeField] float _damage = 600f;
    [SerializeField] string _triggerName = "onFall";
    Animator _animator;

    void OnEnable()
    {
        Timer.OnTimeOut += SetTrigger;
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    void OnDisable()
    {
        Timer.OnTimeOut -= SetTrigger;
    }

    void OnTriggerEnter(Collider other)
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