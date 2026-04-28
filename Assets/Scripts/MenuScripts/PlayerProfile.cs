using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;

public class PlayerProfile : MonoBehaviourPunCallbacks, IPunObservable
{
    public string profileName;
    public string elementSelected;

    public void Start()
    {
        updateProfile(profileName, elementSelected);
        //RPCSetProfile(profileName, elementSelected);
    }

    public void Update()
    {
        updateProfile(profileName, elementSelected);
    }


    //[PunRPC]
    //public void RPCSetProfile(string name, string elem)
    //{
    //    profileName = name;
    //    elementSelected = elem;
    //    Debug.Log(elementSelected);
    //    if (photonView.IsMine)
    //    {
    //        photonView.RPC("RPCSetProfile", RpcTarget.OthersBuffered, name, elem);
    //    }
    //}

    public void updateProfile(string name, string elem)
    {
        profileName = name;
        elementSelected = elem;
        this.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = name;
        this.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = elem;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(profileName);
            stream.SendNext(elementSelected);
        }
        else
        {
            profileName = (string)stream.ReceiveNext();
            elementSelected = (string)stream.ReceiveNext();

        }
    }
}
