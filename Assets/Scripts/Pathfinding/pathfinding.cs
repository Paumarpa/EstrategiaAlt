using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class pathfinding : MonoBehaviour
{

    public Transform seeker, target;
    Grid grid;
    //encontrarCamino encontrarC;

    private void Awake()
    {
        //encontrarC = GetComponent<encontrarCamino>();
        grid = GetComponent<Grid>();
    }
    private void Update()
    {
        findPath(seeker.position, target.position);
    }

    /*public void StartFindPath(Vector3 inicio, Vector3 fin)
    {
        StartCoroutine(findPath(inicio, fin));
    }*/

    void findPath(Vector3 origen,Vector3 destino)
    {
        //Vector3[] waypoints = new Vector3[0];
        //bool exito = false;

        Nodo nodoOrigen = grid.NodoDesdeCoordenadaGlobal(origen);
        Nodo nodoDestino = grid.NodoDesdeCoordenadaGlobal(destino);

        //List<Nodo>openSet = new List<Nodo>();
        Monticulo<Nodo> openSet = new Monticulo<Nodo>(grid.maxSize);
        HashSet<Nodo> closedSer = new HashSet<Nodo>();

        openSet.Add(nodoOrigen);


        while(openSet.Count > 0)
        {
            Nodo actual = openSet.EliminaItem();
            /*Nodo actual = openSet[0];

            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].costeF < actual.costeF || openSet[i].costeF == actual.costeF && openSet[i].costeH < actual.costeH)
                {
                    actual = openSet[i];
                }
            }

            openSet.Remove(actual);*/

            closedSer.Add(actual);

            if(actual == nodoDestino)
            {

                //exito = true;
                devolverCamino(nodoOrigen, nodoDestino);
                break;
            }

            foreach(Nodo vecino in grid.GetNodosVecinos(actual))
            {
                if(!vecino.accesible || closedSer.Contains(vecino))
                {
                    continue;
                }

                int nuevoCosteAVecino = actual.costeG + DevolverDistancia(actual, vecino);
                if(nuevoCosteAVecino < vecino.costeG || !openSet.Contains(vecino))
                {
                    vecino.costeG = nuevoCosteAVecino;
                    vecino.costeH = DevolverDistancia(vecino, nodoDestino);
                    vecino.padre = actual;


                    if (!openSet.Contains(vecino))
                        openSet.Add(vecino);
                    else
                    {
                        openSet.update(vecino);
                    }
                }
            }
        }
        //yield return null;
        /*
        if (exito)
        {
             waypoints = devolverCamino(nodoOrigen, nodoDestino);
            return waypoints;
        }
       
        return null; */
        //encontrarC.caminoProcesado(waypoints, exito);
    }

    void devolverCamino(Nodo inicio, Nodo fin)
    {
        List<Nodo> camino = new List<Nodo>();
        Nodo actual = fin;

        while(actual != inicio)
        {
            camino.Add(actual);
            actual = actual.padre;
        }
        camino.Add(inicio);
        //Vector3[] waypoints = simplificarCamino(camino);
        
        camino.Reverse();
        //Array.Reverse(waypoints);
        //return waypoints;
        grid.camino = camino;
    }

    Vector3[] simplificarCamino(List<Nodo> camino)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 dirAntigua = Vector2.zero;


        for (int i= 1; i< camino.Count; i++)
        {
            Vector2 dirNueva = new Vector2(camino[i - 1].gridX - camino[i].gridX, camino[i - 1].gridY - camino[i].gridY);
            if(dirAntigua != dirNueva)
            {
                waypoints.Add(camino[i].posGlobal);
                
            }
            dirAntigua = dirNueva;
        }
        return waypoints.ToArray();
    }


    int DevolverDistancia(Nodo a, Nodo b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if (distX > distY)
            return 14 *distY +10*(distX-distY);

        return 14 * distX + 10 * (distY - distX);


    }

}
