using JetBrains.Annotations;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public Element myElement;

    public string myElementType;

    public string attackName;
    public string attackType;
    public string attackPower;
    public string attackAccuracy;
    public string attackTarget;
    public string attackDescription;

    public string castName;
    public string castType;
    public string castPower;
    public string castAccuracy;
    public string castTarget;
    public string castDescription;
    public string castTurnLimit;

    public void Start()
    {

    }

    public void Update()
    {
        if (this.transform.childCount>0)
        {
            myElement = GetComponentInChildren<BattlePlayer>().GetComponentInChildren<Element>();
        }

        if (myElement != null)
        {
            getMoves(myElement.MyMoves);
            myElementType = myElement.ElementType;
        }
    }

    public void getMoves(string[,] moves)
    {
        attackName = moves[0, 0];
        attackType = moves[0, 1];
        attackPower = moves[0, 2];
        attackAccuracy = moves[0, 3];
        attackTarget = moves[0, 4];
        attackDescription = moves[0, 5];

        castName = moves[1, 0];
        castType = moves[1, 1];
        castPower = moves[1, 2];
        castAccuracy = moves[1, 3];
        castTarget = moves[1, 4];
        castDescription = moves[1, 5];
        castTurnLimit = moves[1, 6];
    }

    public float damagePlayer(float damage)
    {

        myElement.currHealth -= damage;
        return myElement.currHealth;
    }

}
