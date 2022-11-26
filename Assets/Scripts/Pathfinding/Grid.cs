using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public bool enseñarGrid;
    public Vector2 tamañoGrid;
    public float radioNodo;
    public LayerMask inaccesible;

    Nodo[,] grid;

    float diametroNodo;
    public int ladoGridX, ladoGridY;

    
    void Awake()
    {
        diametroNodo = radioNodo * 2;
        ladoGridX = Mathf.RoundToInt(tamañoGrid.x / diametroNodo);
        ladoGridY = Mathf.RoundToInt(tamañoGrid.y / diametroNodo);
        crearGrid();
    }

    
    public int maxSize
    {
        get{
            return ladoGridX * ladoGridY;
        }
    }
    void crearGrid()
    {

        grid = new Nodo[ladoGridX, ladoGridY];
        Vector2 esquinaInferiorIzquierda = transform.position - (Vector3.right * tamañoGrid.x/2) - (Vector3.up * tamañoGrid.y/2);

        for (int x = 0; x< ladoGridX; x++){
            for (int y = 0; y < ladoGridY; y++){
                Vector2 puntoNodo = esquinaInferiorIzquierda + Vector2.right *(x * diametroNodo + radioNodo) + Vector2.up * (y * diametroNodo + radioNodo);
                bool accesible = esAccesible(puntoNodo);
                grid[x, y] = new Nodo(accesible, puntoNodo,x, y);

            }
        }

    }

    public bool esAccesible(Vector3 puntoNodo)
    {
        Collider2D obstaculo = Physics2D.OverlapCircle(puntoNodo, 0.2f, inaccesible);
        if (obstaculo != null)
        {
            return false;
        }
        else return true;
    }

    public Nodo NodoDesdeCoordenadaGlobal(Vector3 posGlobal)
    {
        float porcentajeX = (posGlobal.x + tamañoGrid.x / 2) / tamañoGrid.x;
        float porcentajeY = (posGlobal.y + tamañoGrid.y / 2) / tamañoGrid.y;
        porcentajeX = Mathf.Clamp01(porcentajeX);
        porcentajeY = Mathf.Clamp01(porcentajeY);

        int x = Mathf.RoundToInt((ladoGridX-1) * porcentajeX);
        int y = Mathf.RoundToInt((ladoGridY - 1) * porcentajeY);

        return grid[x, y];
    }

    public List<Nodo> GetNodosVecinos(Nodo nodo)
    {
        List<Nodo> vecinos = new List<Nodo>();

        for (int x =-1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (Mathf.Abs(x) - Mathf.Abs(y) == 0)
                    continue;

                int verX = nodo.gridX + x;
                int verY = nodo.gridY + y;

                if (verX>= 0 && verX <ladoGridX && verY >= 0 && verY < ladoGridY)
                {
                    vecinos.Add(grid[verX, verY]);
                }
            }
        }
        return vecinos;
    }

    //public List<Nodo> camino;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(tamañoGrid.x, tamañoGrid.y, 1));

        if(grid != null ){
            foreach (Nodo n in grid){
                Gizmos.color = (n.accesible)? Color.white : Color.red;
                /*if (camino != null)
                    if (camino.Contains(n))
                        Gizmos.color = Color.black;*/

                Gizmos.DrawCube(n.posGlobal, Vector3.one * (diametroNodo-.1f));
               // Debug.Log(n.posGlobal);
            }
        }
    }

    public Vector3 GetGlobalPosition(int x, int y){
        return grid[x,y].posGlobal;
    }
}