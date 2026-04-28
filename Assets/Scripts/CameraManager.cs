using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Photon.Pun;

public class CameraManager : MonoBehaviourPunCallbacks
{
    public static CameraManager main;
    public Dictionary<string, GameObject> cameraDict = new Dictionary<string, GameObject>();
    public GameObject[] cameras;
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
        cameraDict.Add("GateCam", cameras[7]);
        cameraDict.Add("OldManBridge", cameras[8]);

        for (int i = 1; i < cameras.Length; i++) //exclude player camera, default
        {
            cameras[i].SetActive(false);
        }

        SetPlayerCam(true);
    }

    void Update()
    {
        
    }

    //Network call to activate given camera on all local clients
    public void ActivateCamera(string cameraName)
    {
        //Debug.Log("Activating " + cameraName);
        photonView.RPC("RPCActivateCamera", RpcTarget.All, cameraName);
    }
    //Network call to disable camera after x seconds on all local clients
    public IEnumerator DisableCameraAfterXSeconds(string cameraName, float x)
    {
        yield return new WaitForSeconds(x);
        Debug.Log("Deactivating " + cameraName);
        photonView.RPC("RPCDisableCamera", RpcTarget.All, cameraName);
        
    }
    //Disable Camera locally
    [PunRPC] public void RPCDisableCamera(string cameraName)
    {
        GameObject cam = cameraDict[cameraName];
        cam.SetActive(false);
    }

    //Activate Camera locally
    [PunRPC] public void RPCActivateCamera(string cameraName)
    {
        //TODO disable movement

        GameObject cam = cameraDict[cameraName];
        cam.SetActive(true);
        
    }

    public void SetPlayerCam(bool val)
    {
        cameras[0].SetActive(val);
    }
}
