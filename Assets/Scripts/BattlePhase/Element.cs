using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Element : MonoBehaviour
{
    public string elementType;
    public float currHealth;
    public float maxHealth;
    public float defence;
    public float attack;
    public float speed;
    public float currStatera;
    public float elementStatera;
    public bool alive;
    public string[,] myMoves = new string[3,7];

    public string ElementType
    {
        get { return elementType; }
        set { elementType = value; }
    }

    public float CurrentHealth
    {
        get { return currHealth; }
        set { currHealth = value; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float Defence
    {
        get { return defence; }
        set { defence = value; }
    }

    public float Attack
    {
        get { return attack; }
        set { attack = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float CurrentElementStatera
    {
        get { return currStatera; }
        set { currStatera = value; }
    }

    public float ElementStatera
    {
        get { return elementStatera; }
        set { elementStatera = value; }

    }

    public bool IsAlive
    {
        get { return alive; }
        set { alive = value; }
    }

    public string[,] MyMoves
    {
        get { return myMoves; }
        set { myMoves = value; }
    }

    public void heal(float aid)
    {   if (alive)
        {
            if ((currHealth + aid) > maxHealth)
            {
                currHealth = maxHealth;
            }
            else
            {
                currHealth += aid;
            }
        }
    }

    public void fullHeal()
    {
        if(alive)
        {
            currHealth = maxHealth;
        }
    }

    public void damage(float dmg)
    {
        if (alive)
        {
            if (currHealth - dmg <= 0)
            {
                currHealth = 0;
                death();
            }
            else
            {
                currHealth -= dmg;
            }
        }
    }

    public void death()
    {
        alive = false;
    }

    public void revive()
    {
        if (!alive)
        {
            alive = true;
            heal(maxHealth / 3);
        }
    }

    public void getPlayerStats(string element)
    {
        StartCoroutine(LoadStats(element));
    }

    public void updatePlayerStats()
    {
        StartCoroutine(UpdateStats());
    }

    public void getPlayerMoves(string element)
    {
        StartCoroutine(LoadMoves(element));
    }

    IEnumerator LoadStats(string element)
    {
        string uri = "http://16.171.171.137/GetStatus.php";

        //Debug.Log(uri);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                //Debug.Log(webRequest.error);
            }
            else
            {
                string playerStats = webRequest.downloadHandler.text;

                string[] stats = playerStats.Split(',', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < stats.Length;i+=6)
                { 
                    if (element.Equals(stats[i]))
                    {
                        ElementType = stats[i];
                        MaxHealth = float.Parse(stats[i + 1]);
                        CurrentHealth = MaxHealth;
                        Defence = float.Parse(stats[i + 2]);
                        Attack = float.Parse(stats[i + 3]);
                        Speed = float.Parse(stats[i + 4]);
                        ElementStatera = float.Parse(stats[i + 5]);
                        CurrentElementStatera = ElementStatera;

                        if (CurrentHealth > 0)
                        {
                            IsAlive = true;
                        }
                        else
                        {
                            IsAlive = false;
                        }

                        break;
                    }
                }
            }
        }
    }

    IEnumerator UpdateStats()
    {
        string uri = "http://16.171.171.137/UpdateStatus.php";

        WWWForm form1 = new WWWForm();
        form1.AddField("Element_Type", ElementType.ToString());
        form1.AddField("Max_Health", MaxHealth.ToString());
        form1.AddField("Defence", Defence.ToString());
        form1.AddField("Attack", Attack.ToString());
        form1.AddField("Speed", Speed.ToString());
        form1.AddField("Elemental_Statera", ElementStatera.ToString());

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form1))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                Debug.Log("Stats updated successfully");
            }
        }
    }

    IEnumerator LoadMoves(string element)
    {
        string uri = "http://16.171.171.137/GetMoves.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string playerMoves = webRequest.downloadHandler.text;

                string[] moves = playerMoves.Split(',', StringSplitOptions.RemoveEmptyEntries);
                int moveNum = 0;
                for (int i = 0; i <= moves.Length-1; i += 7)
                {
                    if (element.Equals(moves[i]))
                    {
                        MyMoves[moveNum, 0] = moves[i];
                        MyMoves[moveNum,1] = moves[i+1];
                        MyMoves[moveNum,2] = moves[i+2];
                        MyMoves[moveNum,3] = moves[i+3];
                        MyMoves[moveNum,4] = moves[i+4];
                        MyMoves[moveNum,5] = moves[i+5];
                        MyMoves[moveNum,6] = moves[i+6];
                        moveNum++;
                    }
                }
            }
        }
    }
}