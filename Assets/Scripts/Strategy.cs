using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Strategy
{
    private StrategyTypes type;
    private List<Action> actions;
    private List<Action> plannedActions;

    public Strategy(StrategyTypes type, List<Action> actions){
        this.type = type;
        this.actions = actions;
        this.plannedActions = new List<Action>();
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

    public bool isActionAvailable(){
        if (plannedActions.Count > 0){
            return true;
        }else{
            return false;
        }
    }

    public override string ToString()
    {
        return "Strategy: " + type;
    }

    public void planActions(int manaAvailable, int coinAvailable, PlayerOrIA myUnits, PlayerOrIA enemyUnits){

        this.plannedActions = new List<Action>();

        switch (type)
        {
            case StrategyTypes.GROW:
                planGrow(manaAvailable,coinAvailable, myUnits);
                break;
            case StrategyTypes.ATTACK:
                planAttack(manaAvailable,coinAvailable, myUnits, enemyUnits);
                break;
            case StrategyTypes.DEFENSE:
                planDefense(manaAvailable,coinAvailable, myUnits, enemyUnits);
                break;
            case StrategyTypes.EXPLORE:
                planExplore(manaAvailable,coinAvailable, myUnits);
                break;
            default:
                this.plannedActions = new List<Action>();
                break;
        }

        //Debug.Log("Estrategia con " + plannedActions.Count + " acciones.");
    }

    public void planGrow(int manaAvailable, int coinAvailable, PlayerOrIA myUnits){
        int mana = manaAvailable;
        int coin = coinAvailable;

        this.plannedActions = new List<Action>();

        int collectors = myUnits.getNum("Collector");
        int towers = myUnits.getNum("Tower");
        int barracks = myUnits.getNum("Barracks");
        int units = myUnits.getNum("Unit");

        while(ActionManager.isActionAvailable(mana,coin)){
            if (barracks < 2 && ActionManager.isActionAvailable(mana,coin,ActionTypes.BUILD_BARRACKS)){
                plannedActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_BARRACKS]);
                mana -= ActionManager.getActionSpecifications(ActionTypes.BUILD_BARRACKS).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.BUILD_BARRACKS).getCoinCost();
            }else if (towers < (units + 1) / 2 && ActionManager.isActionAvailable(mana,coin,ActionTypes.BUILD_TOWER)){
                plannedActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_TOWER]);
                mana -= ActionManager.getActionSpecifications(ActionTypes.BUILD_TOWER).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.BUILD_TOWER).getCoinCost();
            }else if (collectors > (units + 1) /3 && ActionManager.isActionAvailable(mana,coin,ActionTypes.CREATE_UNIT)){
                plannedActions.Add(ActionManager.actions[(int)ActionTypes.CREATE_UNIT]);
                mana -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getCoinCost();
            }else if (ActionManager.isActionAvailable(mana,coin,ActionTypes.BUILD_COLLECTOR)){
                plannedActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_COLLECTOR]);
                mana -= ActionManager.getActionSpecifications(ActionTypes.BUILD_COLLECTOR).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.BUILD_COLLECTOR).getCoinCost();
            }else{
                break;
            }
        }
    }

    public void planDefense(int manaAvailable, int coinAvailable, PlayerOrIA myUnits, PlayerOrIA enemyUnits){
        int mana = manaAvailable;
        int coin = coinAvailable;

        this.plannedActions = new List<Action>();

        int collectors = myUnits.getNum("Collector");
        int towers = myUnits.getNum("Tower");
        int barracks = myUnits.getNum("Barracks");
        int units = myUnits.getNum("Unit");
        List<GameObject> unitsList = myUnits.getGameObjects("Unit");
        int index = 0;

        /* MOVE_UNIT ATTACK_UNIT CREATE_UNIT BUILD_TOWER
        - Mover unidades hacia unidades enemigas y planificar el ataque si es posible
        - Construir torres de defensa
        - Construir unidades
        */

        while(ActionManager.isActionAvailable(mana,coin)){
            if (units > 0 && ActionManager.isActionAvailable(mana,coin,ActionTypes.MOVE_UNIT)){
                if(unitsList[index].GetComponent<Unidad>().GetEnemigoMasCercano() != null){
                    Action action = new Action(ActionManager.actions[(int)ActionTypes.MOVE_UNIT]);
                    action.gameObject = unitsList[index];
                    action.target = enemyUnits.gameObject;
                    plannedActions.Add(action);
                    mana -= ActionManager.getActionSpecifications(ActionTypes.MOVE_UNIT).getManaCost();
                    coin -= ActionManager.getActionSpecifications(ActionTypes.MOVE_UNIT).getCoinCost();
                    

                    if (ActionManager.isActionAvailable(mana,coin,ActionTypes.ATTACK_UNIT)){
                        action = new Action(ActionManager.actions[(int)ActionTypes.ATTACK_UNIT]);
                        action.gameObject = unitsList[index];
                        action.target = enemyUnits.gameObject;
                        plannedActions.Add(action);
                        mana -= ActionManager.getActionSpecifications(ActionTypes.ATTACK_UNIT).getManaCost();
                        coin -= ActionManager.getActionSpecifications(ActionTypes.ATTACK_UNIT).getCoinCost();
                    }
                }
                units -= 1;
                index += 1;
            }else if (towers < (units + 1) / 2 && ActionManager.isActionAvailable(mana,coin,ActionTypes.BUILD_TOWER)){
                plannedActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_TOWER]);
                mana -= ActionManager.getActionSpecifications(ActionTypes.BUILD_TOWER).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.BUILD_TOWER).getCoinCost();
            }else if (ActionManager.isActionAvailable(mana,coin,ActionTypes.CREATE_UNIT)){
                plannedActions.Add(ActionManager.actions[(int)ActionTypes.CREATE_UNIT]);
                mana -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getCoinCost();
            }else{
                break;
            }
        }
    }

    public void planAttack(int manaAvailable, int coinAvailable, PlayerOrIA myUnits, PlayerOrIA enemyUnits){
        int mana = manaAvailable;
        int coin = coinAvailable;

        this.plannedActions = new List<Action>();

        int collectors = myUnits.getNum("Collector");
        int towers = myUnits.getNum("Tower");
        int barracks = myUnits.getNum("Barracks");
        int units = myUnits.getNum("Unit");
        List<GameObject> unitsList = myUnits.getGameObjects("Unit");
        int index = 0;

        while(ActionManager.isActionAvailable(mana,coin)){
            if (units > 0 && ActionManager.isActionAvailable(mana,coin,ActionTypes.MOVE_UNIT)){
                Action action = new Action(ActionManager.actions[(int)ActionTypes.MOVE_UNIT]);
                action.gameObject = unitsList[index];
                action.target = enemyUnits.gameObject;
                plannedActions.Add(action);
                mana -= ActionManager.getActionSpecifications(ActionTypes.MOVE_UNIT).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.MOVE_UNIT).getCoinCost();
                

                if (ActionManager.isActionAvailable(mana,coin,ActionTypes.ATTACK_UNIT)){
                    action = new Action(ActionManager.actions[(int)ActionTypes.ATTACK_UNIT]);
                    action.gameObject = unitsList[index];
                    action.target = enemyUnits.gameObject;
                    plannedActions.Add(action);
                    mana -= ActionManager.getActionSpecifications(ActionTypes.ATTACK_UNIT).getManaCost();
                    coin -= ActionManager.getActionSpecifications(ActionTypes.ATTACK_UNIT).getCoinCost();
                }else if (ActionManager.isActionAvailable(mana,coin,ActionTypes.ATTACK_BUILDING)){
                    action = new Action(ActionManager.actions[(int)ActionTypes.ATTACK_BUILDING]);
                    action.gameObject = unitsList[index];
                    action.target = enemyUnits.gameObject;
                    plannedActions.Add(action);
                    mana -= ActionManager.getActionSpecifications(ActionTypes.ATTACK_BUILDING).getManaCost();
                    coin -= ActionManager.getActionSpecifications(ActionTypes.ATTACK_BUILDING).getCoinCost();
                }

                units -= 1;
                index += 1;

            }else if (ActionManager.isActionAvailable(mana,coin,ActionTypes.CREATE_UNIT)){
                plannedActions.Add(ActionManager.actions[(int)ActionTypes.CREATE_UNIT]);
                mana -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getCoinCost();
            }else{
                break;
            }
        }
    }

    public void planExplore(int manaAvailable, int coinAvailable, PlayerOrIA myUnits){
        int mana = manaAvailable;
        int coin = coinAvailable;

        this.plannedActions = new List<Action>();

        int collectors = myUnits.getNum("Collector");
        int towers = myUnits.getNum("Tower");
        int barracks = myUnits.getNum("Barracks");
        int units = myUnits.getNum("Unit");
        List<GameObject> unitsList = myUnits.getGameObjects("Unit");
        int index = 0;

        while(ActionManager.isActionAvailable(mana,coin)){
            if (units > 0 && ActionManager.isActionAvailable(mana,coin,ActionTypes.MOVE_UNIT)){
                Action action = new Action(ActionManager.actions[(int)ActionTypes.MOVE_UNIT]);
                action.gameObject = unitsList[index];
                plannedActions.Add(action);
                mana -= ActionManager.getActionSpecifications(ActionTypes.MOVE_UNIT).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.MOVE_UNIT).getCoinCost();
                units -= 1;
                index += 1;
            }else if (ActionManager.isActionAvailable(mana,coin,ActionTypes.CREATE_UNIT)){
                plannedActions.Add(ActionManager.actions[(int)ActionTypes.CREATE_UNIT]);
                mana -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getManaCost();
                coin -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getCoinCost();
            }else{
                break;
            }
        }
    }

    public Action getNextAction(){
        Action result = null;
        if (plannedActions.Count > 0){
            result = plannedActions[0];
            plannedActions.RemoveAt(0);
        }

        return result;
    }
}