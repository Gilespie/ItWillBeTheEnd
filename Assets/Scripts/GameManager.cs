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

    private Transform[] _aiNodes;

    public Transform[] AINodes
    {
        get { return _aiNodes; }
        set { _aiNodes = value; }
    }

}
