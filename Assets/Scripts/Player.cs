using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /**
    *  Se necesita: prefab que puedas mover, comprobar si esta puesto, comprobar si esta ocupada (accesible), quitar mana y oro
    *  Para mostrar prefab: se instancia una construcción cuando se da click en un boton de construcción
    *  Tiene que tener algunas de las construcciones un icono de candado cuando no se pueda construir y estén coloradas en negro
    *  Tiene que tener todas las unidades una forma de poner en gris las que no se puedan comprar
    */
    public static Player current;
    GameMaster gm;
    private Casilla[] casillas;
    private Mapa mapa;
    Vector2 MousePos;
    GameObject building;
    bool isBuilding = false;
    Grid grid;
    void Start()
    {
        grid = GameObject.Find("Pathfinding").GetComponent<Grid>();
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        print(MousePos);
    }

    // SOLAMENTE y SOLAMENTE para los botones

    public void InstanciateBuilding(GameObject TypeBuilding){
        if((TypeBuilding.name == "Barracks" && GameObject.Find("GameMaster").GetComponent<PlayerManager>().LOCKED_BARRACKS == false) ||
            (TypeBuilding.name == "Tower" && GameObject.Find("GameMaster").GetComponent<PlayerManager>().LOCKED_TOWER == false) || (TypeBuilding.name != "Barracks" && TypeBuilding.name != "Tower") ){
            building = Instantiate(TypeBuilding, new Vector3(0,0,1.0f), Quaternion.identity) as GameObject;
            building.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 1.0f );
            building.GetComponent<Player>().isBuilding = true;
        }
        else{
            Debug.Log("Nope");
        }
        //Cambiar de color celdas mas cercanas a partir del castillo  --> booleano true

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate(){
        if(isBuilding){
        
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 1.0f );
            if(Input.GetMouseButton(0)){
                isBuilding = false;
                string tag = gameObject.tag;
                Debug.Log("TAG: "+gameObject.tag);
                if(gameObject.tag == "Collector"){
                        GameObject.Find("GameMaster").GetComponent<PlayerManager>().LOCKED_BARRACKS = false;
                        GameObject.Find("GameMaster").GetComponent<PlayerManager>().Unlock("Barrack");
                        GameObject.Find("GameMaster").GetComponent<PlayerManager>().Unlock("Warrior");
                        Debug.Log("UNLOCKED Barracks");
                }
                else if(gameObject.tag == "Barracks"){
                    GameObject.Find("GameMaster").GetComponent<PlayerManager>().LOCKED_TOWER = false;
                    GameObject.Find("GameMaster").GetComponent<PlayerManager>().Unlock("Tower");
                    Debug.Log("UNLOCKED Tower");
                }
                // Colocar BUILD y cambiar a color normal --> booleano false
                Destroy(this.gameObject);
            }
        }
    }
}
