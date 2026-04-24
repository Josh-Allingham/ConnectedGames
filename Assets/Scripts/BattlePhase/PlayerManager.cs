using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEditor.Rendering.FilterWindow;


public class PlayerManager : MonoBehaviourPun 
{
    public Element myElement;

    public string myElementType;
    public int numOfCPU;
    public List<Element> cpuElement = new List<Element>();


    public string attackName;
    public string attackType;
    public int attackPower;
    public int attackAccuracy;
    public string attackTarget;
    public string attackDescription;

    public string castName;
    public string castType;
    public int castPower;
    public int castAccuracy;
    public string castTarget;
    public string castDescription;

    public bool cpuTurnsLockedIn = false;
    public bool turnLockedIn = false;
    public string turnAction;
    public string turnTarget;

    public float fireHealth;
    public float fireMaxHealth;
    public float fireStatera;
    public float fireMaxStatera;
    public float fireSpeed;
    public bool fireAlive;

    public float waterHealth;
    public float waterMaxHealth;
    public float waterStatera;
    public float waterMaxStatera;
    public float waterSpeed;
    public bool waterAlive;

    public float earthHealth;
    public float earthMaxHealth;
    public float earthStatera;
    public float earthMaxStatera;
    public float earthSpeed;
    public bool earthAlive;

    public float windHealth;
    public float windMaxHealth;
    public float windStatera;
    public float windMaxStatera;
    public float windSpeed;
    public bool windAlive;

    public float chaosHealth;
    public float chaosMaxHealth;
    public float chaosStatera;
    public float chaosMaxStatera;
    public float chaosSpeed;
    public bool chaosAlive;

    public List<string> turnActions = new List<string>();
    public string actionOccuring;
    public bool nextTurnReady = false;

