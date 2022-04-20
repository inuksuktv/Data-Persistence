using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    public static DataContainer Instance;
    public string input;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadStringInput(string s)
    {
        input = s;
        Debug.Log(input);
    }
}
