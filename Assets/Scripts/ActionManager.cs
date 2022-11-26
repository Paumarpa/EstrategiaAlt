using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionManager{

    public static List<Action> actions;
    /*BUILD_TOWER,
        BUILD_BARRACKS,
        BUILD_COLLECTOR,
        MOVE_UNIT,
        ATTACK_UNIT,
        ATTACK_BUILDING,
        CREATE_UNIT
        */
    static ActionManager(){
        actions = new List<Action>();
        actions.Add(new Action(ActionTypes.NONE,0,0));
        actions.Add(new Action(ActionTypes.BUILD_TOWER,2,1000));
        actions.Add(new Action(ActionTypes.BUILD_BARRACKS,2,1000));
        actions.Add(new Action(ActionTypes.BUILD_COLLECTOR,1,50));
        actions.Add(new Action(ActionTypes.MOVE_UNIT,2,1000));
        actions.Add(new Action(ActionTypes.ATTACK_UNIT,2,1000));
        actions.Add(new Action(ActionTypes.ATTACK_BUILDING,2,1000));
        actions.Add(new Action(ActionTypes.CREATE_UNIT,2,1000));
    }

    public static Action getAction(int manaAvailable, int coinAvailable){
        for (int i = 1; i < actions.Count; i++)
        {
            if(actions[i].getManaCost() <= manaAvailable && actions[i].getCoinCost() <= coinAvailable){
                return actions[i];
            }
        }

        return actions[0];
    }

    public static bool isActionAvailable(int manaAvailable, int coinAvailable){
        for (int i = 1; i < actions.Count; i++)
        {
            if(actions[i].getManaCost() <= manaAvailable && actions[i].getCoinCost() <= coinAvailable){
                return true;
            }
        }

        return false;
    }

    public static Action getActionSpecifications(ActionTypes type){
        return actions[(int)type];
    }
}