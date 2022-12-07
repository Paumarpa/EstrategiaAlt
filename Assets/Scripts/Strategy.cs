using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Strategy
{
    private StrategyTypes type;
    private List<Action> actions;

    public Strategy(StrategyTypes type, List<Action> actions){
        this.type = type;
        this.actions = actions;
    }

    public StrategyTypes getType(){
        return type;
    }

    public List<Action> getActions(){
        return actions;
    }

    public Action getAction(int manaAvailable, int coinAvailable){
        for (int i = 1; i < actions.Count; i++)
        {
            if(actions[i].getManaCost() <= manaAvailable && actions[i].getCoinCost() <= coinAvailable){
                return actions[i];
            }
        }

        return actions[0];
    }

    public bool isActionAvailable(int manaAvailable, int coinAvailable){
        for (int i = 0; i < actions.Count; i++)
        {
            if(actions[i].getManaCost() <= manaAvailable && actions[i].getCoinCost() <= coinAvailable){
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return "Strategy: " + type;
    }
}