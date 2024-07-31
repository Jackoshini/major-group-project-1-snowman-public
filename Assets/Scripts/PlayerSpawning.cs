using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{
    [SerializeField] private GameObject worldMap;
    private bool spawned;

    

    // Start is called before the first frame update
    void Start()
    {
        spawned = false;
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnPlayer();
    }

    private void spawnPlayer()
    {
        if (!spawned)
        {
            Vector3 spawnLocation = worldMap.GetComponent<WorldGeography>().spawnLocation;
            gameObject.transform.position = spawnLocation;
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            spawned = true;
        }
    }
}
