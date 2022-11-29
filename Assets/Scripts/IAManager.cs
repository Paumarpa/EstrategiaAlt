using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    public int mana = 1;
    public int coins = 50;

    public int turnos = 1;
    public GameMaster GMS;
    public int id = 2;

    public ActionTypes currentAction;
    public StrategyTypes currentStrategy;

    const int MANA_MAX = 15;

    private Grid grid;

    private Vector2Int townHallLocation;

    public const int COINS_BY_COLLECTOR =  50;

    private bool strategyDecided = false;
    private Strategy strategy;

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
            if (!strategyDecided){
                DecideStrategy();
            }
            Debug.Log("Turno id: " + id);
            if (strategy.isActionAvailable(mana,coins)){
                StartCoroutine("doAction");
            }
            else
            {
                FinalizarTurno();
            }
        }
    }

    void DecideStrategy(){
        int collectors = getNum("Collector");
        int towers = getNum("Tower");
        int barracks = getNum("Barracks");
        int units = getNum("Unit");
        strategy = StrategyManager.getStrategy(collectors,towers,barracks,units, false, false);
        strategyDecided = true;
        Debug.Log("IA Strategy " + strategy);
    }

    private void FinalizarTurno()
    {
        GMS.finalizarTurno();
        Debug.Log("Fin Turno id: " + id);
        UpdateResourcesNextTurn();
        strategyDecided = false;
    }

    void UpdateResourcesNextTurn(){
        turnos++;
        incMana(turnos);

        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Collector"){
                incCoins(COINS_BY_COLLECTOR);
            }
        }
    }

    IEnumerator doAction(){
        Action action = strategy.getAction(mana,coins);

        Debug.Log("doAction: " + action);

        switch (action.getType())
        {
            case ActionTypes.BUILD_COLLECTOR:
                createBuildingAction(action);
                break;
            case ActionTypes.BUILD_TOWER:
                createBuildingAction(action);
                break;
            case ActionTypes.BUILD_BARRACKS:
                createBuildingAction(action);
                break;
            default:
                Debug.Log("Nada que hacer" + " Mana: " + mana + " Coins: " + coins);
                FinalizarTurno();
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

    public void createBuildingAction(Action action){
        Debug.Log("IA Create Building: " + action);

        string resource;

        switch (action.getType())
        {
            case ActionTypes.BUILD_COLLECTOR:
                resource = "Prefabs/Collector";
                break;
            case ActionTypes.BUILD_TOWER:
                resource = "Prefabs/Tower";
                break;
            case ActionTypes.BUILD_BARRACKS:
                resource = "Prefabs/Barracks";
                break;
            default:
                return;
        }

        Vector2Int location = getSlotNearTownHall();

        if (isValidLocation(location)){
            GameObject building = Instantiate(Resources.Load(resource), grid.GetGlobalPosition(location.x,location.y), Quaternion.identity) as GameObject;
            building.transform.parent = transform;
            building.GetComponent<Unidad>().numJugador = id;
            building.GetComponent<Unidad>().Location = location;
            decMana(ActionManager.getActionSpecifications(action.getType()).getManaCost());
            decCoins(ActionManager.getActionSpecifications(action.getType()).getCoinCost());
            grid.grid[location.x,location.y].accesible = false;
            
        }else{
            Debug.Log("IA: " + "Imposible construir");
            FinalizarTurno();
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

        grid.grid[location.x,location.y].accesible = false;
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

    int getNum(string type){

        int resultado = 0;

        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Collector"){
                resultado++;
            }
        }

        return resultado;
    }

}
