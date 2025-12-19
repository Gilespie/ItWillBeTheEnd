using UnityEngine;

public class KillObject : MonoBehaviour
{
    [SerializeField] private float speedThreshold = 2f; // Порог скорости
    [SerializeField] private float damageAmount = 20f; // Урон

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float currentSpeed = rb.velocity.magnitude;

        if (currentSpeed > speedThreshold)
        {
            Destructable destruct = collision.gameObject.GetComponent<Destructable>();

            if (destruct != null)
            {
                destruct.InstantKill();
                Debug.Log($"Камень ударил игрока со скоростью {currentSpeed}, нанесено {damageAmount} урона!");
            }

        }

    }
}