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
}