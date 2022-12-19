using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{


    [SerializeField] public int ancho, alto;
    [SerializeField] private Casilla casillaPrefab;
    [SerializeField] private GameObject obstaculoPrefab;

    private Dictionary<Vector2, Casilla> casillas;

    void Awake()
    {
        generaMapa();
    }

    void generaMapa()
    {
        casillas = new Dictionary<Vector2, Casilla>();

        for (int x = -ancho / 2; x < (float)ancho / 2 + 0.5; x++)
        {
            for (int y = -alto / 2; y < (float)alto / 2 + 0.5; y++)
            {
                var casillaSpawn = Instantiate(casillaPrefab, new Vector3(x, y), Quaternion.identity);
                casillaSpawn.mapaX = x;
                casillaSpawn.mapaY = y;
                casillaSpawn.Location = new Vector2Int(x+ancho/2, y+alto/2);
                if (Random.Range(0, 95) == 0 && casillaSpawn.esAccesible())
                {
                    var obstaculoSpawn = Instantiate(obstaculoPrefab, new Vector3(x, y), Quaternion.identity);
                }

                casillas[new Vector2(x, y)] = casillaSpawn;
            }
        }
    }


    public List<Casilla> GetCasillasVisibles(Casilla casilla,int vision)
    {
        List<Casilla> result = new List<Casilla>();

        if (casilla == null){
            return result;
        }
        
        if( vision == 0){
            result.Add(casilla);
            return result;
        }

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {            
                if (Mathf.Abs(x) - Mathf.Abs(y) == 0)
                    continue;
                else
                    { 
                    Casilla newCasilla = encontrarCasillaPos(new Vector2(casilla.mapaX+x, casilla.mapaY + y));
                    if(newCasilla ==null)
                    {
                        continue;
                    }
                    int verX = newCasilla.mapaX ;
                    int verY = newCasilla.mapaY ;

                    if ( newCasilla.esAccesible())
                    {
                        newCasilla.highLight();
                        result.AddRange(GetCasillasVisibles(newCasilla, vision - 1));
                    }

                }
            }
        }

        return result;
        
    }

        public Casilla encontrarCasillaPos(Vector2 pos)
        {
            if (casillas.TryGetValue(pos, out var casilla)) {
                return casilla;
            }
        return null;

        }

        public Casilla encontrarCasillaLocation(Vector2Int location)
        {
            foreach (KeyValuePair<Vector2,Casilla> item in casillas)
            {
                if (item.Value.Location == location){
                    return item.Value;
                }
            }

            return null;
        }


}
