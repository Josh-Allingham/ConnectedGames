using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//The purpose of this class is to handle everything that happen within the battle for all players, recording stats, moves taken and the state of the fight.
public class PlayerManager : MonoBehaviourPun
{
    public Element myElement;
    public bool spawnedIn = false;
    public bool despawned = false;

    public string myElementType;
    public int numOfCPU;
    public List<Element> cpuElement = new List<Element>();
    public bool cpuSpawned = false;

    public bool gameWinFlag = false;
    public bool gameOverFlag = false;

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
    public Dictionary<string, float> turnPower = new Dictionary<string, float>();
    public List<string> alivePlayers = new List<string>();
    public List<string> deadPlayers = new List<string>();
    public bool fighting = false;

    public void Update()
    {
        //First child as Host is your own player; if you are not host, the only child will be your player
        if (this.transform.childCount>0)
        {
            myElement = GetComponentInChildren<BattlePlayer>().GetComponentInChildren<Element>();
            spawnedIn = true;

        }

        //If you are the host, handle/record all of the CPUs that are spawned under the player manager and their element
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
                else if(cpuElement.Count == numOfCPU)
                {
                    cpuSpawned = true;
                }
            }
            
            //Announce the CPU element stats to all players (including non-host players)
            if(cpuElement.Count > 0)
            {
                foreach (Element cpu in cpuElement)
                {
                    getMyPlayerStats(cpu);
                    photonView.RPC("RPCAnnounceStats", RpcTarget.AllBuffered, cpu.elementType, cpu.maxHealth, cpu.currHealth, cpu.elementStatera, cpu.currStatera, cpu.speed, cpu.alive);
                }
            }

        }

        //Announce my element stats to all other players
        if (myElement != null)
        {
            getMyPlayerStats(myElement);
            photonView.RPC("RPCAnnounceStats", RpcTarget.AllBuffered, myElementType, myElement.maxHealth, myElement.currHealth, myElement.elementStatera, myElement.currStatera, myElement.speed, myElement.alive);

        }

        //If you have spawned in with your element
        if (spawnedIn)
        {
            //And you have locked your turn in using the battle menu, handle your actions and target before announcing your turn to everyone
            if (turnLockedIn)
            {
                turnLockedIn = false;
                //Check if the attack lands or misses before recording
                if (turnAction == "Attack" && myElement.IsAlive)
                {
                    int attackHitProb = UnityEngine.Random.Range(0, 100);
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
                //Check if cast lands or misses before recording
                if (turnAction == "Cast" && myElement.IsAlive)
                {
                    int castHitProb = UnityEngine.Random.Range(0, 100);
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

                //Also if you are the host do this for all the CPUs
                if (PhotonNetwork.IsMasterClient)
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
            //If you are the host and your element is dead, still continue to handle all of CPU actions
            else if (!myElement.alive && cpuSpawned)
            {
                if (PhotonNetwork.IsMasterClient)
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
            
        }

        //Starts the coroutine for the fight if everyone that is alive has locked in their turn
        if(turnActions.Count == 5 - deadPlayers.Count && !fighting)
        {
            fighting = true;
            photonView.RPC("RPCCheckSpeed", RpcTarget.AllBuffered);
            photonView.RPC("RPCCheckAlive", RpcTarget.AllBuffered);
            StartCoroutine(commenceFight());
        }

        //Once the turn has finished
        if(nextTurnReady)
        {
            fighting = false;
            //Clear the recorded turns from last turn
            photonView.RPC("RPCResetTurns", RpcTarget.AllBuffered);
            nextTurnReady = false;
            //Check if the boss died, despawn and win game if true
            if(!chaosAlive)
            {
                foreach(Element cpu in cpuElement)
                {
                    if(cpu.elementType == "Chaos")
                    {
                        photonView.RPC("RPCDespawn", RpcTarget.AllBuffered, cpu.elementType);
                    }
                }
                photonView.RPC("RPCGameWin", RpcTarget.AllBuffered);
            }

            //If all elements are dead, you lose
            if (!fireAlive && !waterAlive && !earthAlive && !windAlive)
            {
                photonView.RPC("RPCGameOver", RpcTarget.AllBuffered);
            }

            //Check if your element died this turn, despawn if so
            if (!myElement.alive && !despawned)
            {
                despawned = true;
                photonView.RPC("RPCDespawn", RpcTarget.AllBuffered, myElement.elementType);
            }

            //Despawn all elements that has died
            foreach (Element cpu in cpuElement)
            {
                if (!cpu.alive)
                {
                    photonView.RPC("RPCDespawn", RpcTarget.AllBuffered, cpu.elementType);
                }
            }
        }

    }

    //This functions fetches the players current stats from the element they control
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

    //This function fetches the players moves from the element class it controls
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

    //An RPC call that updates all players of the the stats sent to it. This function is used to keep being called to ensure everyone's menu and stats are up to date
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

    //This RPC records the actions of the element passed to it with all the vital information needed for the move to be carried out on everyon'e client for a single turn
    [PunRPC]
    public void RPCRecordTurnActions(string element, string action, string target, string attackMoveName, int attackPower, string castMoveName, int castPower)
    {
        if (action == "Attack")
        {
            turnPower.Add(element, attackPower);
            turnActions.Add(element + " is using " + attackMoveName + " on " + target);
        }
        else if (action == "Cast")
        {
            turnPower.Add(element, castPower);
            turnActions.Add(element + " is using " + castMoveName + " on " + target);
            if (myElementType == element)
            {
                myElement.CurrentElementStatera = myElement.CurrentElementStatera - 5;
            }
        }
        else if (action == "Miss")
        {
            turnPower.Add(element, 0);
            turnActions.Add(element + " missed their attack on " + target);
        }
    }

    //Clears all buffers and details of all turns recorded after the turn is finished
    [PunRPC]
    public void RPCResetTurns()
    {
        turnActions.Clear();
        turnPower.Clear();
        turnLockedIn = false;
        cpuTurnsLockedIn = false;
    }

    //Called by host only to determine all of the cpu actions for this turn, including the boss actions
    public void cpuAction(Element cpuElement)
    {
        string lockInCPUAction;
        string lockInCPUTarget;
        float randomAction;
        float randomTarget;

        if (cpuElement.alive)
        {
            switch (cpuElement.elementType)
            {
                //Chaos prioritises attack the whole party, with a stunning cast on a random player. Once at half health, it uses it's second attack where the damage dealt is doubled.
                case "Chaos":
                    randomAction = UnityEngine.Random.Range(0, 1);
                    if (randomAction <= 1f) //Set to one as the cast implementation for the boss has not been implemented (Meant to be .75)
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
                        if (randomTarget <= 0.25f && fireAlive)
                        {
                            lockInCPUTarget = "Fire";
                        }
                        else if (randomTarget <= 0.5f && waterAlive)
                        {
                            lockInCPUTarget = "Water";
                        }
                        else if (randomTarget <= 0.75f && earthAlive)
                        {
                            lockInCPUTarget = "Earth";
                        }
                        else
                        {
                            lockInCPUTarget = "Wind";
                        }
                    }

                    if (lockInCPUAction == "Attack")
                    {
                        int attackHitProb = UnityEngine.Random.Range(0, 100);

                        if (cpuElement.currHealth <= cpuElement.MaxHealth / 2)
                        {
                            if (attackHitProb <= Convert.ToInt32(cpuElement.MyMoves[1, 3]))
                            {
                                photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[1, 1], Convert.ToInt32(cpuElement.MyMoves[1, 2]), cpuElement.MyMoves[2, 1], Convert.ToInt32(cpuElement.MyMoves[2, 2]));
                            }
                            else
                            {
                                lockInCPUAction = "Miss";
                            }
                        }
                        else
                        {
                            if (attackHitProb <= Convert.ToInt32(cpuElement.MyMoves[0, 3]))
                            {
                                photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[2, 1], Convert.ToInt32(cpuElement.MyMoves[2, 2]));
                            }
                            else
                            {
                                lockInCPUAction = "Miss";
                                photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[2, 1], Convert.ToInt32(cpuElement.MyMoves[2, 2]));
                            }
                        }
                    }
                    else
                    {

                        int castHitProb = UnityEngine.Random.Range(0, 100);
                        if (castHitProb <= Convert.ToInt32(cpuElement.MyMoves[2, 3]))
                        {
                            photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[2, 1], Convert.ToInt32(cpuElement.MyMoves[2, 2]));
                        }
                        else
                        {
                            lockInCPUAction = "Miss";
                            photonView.RPC("RPCRecordTurnActions", RpcTarget.AllBuffered, cpuElement.elementType, lockInCPUAction, lockInCPUTarget, cpuElement.MyMoves[0, 1], Convert.ToInt32(cpuElement.MyMoves[0, 2]), cpuElement.MyMoves[2, 1], Convert.ToInt32(cpuElement.MyMoves[2, 2]));
                        }
                    }

                    break;
                //Water prioritises casting on the other party members if they are below half health, otherwise it will attack the boss
                case "Water":
                    //Check other players health to heal, otheriwse copy others actions
                    if (((fireHealth / fireMaxHealth) < 0.33f) && fireAlive && cpuElement.CurrentElementStatera >= 5)
                    {
                        lockInCPUAction = "Cast";
                        lockInCPUTarget = "Fire";
                        cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                    }
                    else if (((earthHealth / earthMaxHealth) < 0.33f) && earthAlive && cpuElement.CurrentElementStatera >= 5)
                    {
                        lockInCPUAction = "Cast";
                        lockInCPUTarget = "Earth";
                        cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                    }
                    else if (((windHealth / windMaxHealth) < 0.33f) && windAlive && cpuElement.CurrentElementStatera >= 5)
                    {
                        lockInCPUAction = "Cast";
                        lockInCPUTarget = "Wind";
                        cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                    }
                    else
                    {
                        lockInCPUAction = "Attack";
                        lockInCPUTarget = "The Guardian";
                    }

                    if (lockInCPUAction == "Attack")
                    {
                        int attackHitProb = UnityEngine.Random.Range(0, 100);
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
                //Fire prioritises its high damage attack on the boss, with a low chance to cast on a team member 
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
                        if (randomTarget <= 0.33f && windAlive && cpuElement.CurrentElementStatera >= 5)
                        {
                            lockInCPUTarget = "Wind";
                            cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                        }
                        else if (randomTarget <= 0.66f && waterAlive && cpuElement.CurrentElementStatera >= 5)
                        {
                            lockInCPUTarget = "Water";
                            cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                        }
                        else if(earthAlive && cpuElement.CurrentElementStatera >= 5)
                        {
                            lockInCPUTarget = "Earth";
                            cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                        }
                        else
                        {
                            lockInCPUAction = "Attack";
                            lockInCPUTarget = "The Guardian";
                        }
                    }

                    if (lockInCPUAction == "Attack")
                    {
                        int attackHitProb = UnityEngine.Random.Range(0, 100);
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
                //Similar to Water, Earth prioritises casting on the other party members if they are below half health, otherwise attacking the boss
                case "Earth":
                    if (((fireHealth / fireMaxHealth) < 0.33f) && fireAlive && cpuElement.CurrentElementStatera >= 5)
                    {
                        lockInCPUAction = "Cast";
                        lockInCPUTarget = "Fire";
                        cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                    }
                    else if (((waterHealth / waterMaxHealth) < 0.33f) && waterAlive && cpuElement.CurrentElementStatera >= 5)
                    {
                        lockInCPUAction = "Cast";
                        lockInCPUTarget = "Water";
                        cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                    }
                    else if (((windHealth / windMaxHealth) < 0.33f) && windAlive && cpuElement.CurrentElementStatera >= 5)
                    {
                        lockInCPUAction = "Cast";
                        lockInCPUTarget = "Wind";
                        cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                    }
                    else
                    {
                        lockInCPUAction = "Attack";
                        lockInCPUTarget = "The Guardian";
                    }

                    if (lockInCPUAction == "Attack")
                    {
                        int attackHitProb = UnityEngine.Random.Range(0, 100);
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
                //Wind prioritises its fast and more accurate attack, only casting on team members with a small chance
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
                        if (randomTarget <= 0.33f && fireAlive && cpuElement.CurrentElementStatera >= 5)
                        {
                            lockInCPUTarget = "Fire";
                            cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                        }
                        else if (randomTarget <= 0.66f && waterAlive && cpuElement.CurrentElementStatera >= 5)
                        {
                            lockInCPUTarget = "Water";
                            cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                        }
                        else if (earthAlive && cpuElement.CurrentElementStatera >= 5)
                        {
                            lockInCPUTarget = "Earth";
                            cpuElement.CurrentElementStatera = cpuElement.CurrentElementStatera - 5;
                        }
                        else
                        {
                            lockInCPUAction = "Attack";
                            lockInCPUTarget = "The Guardian";
                        }
                    }

                    if (lockInCPUAction == "Attack")
                    {
                        int attackHitProb = UnityEngine.Random.Range(0, 100);
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
    }

    //Gets the speed of all members, dead or alive, and orders them from fastest to slowest before the turn starts
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

    //This function runs as a coroutine that everyone handles client side
    public IEnumerator commenceFight()
    {
        //First check the order the turns should be carried out in based on player speed
        for (int e = 0; e < turnOrder.Count; e++)
        {
            //If the boss dies, the fight should no longer continue
            if(!chaosAlive)
            {
                break;
            }
            //Quick check of speed and health before the next action is carried out
            photonView.RPC("RPCCheckSpeed", RpcTarget.AllBuffered);
            photonView.RPC("RPCCheckAlive", RpcTarget.AllBuffered);
            var player = turnOrder.ElementAt(e);
            if (alivePlayers.Contains(player.Key))
            {
                for (int i = 0; i < turnActions.Count; i++)
                {
                    if (turnActions[i].Contains(player.Key))
                    {
                        string[] actionArray = turnActions[i].Split(' ');
                        //Whos carrying out the action
                        string actionee = actionArray[0];
                        //Did they miss
                        bool miss = (actionArray[1] == "missed");
                        //Who is the target
                        string target = actionArray[actionArray.Length - 1];
                        //power of the attack
                        float damageToDeal;
                        //power of the cast (Simplified to just heal for the time purposes)
                        float healToDeal;

                        if (actionee == player.Key && (alivePlayers.Contains(target) || target == "Guardian" || target == "Players"))
                        {
                            //Display the turn
                            actionOccuring = turnActions[i];
                            yield return new WaitForSeconds(2);

                            //Guardian Attacks
                            if(target == "Players" && !miss)
                            {
                                damageToDeal = turnPower[actionee] / 4;
                                myElement.damage(damageToDeal);
                            }
                            //A team member casts
                            else if(myElement.elementType == target && (actionee == "Fire" || actionee == "Water" ||actionee == "Earth" || actionee == "Wind"))
                            {
                                //Simplified Cast abillity to just heal for time purposes
                                healToDeal = turnPower[actionee];
                                myElement.heal(healToDeal);
                            }
                            //Boss cast implementation should go here on player element
                            //else if(myElement.elementType == target && actionee !="The")
                            //{
                            //    switch(actionee)
                            //    {
                            //        case "Fire":
                            //            break;
                            //        case "Water":
                            //            break;
                            //        case "Earth":
                            //            break;
                            //        case "Wind":
                            //            break;
                            //    }
                            //}
                            //Handles all cpu effects for this turn
                            for (int cpu = 0; cpu < cpuElement.Count; cpu++)
                            {
                                //CPUs taking damage from chaos attack
                                if (target == "Players" && !miss && cpuElement[cpu].elementType != "Chaos")
                                {
                                    damageToDeal = turnPower[actionee] / 4;
                                    cpuElement[cpu].damage(damageToDeal);
                                }
                                //CPU boss taking damage from team
                                else if (cpuElement[cpu].elementType == "Chaos" && target == "Guardian" && !miss)
                                {
                                    damageToDeal = turnPower[actionee];
                                    cpuElement[cpu].damage(damageToDeal);
                                }
                                //CPUs having cast affects on them
                                else if (cpuElement[cpu].elementType == target && (actionee == "Fire" || actionee == "Water" || actionee == "Earth" || actionee == "Wind"))
                                {
                                    //Simplified Cast abillity to just heal for time purposes
                                    healToDeal = turnPower[actionee];
                                    cpuElement[cpu].heal(healToDeal);
                                }
                                //Boss cast implementation should go here on cpu element
                                //else if (cpuElement[cpu].elementType == target && actionee != "The")
                                //{
                                //    switch (actionee)
                                //    {
                                //        case "Fire":
                                //            break;
                                //        case "Water":
                                //            break;
                                //        case "Earth":
                                //            break;
                                //        case "Wind":
                                //            break;
                                //    }
                                //}
                            }
                        }
                        else if(!alivePlayers.Contains(target))
                        {
                            //The target of the turn died before it was carried out
                            actionOccuring = player.Key + "'s target was lost";
                        }
                    }
                    yield return new WaitForSeconds(1);
                }
            }
            else
            {
                //Displayed if they died before their turn was executed
                actionOccuring = player.Key + " succumbed to their wounds...";
                yield return new WaitForSeconds(3);
            }
        }
        nextTurnReady = true;
        actionOccuring = "";
    }
    
    //Sanity checks that players are alive before the turn starts, and adds them to a list to see if the move should be carried out or if they died during the turn
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

    //Handles "despawning" the elements at the end of the turn if they died during the turn
    [PunRPC]
    public void RPCDespawn(string element)
    {        
        if(PhotonNetwork.IsMasterClient)
        {
            if(myElementType == element)
            {
                myElement.GetComponentInChildren<SpriteRenderer>().enabled = false;
                alivePlayers.Remove(element);
                deadPlayers.Add(element);
            }

            for(int i = 0; i< cpuElement.Count; i++)
            {
                if (cpuElement[i].elementType == element)
                {
                    cpuElement[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
                    alivePlayers.Remove(element);
                    deadPlayers.Add(element);
                }
            }
        }
        else
        {
            GameObject despawn = GameObject.FindWithTag(element);
            despawn.GetComponentInChildren<SpriteRenderer>().enabled = false;
            alivePlayers.Remove(element);
            deadPlayers.Add(element);
        }
    }

    //Handles if the team gets defeated by the boss
    [PunRPC]
    public void RPCGameOver()
    {
        gameOverFlag = true;
    }

    //Handles if the team defeat the boss and beat the game
    [PunRPC]
    public void RPCGameWin()
    {
        gameWinFlag = true;
    }
}
