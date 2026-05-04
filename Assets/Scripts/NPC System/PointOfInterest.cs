using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [Header("Настройки точки")]
    public float waitTimeMin = 5f;
    public float waitTimeMax = 15f;
    
    [Header("Доступные анимации здесь")]
    public string[] pointAnimations;
    
    // Сюда можно добавить переменные для пропсов (кружка кофе, телефон), 
    // чтобы NPC брал их в руки, приходя сюда.
}