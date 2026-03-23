using UnityEngine;

public class Drone : MonoBehaviour, IDamageable
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            damageable.InstantKill();
            InstantKill();
        }
    }


    public void InstantKill(params object[] parameters)
    {
        Destroy(gameObject);
    }
}
