using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Unidad unidadSeleccionada;

    public int turno = 1;

    private Grid grid;
    private Mapa mapa;

    [SerializeField] private GameObject P1;
    [SerializeField] private GameObject P2;

    private Vector2Int TH1,TH2;

    private void Start(){

        grid = GameObject.Find("Pathfinding").GetComponent<Grid>();
        mapa = GameObject.Find("Mapa").GetComponent<Mapa>();

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
}
