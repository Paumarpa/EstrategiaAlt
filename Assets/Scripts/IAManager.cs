using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    public int mana = 1;
    public int coins = 50;
    public GameMaster GMS;
    public int id = 2;

    public ActionTypes currentAction;
    public StrategyTypes currentStrategy;

    const int MANA_MAX = 15;

    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        currentAction = ActionTypes.NONE;
        currentStrategy = StrategyTypes.GROW;
        grid = GameObject.Find("Pathfinding").GetComponent<Grid>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMyTurn()){
            if (ActionManager.isActionAvailable(mana,coins)){
                StartCoroutine("doAction");
            }
            else{
                GMS.finalizarTurno();
            }
        }
    }

    IEnumerator doAction(){
        Action action = ActionManager.getAction(mana,coins);

        switch (action.getType())
        {
            case ActionTypes.BUILD_COLLECTOR:
                createCollectorAction();
                break;
            default:
                Debug.Log("Nada que hacer" + " Mana: " + mana + " Coins: " + coins);
            break;
        }
        yield return new WaitForSeconds(2.0f);
    }

    private bool isMyTurn(){
        if (GMS.isCurrentTeamIA(id)){
            return true;
        }
        else{
            return false;
        }
    }

    public void incMana(int value = 1){
        mana += value;
        if (mana > MANA_MAX){
            mana = MANA_MAX;
        }
    }

    public void decMana(int value = 1){
        mana -= value;
        Debug.Log("IA: " + "Gastando Maná: " + value);
        if (mana < 0){
            mana = 0;
        }
    }

    public void incCoins(int value = 1){
        coins += value;
    }

    public void decCoins(int value = 1){
        coins -= value;
    }

    public int getMana(){
        return mana;
    }

    public int getCoins(){
        return coins;
    }

    public void moveUnitAction(){
        Debug.Log("IA: " + "Move Unit Action");
        decMana();
    }

    public void createCollectorAction(){
        Debug.Log("IA: " + "Create Collector Action");
        Vector2Int pos = new Vector2Int(Random.Range(0,grid.ladoGridX-1), 0 );
        GameObject collector = Instantiate(Resources.Load("Prefabs/Collector"), grid.GetGlobalPosition(pos.x,pos.y), Quaternion.identity) as GameObject;
        collector.GetComponent<Unidad>().numJugador = id;
        decMana(ActionManager.getActionSpecifications(ActionTypes.BUILD_COLLECTOR).getManaCost());
        decCoins(ActionManager.getActionSpecifications(ActionTypes.BUILD_COLLECTOR).getCoinCost());
    }
}
