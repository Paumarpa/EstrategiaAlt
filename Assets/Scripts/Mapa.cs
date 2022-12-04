using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{


    [SerializeField] private int ancho, alto;
    [SerializeField] private Casilla casillaPrefab;
    [SerializeField] private GameObject obstaculoPrefab;

    void Start()
    {
        generaMapa();
    }

   void generaMapa()
    {
        for(int x = -ancho/2; x<(float)ancho/2 +0.5; x++)
        {
            for (int y = -alto / 2; y < (float)alto / 2 + 0.5; y++)
            {
                var casillaSpawn = Instantiate(casillaPrefab, new Vector3(x, y), Quaternion.identity);
                if (Random.Range(0, 5) == 0 && casillaSpawn.esAccesible())
                {
                    var obstaculoSpawn = Instantiate(obstaculoPrefab, new Vector3(x, y), Quaternion.identity);
                }
            }
        }
    }
}
