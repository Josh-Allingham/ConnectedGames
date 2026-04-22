using Photon.Pun;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviourPun 
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

    public bool cpuTurnsLockedIn;
    public bool turnLockedIn;
    public string turnAction;
    public string turnTarget;

    public float fireHealth;
    public float fireMaxHealth;
    public float fireStatera;
    public float fireMaxStatera;
    public float fireSpeed;

    public float waterHealth;
    public float waterMaxHealth;
    public float waterStatera;
    public float waterMaxStatera;
    public float waterSpeed;

    public float earthHealth;
    public float earthMaxHealth;
    public float earthStatera;
    public float earthMaxStatera;
    public float earthSpeed;

    public float windHealth;
    public float windMaxHealth;
    public float windStatera;
    public float windMaxStatera;
    public float windSpeed;

    public float chaosHealth;
    public float chaosMaxHealth;
    public float chaosStatera;
    public float chaosMaxStatera;
    public float chaosSpeed;

    public List<string> turnActions = new List<string>();
    public List<bool> turnActionRecorded = new List<bool>();
    public bool nextTurnReady;


    public void Update()
    {

        if (this.transform.childCount>0)
        {
            myElement = GetComponentInChildren<BattlePlayer>().GetComponentInChildren<Element>();
        }

        if ((PhotonNetwork.IsMasterClient && this.transform.childCount > 1))
        {
            numOfCPU = 5 - PhotonNetwork.CurrentRoom.PlayerCount;
            for (int i = 0; i < numOfCPU; i++)
            {
                Element cpu = this.transform.GetChild(i + 1).GetComponentInChildren<BattleCPU>().GetComponentInChildren<Element>();
                if((cpu != null) && (cpuElement.Count < numOfCPU))
                {
                    cpuElement.Add(cpu);
                }
            }

            if(cpuElement.Count > 0)
            {
                foreach (Element cpu in cpuElement)
                {
                    getMyPlayerStats(cpu);
                    photonView.RPC("announceStats", RpcTarget.AllBuffered, cpu.elementType, cpu.maxHealth, cpu.currHealth, cpu.elementStatera, cpu.currStatera, cpu.speed);
                }
            }

            if(!cpuTurnsLockedIn && cpuElement.Count == numOfCPU)
            {
                foreach(Element cpu in cpuElement)
                {
                    cpuAction(cpu);
                }
                cpuTurnsLockedIn = true;
            }
        }

        if (myElement != null)
        {
            getMyPlayerStats(myElement);
            photonView.RPC("announceStats", RpcTarget.AllBuffered, myElementType, myElement.maxHealth, myElement.currHealth, myElement.elementStatera, myElement.currStatera, myElement.speed);
        }

        if(turnLockedIn)
        {
            photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, myElementType, turnAction, turnTarget);
            turnLockedIn = false;
        }

        if(turnActions.Count == 5)
        {
           
        }

        if(nextTurnReady)
        {
            photonView.RPC("RPCResetTurns", RpcTarget.AllBuffered);
            nextTurnReady = false;
        }
    }

    public void getMyPlayerStats(Element element)
    {
        switch (element.ElementType)
        {
            case "Fire":
                fireMaxHealth = element.maxHealth;
                fireHealth = element.currHealth;
                fireMaxStatera = element.ElementStatera;
                fireStatera = element.CurrentElementStatera;
                fireSpeed = element.Speed;
                myElementType = element.ElementType;
                break;
            case "Water":
                waterMaxHealth = element.maxHealth;
                waterHealth = element.currHealth;
                waterMaxStatera = element.ElementStatera;
                waterStatera = element.CurrentElementStatera;
                waterSpeed = element.Speed;
                myElementType = element.ElementType;
                break;
            case "Earth":
                earthMaxHealth = element.maxHealth;
                earthHealth = element.currHealth;
                earthMaxStatera = element.ElementStatera;
                earthStatera = element.CurrentElementStatera;
                earthSpeed = element.Speed;
                myElementType = element.ElementType;
                break;
            case "Wind":
                windMaxHealth = element.maxHealth;
                windHealth = element.currHealth;
                windMaxStatera = element.ElementStatera;
                windStatera = element.CurrentElementStatera;
                windSpeed = element.Speed;
                myElementType = element.ElementType;
                break;
            case "Chaos":
                chaosMaxHealth = element.maxHealth;
                chaosHealth = element.currHealth;
                chaosMaxStatera = element.ElementStatera;
                chaosStatera = element.CurrentElementStatera;
                chaosSpeed = element.Speed;
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
    public void announceStats(string element, float maxHealth, float health, float maxStatera, float statera, float speed)
    {
        switch (element)
        {
            case "Fire":
                fireHealth = health;
                fireMaxHealth = maxHealth;
                fireStatera = statera;
                fireMaxStatera = maxStatera;
                fireSpeed = speed;
                break;
            case "Water":
                waterHealth = health;
                waterMaxHealth = maxHealth;
                waterStatera = statera;
                waterMaxStatera = maxStatera;
                waterSpeed = speed;
                break;
            case "Earth":
                earthHealth = health;
                earthMaxHealth = maxHealth;
                earthStatera = statera;
                earthMaxStatera = maxStatera;
                earthSpeed = speed;
                break;
            case "Wind":
                windHealth = health;
                windMaxHealth = maxHealth;
                windStatera = statera;
                windMaxStatera = maxStatera;
                windSpeed = speed;
                break;
            case "Chaos":
                chaosHealth = health;
                chaosMaxHealth = maxHealth;
                chaosStatera = statera;
                chaosMaxStatera = maxStatera;
                chaosSpeed = speed;
                break;
        }
    }

    [PunRPC]
    public void RPCRecordTurnActions(string element, string action, string target)
    {
        turnActions.Add(element + " using " + action + " on " + target);
        turnActionRecorded.Add(true);
    }

    [PunRPC]
    public void RPCResetTurns()
    {
        turnActions.Clear();
        turnActionRecorded.Clear();
        cpuTurnsLockedIn = false;
        turnLockedIn = false;
    }

    public void cpuAction(Element cpuElement)
    {
        string lockInCPUAction;
        string lockInCPUTarget;
        float randomAction;
        float randomTarget;

        switch (cpuElement.elementType)
        {
            case "Chaos":
                randomAction = Random.Range(0, 1);
                if(randomAction <= .75f)
                {
                    lockInCPUAction = cpuElement.myMoves[0,1];
                }
                else
                {
                    lockInCPUAction = cpuElement.myMoves[1,1];
                }

                if(lockInCPUAction == cpuElement.myMoves[0, 1])
                {
                    lockInCPUTarget = "All Players";
                }
                else
                {
                    randomTarget = Random.Range(0, 1);
                    if (randomTarget <= 0.25f)
                    {
                        lockInCPUTarget = "Fire";
                    }
                    else if (randomTarget <= 0.5f)
                    {
                        lockInCPUTarget = "Water";
                    }
                    else if (randomTarget <= 0.75f)
                    {
                        lockInCPUTarget = "Earth";
                    }
                    else
                    {
                        lockInCPUTarget = "Wind";
                    }
                }  
                photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget);
                break;
            case "Water":
                //Check other players health to heal, otheriwse copy others actions
                if((fireHealth / fireMaxHealth) < 0.33f)
                {
                    lockInCPUAction = cpuElement.myMoves[1, 1];
                    lockInCPUTarget = "Fire";
                }
                else if((earthHealth / earthMaxHealth) < 0.33f)
                {
                    lockInCPUAction = cpuElement.myMoves[1, 1];
                    lockInCPUTarget = "Earth";
                }
                else if((windHealth / windMaxHealth) < 0.33f)
                {
                    lockInCPUAction = cpuElement.myMoves[1, 1];
                    lockInCPUTarget = "Wind";
                }
                else
                {
                    lockInCPUAction = cpuElement.myMoves[0, 1];
                    lockInCPUTarget = "The Guardian";
                }
                photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget);
                break;
            case "Fire":
                randomAction = Random.Range(0, 1);
                if (randomAction <= .75f)
                {
                    lockInCPUAction = cpuElement.myMoves[0, 1];
                }
                else
                {
                    lockInCPUAction = cpuElement.myMoves[1, 1];
                }

                if (lockInCPUAction == cpuElement.myMoves[0, 1])
                {
                    lockInCPUTarget = "The Guardian";
                }
                else
                {
                    randomTarget = Random.Range(0, 1);
                    if (randomTarget <= 0.33f)
                    {
                        lockInCPUTarget = "Wind";
                    }
                    else if (randomTarget <= 0.66f)
                    {
                        lockInCPUTarget = "Water";
                    }
                    else
                    {
                        lockInCPUTarget = "Earth";
                    }
                }
                photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget);
                break;
            case "Earth":
                if ((fireHealth / fireMaxHealth) < 0.33f)
                {
                    lockInCPUAction = cpuElement.myMoves[1, 1];
                    lockInCPUTarget = "Fire";
                }
                else if ((waterHealth / waterMaxHealth) < 0.33f)
                {
                    lockInCPUAction = cpuElement.myMoves[1, 1];
                    lockInCPUTarget = "Water";
                }
                else if ((windHealth / windMaxHealth) < 0.33f)
                {
                    lockInCPUAction = cpuElement.myMoves[1, 1];
                    lockInCPUTarget = "Wind";
                }
                else
                {
                    lockInCPUAction = cpuElement.myMoves[0, 1];
                    lockInCPUTarget = "The Guardian";
                }
                photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget);
                break;
            case "Wind":
                randomAction = Random.Range(0, 1);
                if (randomAction <= .75f)
                {
                    lockInCPUAction = cpuElement.myMoves[0, 1];
                }
                else
                {
                    lockInCPUAction = cpuElement.myMoves[1, 1];
                }

                if (lockInCPUAction == cpuElement.myMoves[0, 1])
                {
                    lockInCPUTarget = "The Guardian";
                }
                else
                {
                    randomTarget = Random.Range(0, 1);
                    if (randomTarget <= 0.33f)
                    {
                        lockInCPUTarget = "Fire";
                    }
                    else if (randomTarget <= 0.66f)
                    {
                        lockInCPUTarget = "Water";
                    }
                    else
                    {
                        lockInCPUTarget = "Earth";
                    }
                }
                photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget);
                break;
        }

    }

}
