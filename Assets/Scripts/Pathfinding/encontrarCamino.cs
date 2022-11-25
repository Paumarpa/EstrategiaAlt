using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class encontrarCamino : MonoBehaviour
{
    Queue<caminos> colaCaminos = new Queue<caminos>();
    caminos caminoActual;

    static encontrarCamino instance;
    pathfinding _pathfinding;

    bool procesando;
    private void Awake()
    {
        instance = this;
        _pathfinding = GetComponent<pathfinding>();

    }
    public static void pedirCamino(Vector3 inicio, Vector3 fin, Action<Vector3[], bool> callback)
    {
        caminos caminoNuevo = new caminos(inicio, fin, callback);
        instance.colaCaminos.Enqueue(caminoNuevo);
        instance.procesarSig();
    }

    void procesarSig()
    {
        if(!procesando && colaCaminos.Count > 0)
        {
            caminoActual = colaCaminos.Dequeue();
            procesando = true;
            _pathfinding.StartFindPath(caminoActual.inicio, caminoActual.fin);
        }
    }
    public void caminoProcesado(Vector3[] cami, bool exito)
    {
        caminoActual.callback(cami, exito);
        procesando = false;
        procesarSig();
    }

    struct caminos
    {
        public Vector3 inicio;
        public Vector3 fin;
        public Action<Vector3[], bool> callback;

        public caminos(Vector3 _inicio, Vector3 _fin, Action<Vector3[], bool> _callback)
        {

            inicio = _inicio;
            fin = _fin;
            callback = _callback;

        }
    }
}
