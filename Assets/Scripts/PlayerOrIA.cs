using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrIA : MonoBehaviour
{
    const int MANA_MAX = 15;
    [SerializeField] private bool human = false;
    public IAManager IA;
    public int mana = 1;
    public int coins = 50;

    void Start(){

        IA = gameObject.GetComponent<IAManager>();
        if (IA != null){
            human = false;
        }else{
            human = true;
        }

    }
    public int getNum(string type){

        int resultado = 0;

        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == type){
                resultado++;
            }
        }

        return resultado;
    }

    public List<GameObject> getGameObjects(string type){
        List<GameObject> result = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == type){
                result.Add(child.gameObject);
            }
        }

        return result;
    }

    public Strategy getStrategy(){
        if(human){
            return new Strategy(StrategyTypes.HUMAN);
        }else{
            return IA.getStrategy();
        }
    }

    public void SetMana(int value){
        mana = value;
        if (mana > MANA_MAX){
            mana = MANA_MAX;
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

    public GameObject GetUnitNearEnemyUnit(List<GameObject> myUnits, PlayerOrIA enemy){
        foreach (GameObject unit in myUnits){
            if (unit.GetComponent<Unidad>().GetEnemigosEnRango(enemy).Count > 0){
                return unit;
            }
        }

        return null;
    }

    public GameObject GetUnitNearEnemyBuilding(List<GameObject> myUnits, PlayerOrIA enemy){
        foreach (GameObject unit in myUnits){
            if (unit.GetComponent<Unidad>().GetBuildingsInRange(enemy).Count > 0){
                return unit;
            }
        }

        return null;
    }
}