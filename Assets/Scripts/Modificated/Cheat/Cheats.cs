using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    [SerializeField] DronMovement _drone;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /*if(Input.GetKeyDown(KeyCode.K))
        {
            _drone.gameObject.SetActive(!_drone.gameObject.activeSelf);
        }  */
    }
}