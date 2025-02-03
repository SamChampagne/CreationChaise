using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class SpawnableManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_Raycast_manager;

    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spawnablePrefab;

    Camera arCam;
    GameObject spawnObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnObject = null;
        GameObject camObject = GameObject.FindGameObjectWithTag("MainCamera");
        arCam = camObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 0)
        {
            return;
        }
        // Créer un rayon depuis la caméra principale vers l'écran tactile
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;

        if (m_Raycast_manager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began && spawnObject == null)
            {
                if(Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable")
                    {
                        spawnObject = hit.collider.gameObject;
                    }
                    else
                    {
                        spawnPrefab(m_Hits[0].pose.position);
                    }
                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved && spawnObject == null)
            {
                spawnObject.transform.position = m_Hits[0].pose.position;
            }
            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnObject = null;
            }
        }
    }
    private void spawnPrefab(Vector3 spawnPosition)
    {
        spawnObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
    }
}
