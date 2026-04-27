using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerProfile : MonoBehaviourPunCallbacks
{
    public string profileName;
    public string elementSelected;

    public void Update()
    {
        RPCSetProfileName(profileName);
        RPCSetProfileElement(elementSelected);
    }


    [PunRPC]
    public void RPCSetProfileName(string name)
    {
        this.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = name;
        profileName = name;

        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetProfileName", RpcTarget.OthersBuffered, name);
        }
    }

    [PunRPC]
    public void RPCSetProfileElement(string elem)
    {
        this.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = elem;
        elementSelected = elem;

        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetProfileElement", RpcTarget.OthersBuffered, elem);
        }
    }

}
