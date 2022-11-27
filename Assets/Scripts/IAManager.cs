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

    private Vector2Int townHallLocation;

    // Start is called before the first frame update
    void Start()
    {
        currentAction = ActionTypes.NONE;
        currentStrategy = StrategyTypes.GROW;
        grid = GameObject.Find("Pathfinding").GetComponent<Grid>();
        createTownHall();
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
        //Vector2Int location = new Vector2Int(Random.Range(0,grid.ladoGridX-1), 0 );

        Vector2Int location = getSlotNearTownHall();

        if (isValidLocation(location)){
            GameObject collector = Instantiate(Resources.Load("Prefabs/Collector"), grid.GetGlobalPosition(location.x,location.y), Quaternion.identity) as GameObject;
            collector.transform.parent = transform;
            collector.GetComponent<Unidad>().numJugador = id;
            collector.GetComponent<Unidad>().Location = location;
            decMana(ActionManager.getActionSpecifications(ActionTypes.BUILD_COLLECTOR).getManaCost());
            decCoins(ActionManager.getActionSpecifications(ActionTypes.BUILD_COLLECTOR).getCoinCost());
        }else{
            Debug.Log("IA: " + "Imposible construir");
        }
    }

    public void createTownHall()
    {
        Vector2Int location;

        if (id == 1){
            location = new Vector2Int(Random.Range(0,grid.ladoGridX-1), 0 ); 
        }else{
            location = new Vector2Int(Random.Range(0,grid.ladoGridX-1), grid.ladoGridY -1 );
        }
        
        GameObject townhall = Instantiate(Resources.Load("Prefabs/TownHall"), grid.GetGlobalPosition(location.x,location.y), Quaternion.identity) as GameObject;        
        townhall.GetComponent<Unidad>().numJugador = id;
        townhall.GetComponent<Unidad>().Location = location;
        townhall.transform.parent = transform;
        townHallLocation = location;
        //cleanTownHallUbication(TH1);
        //cleanTownHallUbication(TH2);
    }

    Vector2Int getSlotNearTownHall(){
        int radius = 1;

        while (radius < 5){
            for (int i = townHallLocation.x - radius; i <= townHallLocation.x + radius; i++)
            {
                for (int j = townHallLocation.y - radius; j <= townHallLocation.y + radius; j++)
                {
                    if (isValidLocation(i,j)){
                        return new Vector2Int(i,j);
                    }
                }
            }
            radius++;
        }

        return new Vector2Int(-1,-1);
    }

    bool isValidLocation(int x, int y){
        if (x < 0 || y < 0 || x >= grid.ladoGridX || y >= grid.ladoGridY){
            return false;
        }

        return grid.grid[x,y].accesible;

    }

    bool isValidLocation(Vector2Int location){

        int x = location.x;
        int y = location.y;

        return isValidLocation(x,y);
    }
}
