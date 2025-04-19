using UnityEngine;

public abstract class Destructable : MonoBehaviour
{
    [Header("Entity ID")]
    [SerializeField] protected int _id = 0;

    [Header("Health")]
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth = 0f;
    private bool _isAlive = true;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}