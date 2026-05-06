using UnityEngine;

// Ваша структура из прошлого проекта
[System.Serializable]
public class CinematicAction
{
    public string actionName = "New Action";

    [Header("1. Входная анимация")]
    public string introState;

    [Header("2. Зацикленная часть")]
    public string loopState;
    public int minLoops = 2;
    public int maxLoops = 5;

    [Header("3. Выходная анимация")]
    public string outroState;
}

public class PointOfInterest : MonoBehaviour
{
    [Header("Настройки точки")]
    public float waitTimeMin = 10f;
    public float waitTimeMax = 30f;
    
    [Header("Сложные составные анимации")]
    public CinematicAction[] advancedActions; // Теперь тут мощные настройки!
}