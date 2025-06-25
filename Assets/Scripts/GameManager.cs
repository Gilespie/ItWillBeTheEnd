using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singlton
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }  
    }
    #endregion

    private Player _player;

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private Vector3 _actualCheckpoint;

    public Vector3 ActualCheckpoint
    {
        get { return _actualCheckpoint; }
        set { _actualCheckpoint = value; }
    }

    private Transform[] _aiDogNodes;

    public Transform[] AIDogNodes
    {
        get { return _aiDogNodes; }
        set { _aiDogNodes = value; }
    }

    private NavMeshSurface _surface;

    public NavMeshSurface Surface
    { 
        get { return _surface; }
        set { _surface = value; }
    }

    private CameraFollower _camera;

    public CameraFollower Camera
    {
        get { return _camera; }
        set { _camera = value; }
    }
}