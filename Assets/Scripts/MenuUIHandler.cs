using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Sets the script to be executed later than all default scripts
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public static MenuUIHandler Instance;

    void Awake()
    {

    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }
}
