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
    public void getPlayerStats(string url)
    {
        StartCoroutine(LoadStats(url));
    }

    public void updatePlayerStats(string url)
    {
        StartCoroutine(UpdateStats(url));
    }

    IEnumerator LoadStats(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string playerStats = webRequest.downloadHandler.text;
                Debug.Log(playerStats);

                float[] stats = playerStats.Split(',', 6).Select(float.Parse).ToArray();
                CurrentHealth = stats[0];
                MaxHealth = stats[1];
                Defence = stats[2];
                Attack = stats[3];
                Speed = stats[4];
                ElementStatera = stats[5];

                if(CurrentHealth > 0)
                {
                    IsAlive = true;
                }
                else
                {
                    IsAlive = false;
                }
            }
        }
    }

    IEnumerator UpdateStats(string url)
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("Curr_Health", CurrentHealth.ToString());
        form1.AddField("Max_Health", MaxHealth.ToString());
        form1.AddField("Defence", Defence.ToString());
        form1.AddField("Attack", Attack.ToString());
        form1.AddField("Speed", Speed.ToString());
        form1.AddField("Elemental_Statera", ElementStatera.ToString());

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form1))
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