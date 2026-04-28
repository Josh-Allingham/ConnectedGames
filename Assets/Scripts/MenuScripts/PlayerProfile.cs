using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviourPunCallbacks, IPunObservable
{
    public string profileName;
    public string elementSelected;
    public bool readyUp = false;

    [Header("Element Icon Sprites")]
    public Sprite waterIcon;
    public Sprite fireIcon;
    public Sprite earthIcon;
    public Sprite windIcon;

    public int elemNumber;


    public void Start()
    {
        updateProfile(profileName, elementSelected);
    }

    public void Update()
    {
        updateProfile(profileName, elementSelected);
       
        WorldToBattleTransfer.playerName = profileName;
        WorldToBattleTransfer.element = elemNumber;
    }

    public void updateProfile(string name, string elem)
    {
        profileName = name;
        elementSelected = elem;
        this.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = name;
        this.transform.GetChild(0).GetChild(2).GetComponentInChildren<TMP_Text>().text = elem;
        
        if(readyUp)
        {
            transform.GetChild(2).GetComponent<Image>().enabled = false;
            transform.GetChild(3).GetComponent<Image>().enabled = true;
        }
        else
        {
            transform.GetChild(2).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetComponent<Image>().enabled = false;
        }

        switch (elem)
        {
            case "Water":
                transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = waterIcon;
                elemNumber = 0;
                break;
            case "Fire":
                transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = fireIcon;
                elemNumber = 1;
                break;
            case "Earth":
                transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = earthIcon;
                elemNumber = 2;
                break;
            case "Wind":
                transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = windIcon;
                elemNumber = 3;
                break;
            }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(profileName);
            stream.SendNext(elementSelected);
            stream.SendNext(readyUp);
        }
        else
        {
            profileName = (string)stream.ReceiveNext();
            elementSelected = (string)stream.ReceiveNext();
            readyUp = (bool)stream.ReceiveNext();

        }
    }
}
