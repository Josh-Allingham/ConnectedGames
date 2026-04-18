using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviourPunCallbacks  
{
    public Element myElement;

    public string myElementType;
    public int numOfCPU;
    public List<Element> cpuElement = new List<Element>();
    
    public string attackName;
    public string attackType;
    public string attackPower;
    public string attackAccuracy;
    public string attackTarget;
    public string attackDescription;

    public string castName;
    public string castType;
    public string castPower;
    public string castAccuracy;
    public string castTarget;
    public string castDescription;
    public string castTurnLimit;

    public string turnAction;
    public string turnTarget;

    public float fireHealth;
    public float fireStateri;

    public float waterHealth;
    public float waterStateri;

    public float earthHealth;
    public float earthStateri;

    public float windHealth;
    public float windStateri;


    public void Update()
    {
        if (this.transform.childCount>0)
        {
            myElement = GetComponentInChildren<BattlePlayer>().GetComponentInChildren<Element>();
        }

        if((PhotonNetwork.IsMasterClient && this.transform.childCount > 1))
        { 
            numOfCPU = this.transform.childCount - 1;
            for (int i = 0; i < numOfCPU; i++)
            {
                Element cpu = this.transform.GetChild(i + 1).GetComponentInChildren<BattleCPU>().GetComponentInChildren<Element>();
                cpuElement.Add(cpu);
            }

            foreach(Element element in cpuElement)
            {
                getMyPlayerStats(element);
                announceStats(element.ElementType, element.currHealth, element.currStatera);
            }
        }

        if (myElement != null)
        {
            getMyPlayerStats(myElement);
            announceStats(myElementType, myElement.currHealth, myElement.currStatera);
        }
    }

    public void getMyPlayerStats(Element element)
    {
        switch (element.ElementType)
        {
            case "Fire":
                fireHealth = element.maxHealth;
                fireStateri = element.ElementStatera;
                myElementType = element.ElementType;
                break;
            case "Water":
                waterHealth = element.maxHealth;
                waterStateri = element.ElementStatera;
                myElementType = element.ElementType;
                break;
            case "Earth":
                earthHealth = element.maxHealth;
                earthStateri = element.ElementStatera;
                myElementType = element.ElementType;
                break;
            case "Wind":
                windHealth = element.maxHealth;
                windStateri = element.ElementStatera;
                myElementType = element.ElementType;
                break;
        }
        getMoves(element.MyMoves);
    }

    public void getMoves(string[,] moves)
    {
        attackType = moves[0, 0];
        attackName = moves[0, 1];
        attackPower = moves[0, 2];
        attackAccuracy = moves[0, 3];
        attackTarget = moves[0, 4];
        attackDescription = moves[0, 5];

        castType = moves[1, 0];
        castName = moves[1, 1];
        castPower = moves[1, 2];
        castAccuracy = moves[1, 3];
        castTarget = moves[1, 4];
        castDescription = moves[1, 5];
        castTurnLimit = moves[1, 6];
    }

    public float damagePlayer(float damage)
    {
        myElement.currHealth -= damage;
        return myElement.currHealth;
    }

    [PunRPC]
    public void announceStats(string element, float health, float statera)
    {
        switch (element)
        {
            case "Fire":
                fireHealth = health;
                fireStateri = statera;
                myElementType = element;
                break;
            case "Water":
                waterHealth = health;
                waterStateri = statera;
                myElementType = element;
                break;
            case "Earth":
                earthHealth = health;
                earthStateri = statera;
                myElementType = element;
                break;
            case "Wind":
                windHealth = health;
                windStateri = statera;
                myElementType = element;
                break;
        }

        photonView.RPC("announceStats", RpcTarget.Others, element, health, statera);
    }

}
