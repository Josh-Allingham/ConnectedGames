using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Xml;
using System;

public class BattleMenu : MonoBehaviour
{
    [Header("Selection Menus")]
    [SerializeField] 
    private GameObject actionMenu;
    [SerializeField]
    private GameObject skillMenu;
    [SerializeField]
    private GameObject skillInfoMenu;
    [SerializeField]
    private GameObject targetMenu;
    [SerializeField]
    private GameObject targetInfoMenu;
    [SerializeField]
    private GameObject timer;


    public void attack()
    {
        skillMenu.SetActive(false);
        //Fill the skill info menu with the attack moves that can be used
        skillMenu.SetActive(true);
        actionMenu.GetComponentInChildren<Button>().interactable = false;
    }

    public void cast()
    {
        skillMenu.SetActive(false);
        //Fill the skill info menu with the cast moves that can be used
        skillMenu.SetActive(true);
        actionMenu.GetComponentInChildren<Button>().interactable = false;
    }

    public void item()
    {
        skillMenu.SetActive(false);
        //Fill the skill info menu with the items that can be used
        skillMenu.SetActive(true);
        actionMenu.GetComponentInChildren<Button>().interactable = false;
    }

}
