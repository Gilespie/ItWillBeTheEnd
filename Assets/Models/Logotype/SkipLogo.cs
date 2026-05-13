using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SkipLogo : MonoBehaviour
{
    [SerializeField] private Input m_Input;
    [SerializeField] private string m_Level;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 15f || Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(m_Level);
        }
    }
}