    public Dictionary<string, float> playerSpeeds = new Dictionary<string, float>();
    public Dictionary<string, float> turnOrder = new Dictionary<string, float>();
    public List<string> alivePlayers = new List<string>();
    public bool fighting = false;

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
                    photonView.RPC("RPCAnnounceStats", RpcTarget.AllBuffered, cpu.elementType, cpu.maxHealth, cpu.currHealth, cpu.elementStatera, cpu.currStatera, cpu.speed, cpu.alive);
                }
            }

        }

        if (myElement != null)
        {
            getMyPlayerStats(myElement);
            photonView.RPC("RPCAnnounceStats", RpcTarget.AllBuffered, myElementType, myElement.maxHealth, myElement.currHealth, myElement.elementStatera, myElement.currStatera, myElement.speed, myElement.alive);
        }

        if(turnLockedIn)
        {
            if (turnAction == "Attack")
            {
                int attackHitProb = UnityEngine.Random.Range(0, 100);
                Debug.Log(attackHitProb);
                if (attackHitProb <= attackAccuracy)
                {
                    photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, myElementType, turnAction, turnTarget, attackName, attackPower, castName, castPower);
                }
                else
                {
                    turnAction = "Miss";
                    photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, myElementType, turnAction, turnTarget, attackName, attackPower, castName, castPower);
                }
            }
            else
            {
                int castHitProb = UnityEngine.Random.Range(0, 100);
                Debug.Log(castHitProb);
                if (castHitProb <= castAccuracy)
                {
                    photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, myElementType, turnAction, turnTarget, attackName, attackPower, castName, castPower);
                }
                else
                {
                    turnAction = "Miss";
                    photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, myElementType, turnAction, turnTarget, attackName, attackPower, castName, castPower);
                }
            }
            turnLockedIn = false;

            if(PhotonNetwork.IsMasterClient)
            {
                if (!cpuTurnsLockedIn && cpuElement.Count == numOfCPU)
                {
                    foreach (Element cpu in cpuElement)
                    {
                        cpuAction(cpu);
                    }
                    cpuTurnsLockedIn = true;
                }
            }
        }

        if(turnActions.Count == 5 && !fighting)
        {
            fighting = true;
            StartCoroutine(commenceFight());
            //photonView.RPC("RPCCommenceFight", RpcTarget.AllBuffered);
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
                fireAlive = element.alive;
                break;
            case "Water":
                waterMaxHealth = element.maxHealth;
                waterHealth = element.currHealth;
                waterMaxStatera = element.ElementStatera;
                waterStatera = element.CurrentElementStatera;
                waterSpeed = element.Speed;
                myElementType = element.ElementType;
                waterAlive = element.alive;
                break;
            case "Earth":
                earthMaxHealth = element.maxHealth;
                earthHealth = element.currHealth;
                earthMaxStatera = element.ElementStatera;
                earthStatera = element.CurrentElementStatera;
                earthSpeed = element.Speed;
                myElementType = element.ElementType;
                earthAlive = element.alive;
                break;
            case "Wind":
                windMaxHealth = element.maxHealth;
                windHealth = element.currHealth;
                windMaxStatera = element.ElementStatera;
                windStatera = element.CurrentElementStatera;
                windSpeed = element.Speed;
                myElementType = element.ElementType;
                windAlive = element.alive;
                break;
            case "Chaos":
                chaosMaxHealth = element.maxHealth;
                chaosHealth = element.currHealth;
                chaosMaxStatera = element.ElementStatera;
                chaosStatera = element.CurrentElementStatera;
                chaosSpeed = element.Speed;
                myElementType = element.ElementType;
                chaosAlive = element.alive;
                break;
        }
        getMoves(element.MyMoves);
    }

    public void getMoves(string[,] moves)
    {
        attackType = moves[0, 0];
        attackName = moves[0, 1];
        attackPower = Convert.ToInt32(moves[0, 2]);
        attackAccuracy = Convert.ToInt32(moves[0, 3]);
        attackTarget = moves[0, 4];
        attackDescription = moves[0, 5];

        castType = moves[1, 0];
        castName = moves[1, 1];
        castPower = Convert.ToInt32(moves[1, 2]);
        castAccuracy = Convert.ToInt32(moves[1, 3]);
        castTarget = moves[1, 4];
        castDescription = moves[1, 5];
    }

    [PunRPC]
    public void RPCAnnounceStats(string element, float maxHealth, float health, float maxStatera, float statera, float speed, bool alive)
    {
        switch (element)
        {
            case "Fire":
                fireHealth = health;
                fireMaxHealth = maxHealth;
                fireStatera = statera;
                fireMaxStatera = maxStatera;
                fireSpeed = speed;
                fireAlive = alive;
                break;
            case "Water":
                waterHealth = health;
                waterMaxHealth = maxHealth;
                waterStatera = statera;
                waterMaxStatera = maxStatera;
                waterSpeed = speed;
                waterAlive = alive;
                break;
            case "Earth":
                earthHealth = health;
                earthMaxHealth = maxHealth;
                earthStatera = statera;
                earthMaxStatera = maxStatera;
                earthSpeed = speed;
                earthAlive = alive;
                break;
            case "Wind":
                windHealth = health;
                windMaxHealth = maxHealth;
                windStatera = statera;
                windMaxStatera = maxStatera;
                windSpeed = speed;
                windAlive = alive;
                break;
            case "Chaos":
                chaosHealth = health;
                chaosMaxHealth = maxHealth;
                chaosStatera = statera;
                chaosMaxStatera = maxStatera;
                chaosSpeed = speed;
                chaosAlive = alive;
                break;
        }
    }

    [PunRPC]
    public void RPCRecordTurnActions(string element, string action, string target, string attackMoveName, int attackPower, string castMoveName, int castPower)
    {
        if (action == "Attack")
        {
            turnActions.Add(element + " is using " + attackMoveName + " on " + target);
        }
        else if (action == "Cast")
        {
            turnActions.Add(element + " is using " + castMoveName + " on " + target);
        }
        else if (action == "Miss")
        {
            turnActions.Add(element + " missed thier attack on " + target);
        }
    }

    [PunRPC]
    public void RPCResetTurns()
    {
        turnActions.Clear();
        turnLockedIn = false;
        cpuTurnsLockedIn = false;
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
                randomAction = UnityEngine.Random.Range(0, 1);
                if(randomAction <= .75f)
                {
                    lockInCPUAction = "Attack";
                }
                else
                {
                    lockInCPUAction = "Cast";
                }

                if (lockInCPUAction == "Attack")
                {
                    lockInCPUTarget = "All Players";
                }
                else
                {
                    randomTarget = UnityEngine.Random.Range(0, 1);
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

                if(lockInCPUAction == "Attack")
                {
                    int attackHitProb = UnityEngine.Random.Range(0, 100);
                    Debug.Log(attackHitProb);
                    if (attackHitProb <= Convert.ToInt32(cpuElement.MyMoves[0, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }
                else
                {

                    int castHitProb = UnityEngine.Random.Range(0, 100);
                    if (castHitProb <= Convert.ToInt32(cpuElement.MyMoves[1, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }

                break;
            case "Water":
                //Check other players health to heal, otheriwse copy others actions
                if((fireHealth / fireMaxHealth) < 0.33f)
                {
                    lockInCPUAction = "Cast";
                    lockInCPUTarget = "Fire";
                }
                else if((earthHealth / earthMaxHealth) < 0.33f)
                {
                    lockInCPUAction = "Cast";
                    lockInCPUTarget = "Earth";
                }
                else if((windHealth / windMaxHealth) < 0.33f)
                {
                    lockInCPUAction = "Cast";
                    lockInCPUTarget = "Wind";
                }
                else
                {
                    lockInCPUAction = "Attack";
                    lockInCPUTarget = "The Guardian";
                }

                if (lockInCPUAction == "Attack")
                {
                    int attackHitProb = UnityEngine.Random.Range(0, 100);
                    Debug.Log(attackHitProb);
                    if (attackHitProb <= Convert.ToInt32(cpuElement.MyMoves[0, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }
                else
                {

                    int castHitProb = UnityEngine.Random.Range(0, 100);
                    if (castHitProb <= Convert.ToInt32(cpuElement.MyMoves[1, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }

                break;
            case "Fire":
                randomAction = UnityEngine.Random.Range(0, 1);
                if (randomAction <= .75f)
                {
                    lockInCPUAction = "Attack";
                }
                else
                {
                    lockInCPUAction = "Cast";
                }

                if (lockInCPUAction == "Attack")
                {
                    lockInCPUTarget = "The Guardian";
                }
                else
                {
                    randomTarget = UnityEngine.Random.Range(0, 1);
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

                if (lockInCPUAction == "Attack")
                {
                    int attackHitProb = UnityEngine.Random.Range(0, 100);
                    Debug.Log(attackHitProb);
                    if (attackHitProb <= Convert.ToInt32(cpuElement.MyMoves[0, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }
                else
                {

                    int castHitProb = UnityEngine.Random.Range(0, 100);
                    if (castHitProb <= Convert.ToInt32(cpuElement.MyMoves[1, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }

                break;
            case "Earth":
                if ((fireHealth / fireMaxHealth) < 0.33f)
                {
                    lockInCPUAction = "Cast";
                    lockInCPUTarget = "Fire";
                }
                else if ((waterHealth / waterMaxHealth) < 0.33f)
                {
                    lockInCPUAction = "Cast";
                    lockInCPUTarget = "Water";
                }
                else if ((windHealth / windMaxHealth) < 0.33f)
                {
                    lockInCPUAction = "Cast";
                    lockInCPUTarget = "Wind";
                }
                else
                {
                    lockInCPUAction = "Attack";
                    lockInCPUTarget = "The Guardian";
                }

                if (lockInCPUAction == "Attack")
                {
                    int attackHitProb = UnityEngine.Random.Range(0, 100);
                    Debug.Log(attackHitProb);
                    if (attackHitProb <= Convert.ToInt32(cpuElement.MyMoves[0, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }
                else
                {

                    int castHitProb = UnityEngine.Random.Range(0, 100);
                    if (castHitProb <= Convert.ToInt32(cpuElement.MyMoves[1, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }

                break;
            case "Wind":
                randomAction = UnityEngine.Random.Range(0, 1);
                if (randomAction <= .75f)
                {
                    lockInCPUAction = "Attack";
                }
                else
                {
                    lockInCPUAction = "Cast";
                }

                if (lockInCPUAction == "Attack")
                {
                    lockInCPUTarget = "The Guardian";
                }
                else
                {
                    randomTarget = UnityEngine.Random.Range(0, 1);
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

                if (lockInCPUAction == "Attack")
                {
                    int attackHitProb = UnityEngine.Random.Range(0, 100);
                    Debug.Log(attackHitProb);
                    if (attackHitProb <= Convert.ToInt32(cpuElement.MyMoves[0, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }
                else
                {

                    int castHitProb = UnityEngine.Random.Range(0, 100);
                    if (castHitProb <= Convert.ToInt32(cpuElement.MyMoves[1, 3]))
                    {
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                    else
                    {
                        lockInCPUAction = "Miss";
                        photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]));
                    }
                }

                break;
        }

    }

    [PunRPC]
    public void RPCCheckSpeed()
    {
        playerSpeeds.Clear();
        turnOrder.Clear();

        playerSpeeds.Add("Fire", fireSpeed);
        playerSpeeds.Add("Water", waterSpeed);
        playerSpeeds.Add("Earth", earthSpeed);
        playerSpeeds.Add("Wind", windSpeed);
        playerSpeeds.Add("Chaos", chaosSpeed);

        foreach (var pair in playerSpeeds.OrderByDescending(pair => pair.Value))
        {
            turnOrder.Add(pair.Key, pair.Value);
        }
    }

    public IEnumerator commenceFight()
    {
        photonView.RPC("RPCCheckSpeed", RpcTarget.AllBuffered);
        photonView.RPC("RPCCheckAlive", RpcTarget.AllBuffered);

        foreach (string player in turnOrder.Keys)
        {
            if (alivePlayers.Contains(player))
            {
                //Debug.Log(player);
                for (int i = 0; i < turnActions.Count; i++)
                {
                    //Debug.Log(turnActions[i]);
                    if (turnActions[i].Contains(player))
                    {
                        //Debug.Log(turnAction[i] + " Contains " + player + " at " + player.IndexOf(turnActions[i]));
                        if (player.IndexOf(turnActions[i]) == -1)
                        {
                            actionOccuring = turnActions[i];
                            yield return new WaitForSeconds(4);
                            Debug.Log(actionOccuring);

                            //if (myElementType == player)
                            //{
                            //    float hitchance = UnityEngine.Random.Range(0, 100);

                            //    if (turnActions[i].Contains(attackName))
                            //    {
                            //        if (hitchance < attackAccuracy)
                            //        {
                            //            //Attack hits
                            //            yield return new WaitForSeconds(4);
                            //        }
                            //        else
                            //        {
                            //            //Miss
                            //            actionOccuring = player + " Missed their attack!";
                            //            yield return new WaitForSeconds(4);
                            //        }
                            //    }
                            //    else if (turnActions[i].Contains(castName))
                            //    {
                            //        if (hitchance < castAccuracy)
                            //        {
                            //            //Success cast
                            //            yield return new WaitForSeconds(4);
                            //        }
                            //        else
                            //        {
                            //            //cast missed
                            //            actionOccuring = player + " Missed their cast!";
                            //            yield return new WaitForSeconds(4);

                            //        }

                            //    }
                            //}
                            //else if (PhotonNetwork.IsMasterClient)
                            //{
                            //    for (int e = 0; e < cpuElement.Count; e++)
                            //    {
                            //        if (cpuElement[e].elementType == player)
                            //        {
                            //            float hitchance = UnityEngine.Random.Range(0, 100);
                            //            int cpuAttackAccu = Convert.ToInt32(cpuElement[e].myMoves[0, 3]);
                            //            int cpuCastAccu = Convert.ToInt32(cpuElement[e].myMoves[1, 3]);

                            //            if (turnActions[i].Contains(cpuElement[e].myMoves[0, 1]))
                            //            {
                            //                if (hitchance < cpuAttackAccu)
                            //                {
                            //                    //Attack hits
                            //                    yield return new WaitForSeconds(4);
                            //                }
                            //                else
                            //                {
                            //                    //Missed attack
                            //                    actionOccuring = player + " Missed their attack!";
                            //                    yield return new WaitForSeconds(4);
                            //                }
                            //            }
                            //            else if (turnActions[i].Contains(castName))
                            //            {
                            //                if (hitchance < cpuCastAccu)
                            //                {
                            //                    //Success cast
                            //                    yield return new WaitForSeconds(4);
                            //                }
                            //                else
                            //                {
                            //                    //cast missed
                            //                    actionOccuring = player + " Missed their cast!";
                            //                    yield return new WaitForSeconds(4);

                            //                }

                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }
            else
            {
                actionOccuring = player + " succumbed to their wounds...";
            }
        }
    }

    [PunRPC]
    public void RPCCheckAlive()
    {
        alivePlayers.Clear();

        if (fireAlive)
        {
            alivePlayers.Add("Fire");
        }
        if (waterAlive)
        {
            alivePlayers.Add("Water");
        }
        if (earthAlive)
        {
            alivePlayers.Add("Earth");
        }
        if (windAlive)
        {
            alivePlayers.Add("Wind");
        }
        if (chaosAlive)
        {
            alivePlayers.Add("Chaos");
        }
    }

    [PunRPC]
    public void RPCAnnounceBattleTurn()
    {

    }

}
