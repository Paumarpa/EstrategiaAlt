using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StrategyManager{

    public static List<Strategy> strategies;
    static StrategyManager(){
        strategies = new List<Strategy>();
        
        List<Action> growActions = new List<Action>();
        growActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_COLLECTOR]);
        growActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_BARRACKS]);
        growActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_TOWER]);
        growActions.Add(ActionManager.actions[(int)ActionTypes.CREATE_UNIT]);
        strategies.Add(new Strategy(StrategyTypes.GROW,growActions));

        List<Action> attackActions = new List<Action>();
        attackActions.Add(ActionManager.actions[(int)ActionTypes.MOVE_UNIT]);
        attackActions.Add(ActionManager.actions[(int)ActionTypes.ATTACK_UNIT]);
        attackActions.Add(ActionManager.actions[(int)ActionTypes.ATTACK_BUILDING]);
        strategies.Add(new Strategy(StrategyTypes.ATTACK,attackActions));

        List<Action> defenseActions = new List<Action>();
        defenseActions.Add(ActionManager.actions[(int)ActionTypes.MOVE_UNIT]);
        defenseActions.Add(ActionManager.actions[(int)ActionTypes.ATTACK_UNIT]);
        defenseActions.Add(ActionManager.actions[(int)ActionTypes.CREATE_UNIT]);
        defenseActions.Add(ActionManager.actions[(int)ActionTypes.BUILD_TOWER]);
        strategies.Add(new Strategy(StrategyTypes.ATTACK,defenseActions));

        List<Action> exploreActions = new List<Action>();
        exploreActions.Add(ActionManager.actions[(int)ActionTypes.MOVE_UNIT]);
        exploreActions.Add(ActionManager.actions[(int)ActionTypes.CREATE_UNIT]);
        strategies.Add(new Strategy(StrategyTypes.ATTACK,exploreActions));

    }

    public static Strategy getStrategy(){ //TODO como se decide la estrategia
        return strategies[0];
    }
}