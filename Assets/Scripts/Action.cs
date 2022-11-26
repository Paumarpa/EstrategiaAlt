public class Action
{
    private ActionTypes type;
    private int manaCost;
    private int coinCost;

    public Action(ActionTypes type, int mana, int coin){
        this.type = type;
        this.manaCost = mana;
        this.coinCost = coin;
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
}