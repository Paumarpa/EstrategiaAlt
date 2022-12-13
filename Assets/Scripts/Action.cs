using UnityEngine;
public class Action
{
    private ActionTypes type;
    private int manaCost;
    private int coinCost;

    public GameObject gameObject;

    public GameObject target;

    public Action(ActionTypes type, int mana, int coin, GameObject gameObject = null, GameObject target = null){
        this.type = type;
        this.manaCost = mana;
        this.coinCost = coin;
        this.gameObject = gameObject;
        this.target = target;
    }

    public Action(Action other){
        this.type = other.type;
        this.manaCost = other.manaCost;
        this.coinCost = other.coinCost;
        this.gameObject = other.gameObject;
    }    

    public ActionTypes getType(){
        return type;
    }

    public int getManaCost(){
        return manaCost;
    }

    public int getCoinCost(){
        return coinCost;
    }

    public override string ToString()
    {
        return "Action: " + type + " Mana: " + manaCost + " Coins: " + coinCost;
    }
}