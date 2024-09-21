using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;


public class DataHolder : MonoBehaviour
{
    public static DataHolder instance;
    public HashTable customProperties = new HashTable();
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameObject when loading new scenes.
        }
        else
        {
            Destroy(gameObject); // If another instance exists, destroy this one.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
