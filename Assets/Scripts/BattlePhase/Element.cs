using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class Element : MonoBehaviour
{
    public string elementType;
    public float currHealth;
    public float maxHealth;
    public float defence;
    public float attack;
    public float speed;
    public float elementStatera;
    public bool alive;


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

    IEnumerator LoadStats(string element)
    {
        string uri = "http://16.171.171.137/GetStatus.php";

        Debug.Log(uri);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string playerStats = webRequest.downloadHandler.text;

                string[] stats = playerStats.Split(',', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < playerStats.Length-7;i+=7)
                { 
                    if (element.Equals(stats[i]))
                    {
                        ElementType = stats[i];
                        CurrentHealth = float.Parse(stats[i + 1]);
                        MaxHealth = float.Parse(stats[i + 2]);
                        Defence = float.Parse(stats[i + 3]);
                        Attack = float.Parse(stats[i + 4]);
                        Speed = float.Parse(stats[i + 5]);
                        ElementStatera = float.Parse(stats[i + 6]);

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
        form1.AddField("Curr_Health", CurrentHealth.ToString());
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
}