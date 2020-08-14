using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(ARRaycastManager))]
public class GameGroundSpawn : MonoBehaviour
{
    public GameObject goPrefab;
    public GameObject goPreview;

    public TextMeshProUGUI pressToPlay;
    
    public Camera camera;
    public Material previewMaterial;

    private ARRaycastManager arRaycastManager;
    private ARPointCloudManager arPointCloudManager;
    private Vector2 touchPosition;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPointCloudManager = GetComponent<ARPointCloudManager>();



    }

    void Update()
    {
        PreviewGround();
    }

    private void PlaceGround(Vector3 pos, Quaternion rot)
    {
        GameObject spawnedGround = Instantiate(goPrefab, pos, rot);
        pressToPlay.gameObject.SetActive(false);
        spawnedGround.GetComponent<GameGroundScript>().setText(pressToPlay);
    }

    private void PreviewGround()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        if (arRaycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            
            var hitPose = hits[0].pose;
            
            if (!goPreview.scene.IsValid())
            {
                goPreview = Instantiate(goPreview, hitPose.position, Quaternion.LookRotation(new Vector3(ray.direction.x,0 , ray.direction.z)));
                goPreview.GetComponent<MeshRenderer>().material = previewMaterial;
                pressToPlay.text = "Press to play";
            }
            else
            {
                goPreview.transform.rotation = Quaternion.LookRotation(new Vector3(ray.direction.x,0 , ray.direction.z));
                goPreview.transform.position = hitPose.position;
            }

            if (Input.touchCount > 0 && goPreview.activeSelf)
            {
                PlaceGround(goPreview.transform.position, Quaternion.LookRotation(new Vector3(ray.direction.x,0 , ray.direction.z)));
                arPointCloudManager.enabled = false;
                goPreview.SetActive(false);
            }
        }
    }
}