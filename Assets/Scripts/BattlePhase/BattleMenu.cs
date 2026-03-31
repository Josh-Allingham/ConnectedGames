using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    [Header("Selection Menus")]
    [SerializeField]
    private GameObject actionMenu;
    [SerializeField]
    private GameObject skillInfoMenu;
    [SerializeField]
    private GameObject targetMenu;
    [SerializeField]
    private GameObject targetInfoMenu;
    [SerializeField]
    private Image timer;

    public float timeLimit;
    public float timerCurrent;
    public bool timeUp;

    public bool attackStudy;
    public bool castStudy;
    public bool targetStudy;

    public void Start()
    {
        resetSelection();
    }

    private void FixedUpdate()
    {
        TimerTick();
        TimeUpCheck();
    }


    public void attack()
    {
        if (targetMenu.activeSelf)
        {
            targetMenu.SetActive(false);
            targetStudy = false;
        }

        if (attackStudy == false && castStudy == false)
        {
            //Show the attack info on menu
            attackInfo(true);
            attackStudy = true;
        }
        else if(attackStudy == true && castStudy == false)
        {
            //Lock in attack and show the target menu
            attackInfo(false);
            targetMenu.SetActive(true);
            attackStudy = false;
        }
        else if(attackStudy == false && castStudy == true)
        {
            //Show the attack info on menu by replacing cast info
            attackInfo(true);
            castStudy = false;
            attackStudy = true;
        }
    }

    public void attackInfo(bool show)
    {
        if(!show)
        {
            skillInfoMenu.SetActive(false);
        }
        else
        {
            skillInfoMenu.SetActive(true);
        }
    }


    public void cast()
    {
        if(targetMenu.activeSelf)
        {
            targetMenu.SetActive(false);
            targetStudy = false;
        }

        if (attackStudy == false && castStudy == false)
        {
            //Show the cast info on menu 
            castInfo(true);
            castStudy = true;
        }
        else if (attackStudy == true && castStudy == false)
        {
            //Show the cast info on menu by replacing attack info
            castInfo(true);
            attackStudy = false;
            castStudy = true;
        }
        else if (attackStudy == false && castStudy == true)
        {
            //Lock in cast and show the target menu
            attackInfo(false);
            targetMenu.SetActive(true);
            castStudy = false;
        }
    }

    public void castInfo(bool show)
    {
        if (!show)
        {
            skillInfoMenu.SetActive(false);
        }
        else
        {
            skillInfoMenu.SetActive(true);
        }
    }

    public void target()
    {
        if (targetStudy == false)
        {
            //Show the target info on menu
            targetInfo(true);
            targetStudy = true;
        }
    }

    public void targetInfo(bool show)
    {
        if (!show)
        {
            targetInfoMenu.SetActive(false);
        }
        else
        {
            targetInfoMenu.SetActive(true);
        }
    }

    //Reduce the timer by the time passed in real-time
    public void TimerTick()
    {
        if (timerCurrent > 0)
        {
            timerCurrent -= Time.deltaTime;
            timer.fillAmount= timerCurrent / timeLimit;

        }
        else
        {
            timerCurrent = 0;
            timer.fillAmount = 0;
        }
    }

    //This checks if the timer has hit 0
    public void TimeUpCheck()
    {
        if (timerCurrent <= 0)
        {
            timeUp = true;
        }
    }

    public void resetTimer()
    {
        timeUp = false;
        timeLimit = 60f; //1 minute
        timerCurrent = timeLimit; //Reset timer
        timer.fillAmount = timerCurrent; //Full timer at the start of the battle
    }

    private void resetSelection()
    {
       
    }

}
