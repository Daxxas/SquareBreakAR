using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;

public class GameGroundScript : MonoBehaviour
{

    public GameObject breakableCube;

    public TextMeshProUGUI pressToPlay;
    
    public TextMeshPro scoreText;
    public TextMeshPro timerText;
    
    [SerializeField] private float yRange = 0.1f;
    [SerializeField] private float yCubeSpawnOffset = 0.02f;
    [SerializeField] private float timer = 60;
    
    private int score;
    
    private GameObject spawnedCube;
    private float nextSpawn = 0;

    private Collider collider;
    private Corner corner;
    
    void Start()
    {
        collider = GetComponent<Collider>();
        
        corner.bottomLeft = new Vector3(collider.bounds.min.x, transform.position.y, collider.bounds.min.z);
        corner.topLeft = new Vector3(collider.bounds.min.x, transform.position.y, collider.bounds.max.z);
        corner.topRight = new Vector3(collider.bounds.max.x, transform.position.y, collider.bounds.max.z);
        corner.bottomRight = new Vector3(collider.bounds.max.x, transform.position.y, collider.bounds.min.z);

    }

    void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer >= 0)
        {
            timerText.text = "Time : " + Mathf.RoundToInt(timer);
            SpawnCubes();
        }
        else
        {
            pressToPlay.gameObject.SetActive(true);
            pressToPlay.text = "Press to play again \n Score : " + score;

            if (spawnedCube)
            {
                if (spawnedCube.scene.IsValid())
                {
                    Destroy(spawnedCube);
                }
            }
            
            if (Input.touchCount > 0)
            {
                score = 0;
                timer = 60;
                scoreText.text = "Score : \n" + score;
                timerText.text = "Time : \n" + timer;
                pressToPlay.gameObject.SetActive(false);
            }
        }
        
    }

    private void SpawnCubes()
    {
        if (!spawnedCube)
        {
            float randx = Random.Range(corner.bottomLeft.x, corner.bottomRight.x);
            float randy = yCubeSpawnOffset + transform.position.y + Random.Range(breakableCube.GetComponent<Collider>().bounds.size.y, yRange);
            float randz = Random.Range(corner.bottomLeft.z, corner.topLeft.z);

            if (!Physics.CheckSphere(new Vector3(randx, randy, randz), 0.05f))
            {
                spawnedCube = Instantiate(breakableCube,new Vector3(randx,randy, randz), transform.rotation);
                spawnedCube.GetComponent<BreakableCubeBehaviour>().SetGameGroundScript(this);
            }
            else
            {
                SpawnCubes();
            }
            
        }
    }

    public void setText(TextMeshProUGUI play)
    {
        pressToPlay = play;
    }
    
    public void addScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : \n" + score;
    }
    
    private struct Corner
    {
        public Vector3 topLeft, bottomLeft;
        public Vector3 topRight, bottomRight;
    }
    
}
