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

public class BattleMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public GameObject actionMenu;
    [SerializeField]
    public GameObject actionDetailMenu;
    [SerializeField]
    public TMP_Text actionInfoTxt;
    [SerializeField]
    public GameObject targetMenu;
    [SerializeField]
    public GameObject targetAlly;
    [SerializeField]
    public TMP_Text targetAllyTxt1;
    [SerializeField]
    public TMP_Text targetAllyTxt2;
    [SerializeField]
    public TMP_Text targetAllyTxt3;

    [SerializeField]
    public GameObject targetEnemy;
    [SerializeField]
    public TMP_Text targetEnemyTxt;

    [SerializeField]
    public GameObject lockInMenu;
    [SerializeField]
    public GameObject waitingScreen;
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


    public void Start()
    {
        resetSelection();
    }

    public void attack()
    {
        if (!attackStudy)
        {
            actionDetailMenu.SetActive(true);
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
            switch (playerManager.myElementType)
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
            targetAllyTxt1.text = "The Guardian";
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
        actionMenu.SetActive(true);
    }

   public void checkLock()
    {

    }

    public void lockIn()
    {

    }
}
