using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    public int mana = 1;
    public int coins = 0;
    public GameMaster GMS;
    public int id = 2;

    public ActionTypes currentAction;
    public StrategyTypes currentStrategy;

    // Start is called before the first frame update
    void Start()
    {
        currentAction = ActionTypes.NONE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMyTurn()){
            if (isActionAvailable()){
                doAction();
            }
            else{
                GMS.finalizarTurno();
                Debug.Log("IA: " + "Fin de mi turno.");
            }
        }
    }

    private bool isActionAvailable(){ //TODO No está bien definida
        if (mana > 0){
            return true;
        }

        return false;
    }

    private void doAction(){
        moveUnitAction();
    }

    private bool isMyTurn(id){
        if (GMS.isCurrentTeamIA()){
            return true;
        }
        else{
            return false;
        }
    }

    public void incMana(int value = 1){
        mana += value;
    }

    public void decMana(int value = 1){
        mana -= value;
        Debug.Log("IA: " + "Gastando Maná: " + value);
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
}
