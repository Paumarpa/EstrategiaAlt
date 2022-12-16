using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    public int turnos = 1;
    public GameMaster GMS;
    public int id = 2;
    const float SIMULATION_STEP = 0.25f;

    private Grid grid;

    private Vector2Int townHallLocation;

    public const int COINS_BY_COLLECTOR =  50;

    private bool strategyDecided = false;
    private bool townHallCreated = false;

    private bool enemyTownHallDiscovered = false;

    private Vector2Int enemyTownHallLocation;

    private bool working = false;
    public Strategy strategy;

    public PlayerOrIA enemy;
    private PlayerOrIA myUnits;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Pathfinding").GetComponent<Grid>();
        myUnits = gameObject.GetComponent<PlayerOrIA>();
        StartCoroutine("createTownHall");
    }

    void FixedUpdate()
    {
        if (isMyTurn()){
            if (!strategyDecided){
                DecideStrategy();
                if (strategy.isActionAvailable()){
                    StartCoroutine("doAction");
                }
                else{
                    FinalizarTurno();
                }
            }
        }
    }

    void DecideStrategy(){
        int collectors = myUnits.getNum("Collector");
        int towers = myUnits.getNum("Tower");
        int barracks = myUnits.getNum("Barracks");
        int units = myUnits.getNum("Unit");
        bool enemyDiscovered = IsEnemyDiscovered();
        bool enemyNearBase = IsEnemyDiscoveredNearOurBase();
        
        if (!enemyTownHallDiscovered){
            enemyTownHallDiscovered = isEnemyTownHallDiscovered();
            if (enemyTownHallDiscovered){
                enemyTownHallLocation = getEnemyTownHallDiscovered();
            }
        }

        strategy = StrategyManager.getStrategy(collectors,towers,barracks,units, enemyDiscovered, enemyTownHallDiscovered, enemyNearBase);
        strategy.setCoins(myUnits.getCoins());
        strategy.setMana(myUnits.getMana());
        strategy.planActions(myUnits,enemy);
        strategyDecided = true;
    }

    void FinalizarTurno()
    { 
        StopCoroutine("doAction");
        UpdateResourcesNextTurn();
        GMS.finalizarTurno();
        strategyDecided = false;
    }

    void UpdateResourcesNextTurn(){
        turnos++;
        myUnits.SetMana(turnos);

        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Collector"){
                myUnits.incCoins(COINS_BY_COLLECTOR);
            }
        }
    }

    IEnumerator doAction(){
        
        while (strategy.isActionAvailable()){
            if(!working){
                Action action = strategy.getNextAction();
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
                    case ActionTypes.CREATE_UNIT:
                        createUnitAction(action);
                        break;
                    case ActionTypes.MOVE_UNIT:
                        moveUnitAction(action);
                        break;
                    case ActionTypes.ATTACK_UNIT:
                        attackUnitAction(action);
                        break;
                    default:
                        Debug.Log("Nada que hacer" + " Mana: " + myUnits.getMana() + " Coins: " + myUnits.getCoins());
                        break;
                }
            }

            yield return new WaitForSeconds(SIMULATION_STEP);
        }

        if (!strategy.isActionAvailable()){
            FinalizarTurno();
        }
    }

    private bool isMyTurn(){
        if (GMS.isCurrentTeamIA(id) && townHallCreated){
            return true;
        }
        else{
            return false;
        }
    }

    public Strategy getStrategy(){
        return strategy;
    }

    public void moveUnitAction(Action action){
        Unidad unidad = action.gameObject.GetComponent<Unidad>();
        unidad.OnMouseDownIA();
        if(action.target == null){
            unidad.MoveToExploreIA(myUnits,this.townHallLocation);
        }else{
            unidad.MoveToAttackIA(action.target);
        }
        
        working = true;
        StartCoroutine(WaitForFinishMovement(unidad));
    }

    public void attackUnitAction(Action action){
        Unidad unidad = action.gameObject.GetComponent<Unidad>();
        unidad.OnMouseDownIA();
        if(action.target != null){
            unidad.AttackIA(action.target);
        }
    }

    IEnumerator WaitForFinishMovement(Unidad unidad){
        while(unidad.ispathfinding){
            yield return new WaitForSeconds(0.25f);
        }
        working = false;
        yield return null;
    }

    public void createBuildingAction(Action action){

        string resource;
        Vector2Int location;

        switch (action.getType())
        {
            case ActionTypes.BUILD_COLLECTOR:
                resource = "Prefabs/Collector";
                location = getSlotNearTownHall("Collector",1);
                break;
            case ActionTypes.BUILD_TOWER:
                resource = "Prefabs/Tower";
                location = getSlotNearTownHall("Tower",2);
                break;
            case ActionTypes.BUILD_BARRACKS:
                resource = "Prefabs/Barracks";
                location = getSlotNearTownHall("Barracks",1);
                break;
            default:
                return;
        }

        if (isValidLocation(location)){
            GameObject building = Instantiate(Resources.Load(resource), grid.GetGlobalPosition(location.x,location.y), Quaternion.identity) as GameObject;
            building.transform.parent = transform;
            building.GetComponent<Unidad>().numJugador = id;
            building.GetComponent<Unidad>().Location = location;
            myUnits.decMana(ActionManager.getActionSpecifications(action.getType()).getManaCost());
            myUnits.decCoins(ActionManager.getActionSpecifications(action.getType()).getCoinCost());
            grid.grid[location.x,location.y].accesible = false;
            
        }else{
            Debug.Log("IA: " + "Imposible construir");
        }
    }

    public void createUnitAction(Action action){

        string resource = "Prefabs/Warrior";

        Vector2Int location = getSlotNearTownHall("Unit",3);

        if (isValidLocation(location)){
            GameObject unit = Instantiate(Resources.Load(resource), grid.GetGlobalPosition(location.x,location.y), Quaternion.identity) as GameObject;
            unit.transform.parent = transform;
            if(id == 2){
                unit.GetComponent<SpriteRenderer>().color = new Color(0,0,1,1);
            }
            unit.GetComponent<Unidad>().numJugador = id;
            unit.GetComponent<Unidad>().Location = location;
            myUnits.decMana(ActionManager.getActionSpecifications(action.getType()).getManaCost());
            myUnits.decCoins(ActionManager.getActionSpecifications(action.getType()).getCoinCost());
            grid.grid[location.x,location.y].accesible = false;
            
        }else{
            Debug.Log("IA: " + "Imposible construir unidad");
        }
    }

    IEnumerator createTownHall()
    {
        Vector2Int location;

        while (!grid.gridCreated){
            yield return new WaitForSeconds(0.1f);
        }

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

        townHallCreated = true;
        //cleanTownHallUbication(TH1);
        //cleanTownHallUbication(TH2);
    }

    Vector2Int getSlotNearTownHall(string type, int minDistance = 1){
        int radius = 1;

        while (radius < 7){
            for (int i = townHallLocation.x - radius; i <= townHallLocation.x + radius; i++)
            {
                for (int j = townHallLocation.y - radius; j <= townHallLocation.y + radius; j++)
                {
                    if (isValidLocation(i,j) && !isNearOtherBuilding(new Vector2Int(i,j),minDistance)){
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

    public bool IsEnemyDiscovered(){

        foreach (GameObject unidad in myUnits.getGameObjects("Tower")){
            if (unidad.GetComponent<Unidad>().GetEnemigosEnRango(enemy).Count > 0){
                return true;
            }
        }

        foreach (GameObject unidad in myUnits.getGameObjects("Unit")){
            if (unidad.GetComponent<Unidad>().GetEnemigosEnRango(enemy).Count > 0){
                return true;
            }
        }

        return false;
    }

    public bool IsEnemyDiscoveredNearOurBase(){

        foreach (GameObject unidad in myUnits.getGameObjects("Tower")){
            if (unidad.GetComponent<Unidad>().GetEnemigosEnRango(enemy).Count > 0){
                return true;
            }
        }

        foreach (GameObject unidad in myUnits.getGameObjects("Collector")){
            if (unidad.GetComponent<Unidad>().GetEnemigosEnRango(enemy).Count > 0){
                return true;
            }
        }

        foreach (GameObject unidad in myUnits.getGameObjects("Barracks")){
            if (unidad.GetComponent<Unidad>().GetEnemigosEnRango(enemy).Count > 0){
                return true;
            }
        }

        foreach (GameObject unidad in myUnits.getGameObjects("TownHall")){
            if (unidad.GetComponent<Unidad>().GetEnemigosEnRango(enemy).Count > 0){
                return true;
            }
        }

        return false;
    }

    public bool isEnemyTownHallDiscovered(){
        foreach (GameObject unidad in myUnits.getGameObjects("Unit")){
            if (unidad.GetComponent<Unidad>().GetBuildingsInRange(enemy).Count > 0){
                return true;
            }
        }
        return false;
    }
    public Vector2Int getEnemyTownHallDiscovered(){
        return enemy.getGameObjects("TownHall")[0].GetComponent<Unidad>().Location;
    }

    public bool isNearOtherBuilding(Vector2Int newLocation, int minDistance){

        List<GameObject> lista =  myUnits.getGameObjects("Tower");
        lista.AddRange(myUnits.getGameObjects("Barracks"));
        lista.AddRange(myUnits.getGameObjects("Collector"));
        lista.AddRange(myUnits.getGameObjects("TownHall"));

        foreach(GameObject item in lista){
            Vector2Int location = item.GetComponent<Unidad>().Location;
            if (Vector2Int.Distance(location,newLocation) < minDistance){
                return true;
            }
        }

        return false;
    }
}
