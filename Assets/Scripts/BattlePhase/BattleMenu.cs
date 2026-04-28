using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//The pupose of this class is to allow the player to input actions, targets and get visual feedback on the status of the fight and all players involved
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
    public GameObject topMenu;
    [SerializeField]
    public GameObject targetEnemy;
    [SerializeField]
    public GameObject waitingScreen;
    [SerializeField]
    public GameObject actionWindow;
    [SerializeField]
    public GameObject statusMenu;
    [SerializeField]
    public GameObject fadeScreen;
    [SerializeField]
    public GameObject winMsg;
    [SerializeField]
    public GameObject loseMsg;

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
    [SerializeField]
    public TMP_Text actionDescriptionTxt;

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

    [Header("Buttons")]
    [SerializeField]
    public Button castBtn;
    [SerializeField]
    public Button ally1Btn;
    [SerializeField]
    public Button ally2Btn;
    [SerializeField]
    public Button ally3Btn;

    public bool attackStudy;
    public bool castStudy;
    public bool targetStudy;

    public string actionSelection;
    public string targetSelection;

    public PlayerManager playerManager;
    public Element owner;

    private IEnumerator drain;

    private Animator bottomAnim;

    public void Start()
    {
        bottomAnim = bottomMenu.GetComponent<Animator>();
        resetSelection();
        resetStatuses();
    }

    public void Update()
    {
        //Set the owner to the element recorded as my own
        if(playerManager.spawnedIn)
        {
            owner = playerManager.myElement;
        }
        
        //Hide the waiting notification when everyone has locked in
        if(playerManager.turnActions.Count == 5 - playerManager.deadPlayers.Count)
        {
            waitingScreen.SetActive(false);
        }

        updateStatuses();
        checkStatuses();

        //When the fight has begun, display what is happening in the fight
        if(playerManager.actionOccuring != "")
        {
            actionDescriptionTxt.text = playerManager.actionOccuring;
            actionWindow.SetActive(true);
        }

        //Hide the battle description once the fight has finished
        if(playerManager.nextTurnReady)
        {
            actionDescriptionTxt.text = "";
            actionWindow.SetActive(false);
            resetSelection();
        }

        //Hide the battle menu and display the game win message
        if (playerManager.gameWinFlag)
        {
            topMenu.SetActive(false);
            bottomMenu.SetActive(false);
            statusMenu.SetActive(false);
            fadeScreenWin();
        }

        //Hide the battle menu and display the game lost message
        if (playerManager.gameOverFlag)
        {
            topMenu.SetActive(false);
            bottomMenu.SetActive(false);
            statusMenu.SetActive(false);
            fadeScreenLose();
        }

        //Disable the cast button if you don't have enough elemental statera
        if(owner != null && owner.CurrentElementStatera < 5)
        {
            castBtn.interactable = false;
        }
        else
        {
            castBtn.interactable = true;
        }

    }
    //This function runs as you press attack, displaying the correct information, if pressed twice it takes you to the target menu
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

    //This function runs as you press cast, displaying the correct information, if pressed twice it takes you to the target menu
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

    //This function allows you to select your target for your selected action. If the target is dead, they will not be selectable to target.
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
                    if(!playerManager.fireAlive)
                    {
                        ally1Btn.interactable = false;
                    }
                    targetAllyTxt2.text = "Earth";
                    if (!playerManager.earthAlive)
                    {
                        ally2Btn.interactable = false;
                    }
                    targetAllyTxt3.text = "Wind";
                    if (!playerManager.windAlive)
                    {
                        ally3Btn.interactable = false;
                    }
                    break;
                case "Fire":
                    targetAllyTxt1.text = "Water";
                    if(!playerManager.waterAlive)
                    {
                        ally1Btn.interactable= false;
                    }
                    targetAllyTxt2.text = "Earth";
                    if (!playerManager.earthAlive)
                    {
                        ally2Btn.interactable = false;
                    }
                    targetAllyTxt3.text = "Wind";
                    if (!playerManager.windAlive)
                    {
                        ally3Btn.interactable = false;
                    }
                    break;
                case "Earth":
                    targetAllyTxt1.text = "Water";
                    if (!playerManager.waterAlive)
                    {
                        ally1Btn.interactable = false;
                    }
                    targetAllyTxt2.text = "Fire";
                    if (!playerManager.fireAlive)
                    {
                        ally1Btn.interactable = false;
                    }
                    targetAllyTxt3.text = "Wind";
                    if (!playerManager.windAlive)
                    {
                        ally3Btn.interactable = false;
                    }
                    break;
                case "Wind":
                    targetAllyTxt1.text = "Water";
                    if (!playerManager.waterAlive)
                    {
                        ally1Btn.interactable = false;
                    }
                    targetAllyTxt2.text = "Fire";
                    if (!playerManager.fireAlive)
                    {
                        ally1Btn.interactable = false;
                    }
                    targetAllyTxt3.text = "Earth";
                    if (!playerManager.earthAlive)
                    {
                        ally2Btn.interactable = false;
                    }
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

    //Pressing back on the target window takes you back to select action
    public void cancelTarget()
    {
        targetAlly.SetActive(false);
        targetEnemy.SetActive(false);
        targetMenu.SetActive(false);
    }

    //Resets the state of the menu
    private void resetSelection()
    {
        attackStudy = false;
        castStudy = false;
        targetStudy = false;

        if(owner != null)
        {
            if (owner.alive)
            {
                bottomAnim.SetBool("HideMenu", false);
            }
            
        }
        
    }

    //Once the target is selected, this will run passing the action and target to the player manager to lock it in, also setting the lockin flag for the player manager
    public void lockIn(TMP_Text target)
    {
        switch (target.text)
        {
            case "Fire":
                actionSelection = "Cast";
                targetSelection = "Fire";
                break;
            case "Water":
                actionSelection = "Cast";
                targetSelection = "Water";
                break;
            case "Earth":
                actionSelection = "Cast";
                targetSelection = "Earth";
                break;
            case "Wind":
                actionSelection = "Cast";
                targetSelection = "Wind";
                break;
            case "The Guardian":
                actionSelection = "Attack";
                targetSelection = "The Guardian";
                break;
        }
        endTurn();
        addMove(actionSelection, targetSelection);
    }

    //Hides the battle menu and waits for everyone else to lock in their turns
    public void endTurn()
    {
        targetEnemy.SetActive(false);
        targetAlly.SetActive(false);
        targetMenu.SetActive(false);
        bottomAnim.SetBool("HideMenu", true);
        waitingScreen.SetActive(true);
    }

    //Sets the variables for the player manager when locking in the action and target
    public void addMove(string action, string target)
    {
        playerManager.turnAction = action;
        playerManager.turnTarget = target;
        playerManager.turnLockedIn = true;
    }

    //Resets all of the status bars to be full
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

    //Updates the status bars to reflect the current state of all elements
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

    //This checks to see if the drain bar coroutine needs to be run if the status of the bars change
    public void checkStatuses()
    {
        //Check Fire
        //Health
        if(fireCurrHealthBar.fillAmount < firePrevHealthBar.fillAmount)
        {
            drain = drainBar(firePrevHealthBar, fireCurrHealthBar);
            StartCoroutine(drain);
        }
        else
        {
            firePrevHealthBar.fillAmount = fireCurrHealthBar.fillAmount;
        }
        //Statera
        if (fireCurrStateraBar.fillAmount < firePrevStateraBar.fillAmount)
        {
            drain = drainBar(firePrevStateraBar, fireCurrStateraBar);
            StartCoroutine(drain);
        }
        else
        {
            firePrevStateraBar.fillAmount = fireCurrStateraBar.fillAmount;
        }

        //Check Water
        //Health
        if (waterCurrHealthBar.fillAmount < waterPrevHealthBar.fillAmount)
        {
            drain = drainBar(waterPrevHealthBar, waterCurrHealthBar);
            StartCoroutine(drain);
        }
        else
        {
            waterPrevHealthBar.fillAmount = waterCurrHealthBar.fillAmount;
        }
        //Statera
        if (waterCurrStateraBar.fillAmount < waterPrevStateraBar.fillAmount)
        {
            drain = drainBar(waterPrevStateraBar, waterCurrStateraBar);
            StartCoroutine(drain);
        }
        else
        {
            waterPrevStateraBar.fillAmount = waterCurrStateraBar.fillAmount;
        }

        //Check Earth
        //Health
        if (earthCurrHealthBar.fillAmount < earthPrevHealthBar.fillAmount)
        {
            drain = drainBar(earthPrevHealthBar, earthCurrHealthBar);
            StartCoroutine(drain);
        }
        else
        {
            earthPrevHealthBar.fillAmount = earthCurrHealthBar.fillAmount;
        }
        //Statera
        if (earthCurrStateraBar.fillAmount < earthPrevStateraBar.fillAmount)
        {
            drain = drainBar(earthPrevStateraBar, earthCurrStateraBar);
            StartCoroutine(drain);
        }
        else
        {
            earthPrevStateraBar.fillAmount = earthCurrStateraBar.fillAmount;
        }

        //Check Wind
        //Health
        if (windCurrHealthBar.fillAmount < windPrevHealthBar.fillAmount)
        {
            drain = drainBar(windPrevHealthBar, windCurrHealthBar);
            StartCoroutine(drain);
        }
        else
        {
            windPrevHealthBar.fillAmount = windCurrHealthBar.fillAmount;
        }
        //Statera
        if (windCurrStateraBar.fillAmount < windPrevStateraBar.fillAmount)
        {
            drain = drainBar(windPrevStateraBar, windCurrStateraBar);
            StartCoroutine(drain);
        }
        else
        {
            windPrevStateraBar.fillAmount = windCurrStateraBar.fillAmount;
        }

        //Check Chaos
        //Health
        if (chaosCurrHealthBar.fillAmount < chaosPrevHealthBar.fillAmount)
        {
            drain = drainBar(chaosPrevHealthBar, chaosCurrHealthBar);
            StartCoroutine(drain);
        }
        else
        {
            chaosPrevHealthBar.fillAmount = chaosCurrHealthBar.fillAmount;
        }
    }

    //This coroutine is run to give the health bar a drain effect so the players can see just how much damage is dealt or how much mana was used
    private IEnumerator drainBar(Image previousHealth, Image newHealth)
    {
        while(previousHealth.fillAmount > newHealth.fillAmount)
        {
            previousHealth.fillAmount = previousHealth.fillAmount - 0.005f;
            yield return new WaitForSeconds(2f);
        }
    }

    //Fades the screen to black and continues to the winning message
    public void fadeScreenWin()
    {
        fadeScreen.SetActive(true);
        StartCoroutine(waitAndPopUp(winMsg));
    }

    //Fades the screen to black and continues to the losing message
    public void fadeScreenLose()
    {
        fadeScreen.SetActive(true);
        StartCoroutine(waitAndPopUp(loseMsg));
    }

    //Coroutine that waits for the fade to balck happen before loading the window passed to it
    public IEnumerator waitAndPopUp(GameObject message)
    {
        yield return new WaitForSeconds(5);
        message.SetActive(true);
    }

    //Quit the application
    public void exitGame()
    {
        Application.Quit();
    }

    //Returns back to the main menu
    public void returnToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
