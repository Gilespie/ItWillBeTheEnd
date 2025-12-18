using UnityEngine;
using UnityEngine.Rendering;

public abstract class Destructable : MonoBehaviour
{
    [Header("Entity ID")]
    [SerializeField] protected int _id = 0;

    [Header("Health")]
    [SerializeField] protected float _maxHealth = 100f;
    protected float _currentHealth = 0f;
    protected bool _isAlive = true;
    public bool IsAlive => _isAlive;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
       _currentHealth = _maxHealth;
    }

    protected virtual void Update()
    {
        if (!_isAlive) return;
    }

    protected virtual void FixedUpdate()
    {
        if (!_isAlive) return;
    }

    protected virtual void LateUpdate()
    {
        if (!_isAlive) return;
    }

    public virtual void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0f;
            DeactivatePlayer();
        }
    }

    protected virtual void DeactivatePlayer(params object[] parameters)
    {
        _isAlive = false;
        this.enabled = false;
    }
}