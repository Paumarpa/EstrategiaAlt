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
    public int dañoAtaque;
    //public int dañoDefensa;
    public int armadura;
    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();

    }

    private void OnMouseDown()
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

    void Atacar(Unidad enemigo)
    {
        haAtacado = true;

        int daño = dañoAtaque - enemigo.armadura;
        //miDaño = enemigo.dañoDefensa - armadura;

        if(dañoAtaque >= 1)
        {
            enemigo.vida -= dañoAtaque;
        }

        /*
         if(miDaño >= 1)
        {
            vida -= miDaño;
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

        Casilla[] casillas = FindObjectsOfType<Casilla>();
        foreach (Casilla casilla in casillas)
        {
            //ahora en valor absoluto, falta a*
            if (Mathf.Abs(transform.position.x - casilla.transform.position.x) + Mathf.Abs(transform.position.y - casilla.transform.position.y) <= velocidad)
            {
                if (casilla.esAccesible() == true)
                {
                    casilla.highLight();
                }
            }
        }
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
        StartCoroutine(EMover(objetivo));
    }

    //no por camino optimo, movimiento linear
    IEnumerator EMover(Vector2 pos)
    {
        while (transform.position.x != pos.x ){
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(pos.x, transform.position.y), velocidad * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y != pos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, pos.y), velocidad * Time.deltaTime);
            yield return null;
        }
        seHaMovido = true;
        ResetIconosArmas();
        GetEnemigos();
    }
}
