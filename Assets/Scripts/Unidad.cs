using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unidad : MonoBehaviour
{
    //seleccion de unidad
    public bool seleccionado;
    GameMaster gm;

    //movimiento
    public int velocidad;
    public bool seHaMovido;

    //equipo
    public int numJugador;

    //ataque 
    public int rangoAtaque;
    List<Unidad> enemigosEnRango = new List<Unidad>();
    public bool haAtacado;//solo puede atacar 1 vez por turno
    public GameObject armaIcono;

    //stats
    public int vida;
    public int danoAtaque;
    //public int danoDefensa;
    public int armadura;

    //pathfinding
    Vector3[] camino;
    private int indiceObj;
    public bool ispathfinding = false;
    encontrarCamino encontrarC;

    public Vector2Int Location;

    private Casilla[] casillas;

    private Mapa mapa;
    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        casillas = FindObjectsOfType<Casilla>();
        mapa = FindObjectOfType<Mapa>();

    }

    public void OnMouseDown()
    {
        ResetIconosArmas();
        if(seleccionado == true)
        {
            seleccionado = false;
            gm.unidadSeleccionada = null;
            gm.resetCasillas();
        }
        else
        {
            if (numJugador == gm.turno)
            {
                if(gm.unidadSeleccionada != null)
                {
                    gm.unidadSeleccionada.seleccionado = false;
                }
                gm.resetCasillas();
                  gm.unidadSeleccionada = this;
                seleccionado = true;
                
                GetWalkableTiles();
                GetEnemigos();
        
            }
            
        }

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        Unidad unidad = col.GetComponent<Unidad>();
        if(gm.unidadSeleccionada != null)
        {
            if(gm.unidadSeleccionada.enemigosEnRango.Contains(unidad) && gm.unidadSeleccionada.haAtacado == false)
            {
                gm.unidadSeleccionada.Atacar(unidad);
            }
        }
    }

    public void AttackIA(GameObject target){
        if (target != null){
            Unidad enemigo = target.GetComponent<Unidad>();
            if (this.enemigosEnRango.Contains(enemigo) && !this.haAtacado){
                Atacar(enemigo);
            }
        }
    }

    public void OnMouseDownIA()
    {
        ResetIconosArmas();
        if(seleccionado == true)
        {
            seleccionado = false;
            gm.unidadSeleccionada = null;
            gm.resetCasillas();
        }
        else
        {
            if (numJugador == gm.turno)
            {
                if(gm.unidadSeleccionada != null)
                {
                    gm.unidadSeleccionada.seleccionado = false;
                }
                gm.resetCasillas();
                  gm.unidadSeleccionada = this;
                seleccionado = true;
                
                GetWalkableTiles();
                GetEnemigos();
        
            }
            
        }
    }

    void Atacar(Unidad enemigo)
    {
        Debug.Log("atacando");
        haAtacado = true;

        int dano = danoAtaque - enemigo.armadura;
        //miDano = enemigo.danoDefensa - armadura;

        if(danoAtaque >= 1)
        {
            enemigo.vida -= danoAtaque;
        }

        /*
         if(miDano >= 1)
        {
            vida -= miDano;
        }
         */

        if(enemigo.vida <= 0)
        {
            Destroy(enemigo.gameObject);
            GetWalkableTiles();
        }

        /*
        if(vida <= 0)
        {
            gm.resetCasillas();
            Destroy(this.gameObject);
        }
        */
    }

    //por ahora anchura, falta por grafo
    void GetWalkableTiles()
    {
        if(seHaMovido == true)
        {
            return;
        }

        Vector2 pos = new Vector2(transform.position.x,transform.position.y);
        Casilla actual = mapa.encontrarCasillaPos(pos);
        mapa.GetCasillasVisibles(actual, velocidad);
        
    }

    void GetEnemigos()
    {
        enemigosEnRango.Clear();

        foreach (Unidad unidad in FindObjectsOfType<Unidad>())
        {
            if (Mathf.Abs(transform.position.x - unidad.transform.position.x) + Mathf.Abs(transform.position.y - unidad.transform.position.y) <= rangoAtaque)
            {
                if(unidad.numJugador != gm.turno && !haAtacado)
                {
                    enemigosEnRango.Add(unidad);
                    unidad.armaIcono.SetActive(true);
                }
            }
        }
    }
    public void ResetIconosArmas()
    {
        foreach(Unidad unidad in FindObjectsOfType<Unidad>())
        {
            unidad.armaIcono.SetActive(false);
        }
    }

    public void Mover(Vector2 objetivo)
    {
        gm.resetCasillas();
        ispathfinding = true;
        encontrarCamino.pedirCamino(transform.position, objetivo, OnCaminEnc);
        //StartCoroutine(EMover(objetivo));
    }

    public void MoveToExploreIA(Vector2Int townHallLocation){
        Vector2 pos = new Vector2(transform.position.x,transform.position.y);
        Casilla actual = mapa.encontrarCasillaPos(pos);
        List<Casilla> casillas = mapa.GetCasillasVisibles(actual, velocidad);
        float maxDistance;
        if (casillas.Count > 0){
            Casilla seleccionada = casillas[0];
            maxDistance = Vector2Int.Distance(townHallLocation, new Vector2Int(seleccionada.mapaX,seleccionada.mapaY));
            foreach (Casilla casilla in casillas)
            {
                float distance = Vector2Int.Distance(townHallLocation, new Vector2Int(casilla.mapaX,casilla.mapaY));
                if (distance >= maxDistance){
                    seleccionada = casilla;
                }
            }
            seleccionada.OnMouseDown();
        }
    }


    //Mover hacia el enemigo más cercano
   public void MoveToAttackIA(GameObject target){
        Vector2 pos = new Vector2(transform.position.x,transform.position.y);
        Casilla actual = mapa.encontrarCasillaPos(pos);
        List<Casilla> casillas = mapa.GetCasillasVisibles(actual, velocidad);
        float minDistance;
        if (casillas.Count > 0){
            Unidad enemigoSeleccionado = GetEnemigoMasCercano();
            Casilla seleccionada = casillas[0];
            minDistance = Vector2Int.Distance(enemigoSeleccionado.Location, new Vector2Int(seleccionada.mapaX,seleccionada.mapaY));
            foreach (Casilla casilla in casillas)
            {
                float distance = Vector2Int.Distance(enemigoSeleccionado.Location, new Vector2Int(casilla.mapaX,casilla.mapaY));
                if (distance <= minDistance){
                    seleccionada = casilla;
                }
            }
            seleccionada.OnMouseDown();
        }
    }

    private void OnCaminEnc(Vector3[] newCamino, bool exito)
    {
        if (exito)
        {
            camino = newCamino;
            indiceObj = 0;
            StopCoroutine("seguirCamino");
            StartCoroutine("seguirCamino");
        }
    }

    IEnumerator seguirCamino()
    {
        ispathfinding = true;
        Vector3 currentWaypoint = camino[0];
        while (ispathfinding)
        {
            if (transform.position == currentWaypoint)
            {
                indiceObj++;
                if (indiceObj >= camino.Length)
                {
                    ResetIconosArmas();
                    GetEnemigos();
                    seHaMovido = true;
                    ispathfinding = false;
                    yield break;
                }
                currentWaypoint = camino[indiceObj];
            }

            yield return null;
            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, velocidad * Time.deltaTime);


        }

    }

    public List<Unidad> GetEnemigosEnRango()
    {
        enemigosEnRango.Clear();

        foreach (Unidad unidad in FindObjectsOfType<Unidad>())
        {
            if (Mathf.Abs(transform.position.x - unidad.transform.position.x) + Mathf.Abs(transform.position.y - unidad.transform.position.y) <= rangoAtaque)
            {
                if(unidad.numJugador != gm.turno && !haAtacado)
                {
                    enemigosEnRango.Add(unidad);
                }
            }
        }

        return enemigosEnRango;
    }

    public Unidad GetEnemigoMasCercano()
    {
        List<Unidad> enemigos =  GetEnemigosEnRango();

        float minDistance;
        Unidad enemigoSeleccionado = null;

        if (enemigos.Count > 0){
            minDistance = Vector2Int.Distance(this.Location, enemigos[0].Location);

            foreach (Unidad enemigo in enemigos)
            {
                float distance = Vector2Int.Distance(this.Location, enemigo.Location);
                if (distance < minDistance ){
                    enemigoSeleccionado = enemigo;
                }
            }
        }

        return enemigoSeleccionado;
    }
}
