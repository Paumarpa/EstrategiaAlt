using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Unidad unidadSeleccionada;

    public int turno = 1;

    private Grid grid;

    private Vector2Int TH1,TH2;

    private void Start(){

        grid = GameObject.Find("Pathfinding").GetComponent<Grid>();
        generateTownHallUbications();
    }

    public void resetCasillas()
    {
        foreach(Casilla casilla in FindObjectsOfType<Casilla>())
        {
            casilla.Reset();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            finalizarTurno();
        }
    }

    public void finalizarTurno()
    {
        if (turno == 1)
        {
            turno = 2;
        }else if(turno== 2)
        {
            turno = 1;
        }

        if(unidadSeleccionada != null)
        {
            unidadSeleccionada.seleccionado = false;
            unidadSeleccionada = null;
        }

        resetCasillas();

        foreach (Unidad unidad in FindObjectsOfType<Unidad>())
        {
            unidad.seHaMovido = false;
            unidad.armaIcono.SetActive(false);
            unidad.haAtacado = false;
        }
    }

    public bool isCurrentTeamIA(int id){
        return turno == id;
    }

    public void generateTownHallUbications()
    {
        TH1 = new Vector2Int(Random.Range(0,grid.ladoGridX-1), 0 );
        TH2 = new Vector2Int(Random.Range(0,grid.ladoGridX-1), grid.ladoGridY -1 );
        
        GameObject th1GameObject = Instantiate(Resources.Load("Prefabs/TownHall"), grid.GetGlobalPosition(TH1.x,TH1.y), Quaternion.identity) as GameObject;
        GameObject th2GameObject = Instantiate(Resources.Load("Prefabs/TownHall"), grid.GetGlobalPosition(TH2.x,TH2.y), Quaternion.identity) as GameObject;
        
        th1GameObject.GetComponent<Unidad>().numJugador = 1;
        th1GameObject.GetComponent<Unidad>().numJugador = 2;
        //cleanTownHallUbication(TH1);
        //cleanTownHallUbication(TH2);
    }
}
