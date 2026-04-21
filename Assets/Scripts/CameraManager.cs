using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public static CameraManager main;
    public Dictionary<string, GameObject> cameraDict = new Dictionary<string, GameObject>();
    public GameObject[] cameras;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        main = this;

        cameraDict.Add("Player", cameras[0]);
        cameraDict.Add("Bridge", cameras[1]);
        cameraDict.Add("WindmillTangled", cameras[2]);
        cameraDict.Add("WindmillBroken", cameras[3]);
        cameraDict.Add("WindmillWater", cameras[4]);
        cameraDict.Add("OldMan", cameras[5]);
        cameraDict.Add("AllWindmills", cameras[6]);

        for (int i = 1; i < cameras.Length; i++) //exclude player camera, default
        {
            cameras[i].SetActive(false);
        }

        SetPlayerCam(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateCamera(string cameraName)
    {
        //TODO disable movement

        GameObject cam = cameraDict[cameraName];
        cam.SetActive(true);
        SetPlayerCam(false); //turn player camera off
        
    }

    public IEnumerator DisableCameraAfterXSeconds(string cameraName, float x, string endCameraName)
    {
        GameObject cam = cameraDict[cameraName];
        GameObject endCam = cameraDict[endCameraName];
        yield return new WaitForSeconds(x); 
        cam.SetActive(false);
        endCam.SetActive(true);
    }

    public void SetPlayerCam(bool val)
    {
        cameras[0].SetActive(val);
    }
}
