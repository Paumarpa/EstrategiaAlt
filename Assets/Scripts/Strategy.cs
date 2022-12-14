using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Strategy
{
    private StrategyTypes type;
    private List<Action> actions;
    private List<Action> plannedActions;

    private int mana;
    private int coin;

    public Strategy(StrategyTypes type, List<Action> actions = null){
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

    public Action getAction(){
        for (int i = 1; i < actions.Count; i++)
        {
            if(actions[i].getManaCost() <= mana && actions[i].getCoinCost() <= coin){
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

    public void planActions(PlayerOrIA myUnits, PlayerOrIA enemyUnits){

        this.plannedActions = new List<Action>();

        switch (type)
        {
            case StrategyTypes.GROW:
                planGrow(myUnits);
                break;
            case StrategyTypes.ATTACK:
                planAttack(myUnits, enemyUnits);
                break;
            case StrategyTypes.DEFENSE:
                planDefense(myUnits, enemyUnits);
                break;
            case StrategyTypes.EXPLORE:
                planExplore(myUnits);
                break;
            default:
                this.plannedActions = new List<Action>();
                break;
        }

        Debug.Log("Estrategia con " + plannedActions.Count + " acciones.");
    }

    public void planGrow(PlayerOrIA myUnits){

        this.plannedActions = new List<Action>();

        int collectors = myUnits.getNum("Collector");
        int towers = myUnits.getNum("Tower");
        int barracks = myUnits.getNum("Barracks");
        int units = myUnits.getNum("Unit");

        while(ActionManager.isActionAvailable(mana,coin)){
            if (barracks < 2 && ActionManager.isActionAvailable(mana,coin,ActionTypes.BUILD_BARRACKS))
            {
                planBuildBarracks();
            }
            else if (towers < (units + 1) / 2 && ActionManager.isActionAvailable(mana,coin,ActionTypes.BUILD_TOWER))
            {
                planBuildTower();
            }
            else if (collectors > (units + 1) /3 && ActionManager.isActionAvailable(mana,coin,ActionTypes.CREATE_UNIT))
            {
                planCreateUnit();
            }
            else if (ActionManager.isActionAvailable(mana,coin,ActionTypes.BUILD_COLLECTOR))
            {
                planBuildCollector();
            }
            else
            {
                break;
            }
        }
    }

    private void planBuildCollector()
    {
        plannedActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_COLLECTOR]);
        mana -= ActionManager.getActionSpecifications(ActionTypes.BUILD_COLLECTOR).getManaCost();
        coin -= ActionManager.getActionSpecifications(ActionTypes.BUILD_COLLECTOR).getCoinCost();
    }

    private void planCreateUnit()
    {
        plannedActions.Add(ActionManager.actions[(int)ActionTypes.CREATE_UNIT]);
        mana -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getManaCost();
        coin -= ActionManager.getActionSpecifications(ActionTypes.CREATE_UNIT).getCoinCost();
    }

    private void planBuildTower()
    {
        plannedActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_TOWER]);
        mana -= ActionManager.getActionSpecifications(ActionTypes.BUILD_TOWER).getManaCost();
        coin -= ActionManager.getActionSpecifications(ActionTypes.BUILD_TOWER).getCoinCost();
    }

    private void planBuildBarracks()
    {
        plannedActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_BARRACKS]);
        mana -= ActionManager.getActionSpecifications(ActionTypes.BUILD_BARRACKS).getManaCost();
        coin -= ActionManager.getActionSpecifications(ActionTypes.BUILD_BARRACKS).getCoinCost();
    }

    public void planDefense(PlayerOrIA myUnits, PlayerOrIA enemyUnits){

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
            if (unitsList.Count > 0 && ActionManager.isActionAvailable(mana,coin,ActionTypes.MOVE_UNIT)){
                index = Random.Range(0,unitsList.Count);
                if(unitsList[index].GetComponent<Unidad>().GetEnemigoMasCercano(enemyUnits) != null){
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
                unitsList.RemoveAt(index);
            }else if (towers < (units + 1) / 2 && ActionManager.isActionAvailable(mana,coin,ActionTypes.BUILD_TOWER)){
                planBuildTower();
            }else if (ActionManager.isActionAvailable(mana,coin,ActionTypes.CREATE_UNIT)){
                planCreateUnit();
            }else{
                break;
            }
        }
    }

    public void planAttack(PlayerOrIA myUnits, PlayerOrIA enemyUnits){

        this.plannedActions = new List<Action>();

        int collectors = myUnits.getNum("Collector");
        int towers = myUnits.getNum("Tower");
        int barracks = myUnits.getNum("Barracks");
        int units = myUnits.getNum("Unit");
        List<GameObject> unitsList = myUnits.getGameObjects("Unit");
        int index = 0;

        while(ActionManager.isActionAvailable(mana,coin)){
            if (unitsList.Count > 0 && ActionManager.isActionAvailable(mana,coin,ActionTypes.MOVE_UNIT)){
                index = Random.Range(0,unitsList.Count);
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

                unitsList.RemoveAt(index);

            }else if (ActionManager.isActionAvailable(mana,coin,ActionTypes.CREATE_UNIT)){
                planCreateUnit();
            }else{
                break;
            }
        }
    }

    public void planExplore(PlayerOrIA myUnits){

        this.plannedActions = new List<Action>();

        int collectors = myUnits.getNum("Collector");
        int towers = myUnits.getNum("Tower");
        int barracks = myUnits.getNum("Barracks");
        int units = myUnits.getNum("Unit");
        List<GameObject> unitsList = myUnits.getGameObjects("Unit");

        while(ActionManager.isActionAvailable(mana,coin)){
            if (unitsList.Count > 0 && ActionManager.isActionAvailable(mana,coin,ActionTypes.MOVE_UNIT))
            {
                int index = Random.Range(0, unitsList.Count);
                planExploreMoveUnit(unitsList[index]);
                unitsList.RemoveAt(index);
            }
            else if (ActionManager.isActionAvailable(mana,coin,ActionTypes.CREATE_UNIT)){
                planCreateUnit();
            }else{
                break;
            }
        }
    }

    private void planExploreMoveUnit(GameObject item)
    {
        Action action = new Action(ActionManager.actions[(int)ActionTypes.MOVE_UNIT]);
        action.gameObject = item;
        plannedActions.Add(action);
        mana -= ActionManager.getActionSpecifications(ActionTypes.MOVE_UNIT).getManaCost();
        coin -= ActionManager.getActionSpecifications(ActionTypes.MOVE_UNIT).getCoinCost();
    }

    public Action getNextAction(){
        Action result = null;
        if (plannedActions.Count > 0){
            result = plannedActions[0];
            plannedActions.RemoveAt(0);
        }

        return result;
    }

    public void setMana(int value){
        this.mana = value;
    }

    public void setCoins(int value){
        this.coin = value;
    }
}