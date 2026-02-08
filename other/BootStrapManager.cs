using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrapManager : MonoBehaviour
{
    public GameObject controlsObject;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(controlsObject);
        
        MoveOn();
    }

    void MoveOn()
    {
        SceneManager.LoadScene(1);
    }
}
