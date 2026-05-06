using UnityEngine;

public class PavilionDirector : MonoBehaviour
{
    [Header("Управление павильоном")]
    public GameObject pavilionContent; // Пустой объект, внутри которого лежат все NPC и реквизит
    
    [Header("Актеры")]
    public NPCBrain npcBob;
    public NPCBrain npcAlice;

    [Header("Точки (POI)")]
    public PointOfInterest fridgePOI;
    public PointOfInterest bedPOI;

    private void Start()
    {
        // Павильон спит, пока игрок не войдет. Экономим ресурсы!
        pavilionContent.SetActive(false); 
    }

    // 1. Игрок спрыгнул к нам (Вход в павильон)
    public void OnPlayerEnteredPavilion()
    {
        pavilionContent.SetActive(true);
        
        // Раздаем стартовые приказы
        npcBob.Command_GoToPoint(bedPOI); 
    }

    // 2. Игрок подошел к Телевизору (Реакция на действия)
    public void OnPlayerTouchedTV()
    {
        // Режиссер командует Бобу сменить задачу
        npcBob.Command_GoToPoint(fridgePOI);
    }

    // 3. Игрок спрыгнул дальше (Точка невозврата)
    public void OnPlayerLeftPavilionForever()
    {
        // Удаляем всех актеров, реквизит и самого режиссера из памяти!
        Destroy(pavilionContent);
        Destroy(gameObject);
    }
}