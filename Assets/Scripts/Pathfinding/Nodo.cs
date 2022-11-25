using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo  : IHeapItem<Nodo> {

    public bool accesible;
    public Vector3 posGlobal;

    public int costeG;
    public int costeH;
    public int gridX;
    public int gridY;
    public Nodo padre;
    int heapIndex;
    public LayerMask inaccesible;

    public Nodo(bool acces,Vector3 _pos, int _gridX, int _gridY){
        accesible = acces;
        posGlobal = _pos;
        gridX = _gridX;
        gridY = _gridY;

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
    public int costeF {
        get {
            return costeG + costeH;
        } 
    }  

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Nodo nodoC)
    {
        int comparar = costeF.CompareTo(nodoC.costeF);
        if(comparar == 0)
        {
            comparar = costeH.CompareTo(nodoC.costeH);
        }
        return -comparar;
    }
}
