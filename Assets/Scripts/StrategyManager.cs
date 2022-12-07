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
        strategies.Add(new Strategy(StrategyTypes.DEFENSE,defenseActions));

        List<Action> exploreActions = new List<Action>();
        exploreActions.Add(ActionManager.actions[(int)ActionTypes.MOVE_UNIT]);
        exploreActions.Add(ActionManager.actions[(int)ActionTypes.CREATE_UNIT]);
        strategies.Add(new Strategy(StrategyTypes.EXPLORE,exploreActions));

    }

    public static Strategy getStrategy(int collectors, int towers, int barracks, int units, bool enemyDiscovered, bool townhallDiscovered){ //TODO como se decide la estrategia
        if (townhallDiscovered){
            if (units < 5){
                return strategies[(int)StrategyTypes.GROW];
            }else{
                return strategies[(int)StrategyTypes.ATTACK];
            }
        }else{
            if (enemyDiscovered){
                return strategies[(int)StrategyTypes.DEFENSE];
            }else{
                if (units < 5 ){
                    return strategies[(int)StrategyTypes.GROW];
                }else{
                    return strategies[(int)StrategyTypes.EXPLORE];
                }
            }
        }
    }
}