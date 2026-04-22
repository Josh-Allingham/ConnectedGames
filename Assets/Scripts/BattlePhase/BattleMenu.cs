using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField]
    public GameObject bottomMenu;
    [SerializeField]
    public GameObject actionDetailMenu;
    [SerializeField]
    public GameObject targetMenu;
    [SerializeField]
    public GameObject targetAlly;
    [SerializeField]
    public GameObject lockInMenu;
    [SerializeField]
    public GameObject targetEnemy;
    [SerializeField]
    public GameObject waitingScreen;

    [Header("Text")]    
    [SerializeField]
    public TMP_Text targetAllyTxt1;
    [SerializeField]
    public TMP_Text targetAllyTxt2;
    [SerializeField]
    public TMP_Text targetAllyTxt3;
    [SerializeField]
    public TMP_Text targetEnemyTxt;
    [SerializeField]
    public TMP_Text actionInfoTxt;
    [SerializeField]
    public TMP_Text actionTitleTxt;

    [Header("Fire Status Bars")]
    [SerializeField]
    public Image firePrevHealthBar;
    [SerializeField]
    public Image fireCurrHealthBar;
    [SerializeField]
    public Image firePrevStateraBar;
    [SerializeField]
    public Image fireCurrStateraBar;

    [Header("Water Status Bars")]
    [SerializeField]
    public Image waterPrevHealthBar;
    [SerializeField]
    public Image waterCurrHealthBar;
    [SerializeField]
    public Image waterPrevStateraBar;
    [SerializeField]
    public Image waterCurrStateraBar;

    [Header("Earth Status Bars")]
    [SerializeField]
    public Image earthPrevHealthBar;
    [SerializeField]
    public Image earthCurrHealthBar;
    [SerializeField]
    public Image earthPrevStateraBar;
    [SerializeField]
    public Image earthCurrStateraBar;

    [Header("Wind Status Bars")]
    [SerializeField]
    public Image windPrevHealthBar;
    [SerializeField]
    public Image windCurrHealthBar;
    [SerializeField]
    public Image windPrevStateraBar;
    [SerializeField]
    public Image windCurrStateraBar;

    [Header("Chaos Status Bars")]
    [SerializeField]
    public Image chaosPrevHealthBar;
    [SerializeField]
    public Image chaosCurrHealthBar;

    [SerializeField]
    public Image timer;

    public float timeLimit;
    public float timerCurrent;
    public bool timeUp;

    public bool attackStudy;
    public bool castStudy;
    public bool targetStudy;

    public string actionSelection;
    public string targetSelection;

    public PlayerManager playerManager;
    public Element owner;

    private IEnumerator drain;

    private Animator anim;

    public void Start()
    {
        anim = bottomMenu.GetComponent<Animator>();
        resetSelection();
        resetStatuses();
    }

    public void Update()
    {
        owner = playerManager.myElement;
        if(playerManager.turnActions.Count == 5)
        {
            waitingScreen.SetActive(false);
        }

        updateStatuses();

        checkStatuses();
    }

    public void attack()
    {
        if (!attackStudy)
        {
            actionDetailMenu.SetActive(true);
            actionTitleTxt.text = playerManager.attackName;
            actionInfoTxt.text = playerManager.attackDescription;
            attackStudy = true;
            castStudy = false;
        }
        else
        {
            attackStudy = false;
            actionInfoTxt.text = "";
            actionDetailMenu.SetActive(false);
            target(playerManager.attackTarget);
        }
    }


    public void cast()
    {
        if (!castStudy)
        {
            actionDetailMenu.SetActive(true);
            actionTitleTxt.text = playerManager.castName;
            actionInfoTxt.text = playerManager.castDescription;
            attackStudy = false;
            castStudy = true;
        }
        else
        {
            castStudy = false;
            actionInfoTxt.text = "";
            actionDetailMenu.SetActive(false);
            target(playerManager.castTarget);
        }
    }

    public void target(string target)
    {
        targetEnemy.SetActive(false);
        targetMenu.SetActive(true);
        if(target == "Ally")
        {
            switch (owner.ElementType)
            {
                case "Water":
                    targetAllyTxt1.text = "Fire";
                    targetAllyTxt2.text = "Earth";
                    targetAllyTxt3.text = "Wind";
                    break;
                case "Fire":
                    targetAllyTxt1.text = "Water";
                    targetAllyTxt2.text = "Earth";
                    targetAllyTxt3.text = "Wind";
                    break;
                case "Earth":
                    targetAllyTxt1.text = "Water";
                    targetAllyTxt2.text = "Fire";
                    targetAllyTxt3.text = "Wind";
                    break;
                case "Wind":
                    targetAllyTxt1.text = "Water";
                    targetAllyTxt2.text = "Fire";
                    targetAllyTxt3.text = "Earth";
                    break;
            }
            targetAlly.SetActive(true);
        }
        else if(target == "Enemy")
        {
            targetAlly.SetActive(false);
            targetMenu.SetActive(true);
            targetEnemyTxt.text = "The Guardian";
            targetEnemy.SetActive(true);
        }
    }

    public void cancelTarget()
    {
        targetAlly.SetActive(false);
        targetEnemy.SetActive(false);
        targetMenu.SetActive(false);
    }

    private void resetSelection()
    {
        attackStudy = false;
        castStudy = false;
        targetStudy = false;
        anim.SetBool("HideMenu", false);
    }

    public void lockIn(TMP_Text target)
    {
        switch (target.text)
        {
            case "Fire":
                actionSelection = playerManager.castName;
                targetSelection = "Fire";
                break;
            case "Water":
                actionSelection = playerManager.castName;
                targetSelection = "Water";
                break;
            case "Earth":
                actionSelection = playerManager.castName;
                targetSelection = "Earth";
                break;
            case "Wind":
                actionSelection = playerManager.castName;
                targetSelection = "Wind";
                break;
            case "The Guardian":
                actionSelection = playerManager.attackName;
                targetSelection = "The Guardian";
                break;
        }
        endTurn();
        addMove(actionSelection, targetSelection);
    }
    public void endTurn()
    {
        targetEnemy.SetActive(false);
        targetAlly.SetActive(false);
        targetMenu.SetActive(false);
        anim.SetBool("HideMenu", true);
        waitingScreen.SetActive(true);
    }

    public void addMove(string action, string target)
    {
        playerManager.turnAction = action;
        playerManager.turnTarget = target;
        playerManager.turnLockedIn = true;
    }

    public void resetStatuses()
    {
        //Fire Reset
        firePrevHealthBar.fillAmount = 1;
        fireCurrHealthBar.fillAmount = 1;
        firePrevStateraBar.fillAmount = 1;
        fireCurrStateraBar.fillAmount = 1;
        //Water Reset
        waterPrevHealthBar.fillAmount = 1;
        waterCurrHealthBar.fillAmount = 1;
        waterPrevStateraBar.fillAmount = 1;
        waterCurrStateraBar.fillAmount = 1;
        //Earth Reset
        earthPrevHealthBar.fillAmount = 1;
        earthCurrHealthBar.fillAmount = 1;
        earthPrevStateraBar.fillAmount = 1;
        earthCurrStateraBar.fillAmount = 1;
        //Wind Reset
        windPrevHealthBar.fillAmount = 1;
        windCurrHealthBar.fillAmount = 1;
        windPrevStateraBar.fillAmount = 1;
        windCurrStateraBar.fillAmount = 1;
        //Chaos Reset
        chaosPrevHealthBar.fillAmount = 1;
        chaosCurrHealthBar.fillAmount = 1;
    }

    public void updateStatuses()
    {
        //Fire Update
        fireCurrHealthBar.fillAmount = playerManager.fireHealth / playerManager.fireMaxHealth;
        fireCurrStateraBar.fillAmount = playerManager.fireStatera / playerManager.fireMaxStatera;

        //Water Update
        waterCurrHealthBar.fillAmount = playerManager.waterHealth / playerManager.waterMaxHealth;
        waterCurrStateraBar.fillAmount = playerManager.waterStatera / playerManager.waterMaxStatera;
        //Earth Update
        earthCurrHealthBar.fillAmount = playerManager.earthHealth / playerManager.earthMaxHealth;
        earthCurrStateraBar.fillAmount = playerManager.earthStatera / playerManager.earthMaxStatera;

        //Wind Update
        windCurrHealthBar.fillAmount = playerManager.windHealth / playerManager.windMaxHealth;
        windCurrStateraBar.fillAmount = playerManager.windStatera / playerManager.windMaxStatera;
        //Chaos Update
        chaosCurrHealthBar.fillAmount = playerManager.chaosHealth / playerManager.chaosMaxHealth;
    }

    public void checkStatuses()
    {
        //Check Fire
        //Health
        if(fireCurrHealthBar.fillAmount < firePrevHealthBar.fillAmount)
        {
            drain = drainBar(firePrevHealthBar, fireCurrHealthBar);
            StartCoroutine(drain);
        }
        //Statera
        if (fireCurrStateraBar.fillAmount < firePrevStateraBar.fillAmount)
        {
            drain = drainBar(firePrevStateraBar, fireCurrStateraBar);
            StartCoroutine(drain);
        }

        //Check Water
        //Health
        if (waterCurrHealthBar.fillAmount < waterPrevHealthBar.fillAmount)
        {
            drain = drainBar(waterPrevHealthBar, waterCurrHealthBar);
            StartCoroutine(drain);
        }
        //Statera
        if (waterCurrStateraBar.fillAmount < waterPrevStateraBar.fillAmount)
        {
            drain = drainBar(waterPrevStateraBar, waterCurrStateraBar);
            StartCoroutine(drain);
        }

        //Check Earth
        //Health
        if (earthCurrHealthBar.fillAmount < earthPrevHealthBar.fillAmount)
        {
            drain = drainBar(earthPrevHealthBar, earthCurrHealthBar);
            StartCoroutine(drain);
        }
        //Statera
        if (earthCurrStateraBar.fillAmount < earthPrevStateraBar.fillAmount)
        {
            drain = drainBar(earthPrevStateraBar, earthCurrStateraBar);
            StartCoroutine(drain);
        }

        //Check Wind
        //Health
        if (windCurrHealthBar.fillAmount < windPrevHealthBar.fillAmount)
        {
            drain = drainBar(windPrevHealthBar, windCurrHealthBar);
            StartCoroutine(drain);
        }
        //Statera
        if (windCurrStateraBar.fillAmount < windPrevStateraBar.fillAmount)
        {
            drain = drainBar(windPrevStateraBar, windCurrStateraBar);
            StartCoroutine(drain);
        }

        //Check Chaos
        //Health
        if (chaosCurrHealthBar.fillAmount < chaosPrevHealthBar.fillAmount)
        {
            drain = drainBar(chaosPrevHealthBar, chaosCurrHealthBar);
            StartCoroutine(drain);
        }

    }

    private IEnumerator drainBar(Image previousHealth, Image newHealth)
    {
        while(true)
        {
            if(previousHealth.fillAmount > newHealth.fillAmount)
            {
                previousHealth.fillAmount = previousHealth.fillAmount - 0.01f;
            }
            yield return new WaitForSeconds(.5f);
        }
    }
}
