using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BreakableCubeBehaviour : MonoBehaviour
{

    private GameGroundScript gameGroundScript;

    public GameObject breakParticle;
    
    private Collider collider;
    [SerializeField] private LayerMask cameraLayer;
    
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( cameraLayer == (cameraLayer | (1 << other.gameObject.layer)))
        {
            gameGroundScript.addScore(1);
            Instantiate(breakParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void SetGameGroundScript(GameGroundScript input)
    {
        gameGroundScript = input;
    }
}
