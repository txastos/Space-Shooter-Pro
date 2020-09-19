﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 19.0f;
    [SerializeField] private GameObject _explosion;
    private SpawnManager _spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _rotateSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosion, this.transform.position , Quaternion.identity );
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
             Destroy(this.gameObject,0.25f);
            
            
        }
    }
}
