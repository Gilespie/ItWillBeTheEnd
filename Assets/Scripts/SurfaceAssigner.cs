using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class SurfaceAssigner : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.Surface = GetComponent<NavMeshSurface>();        
    }
}