using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrIA : MonoBehaviour
{
    public int getNum(string type){

        int resultado = 0;

        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == type){
                resultado++;
            }
        }

        return resultado;
    }

    public List<GameObject> getGameObjects(string type){
        List<GameObject> result = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == type){
                result.Add(child.gameObject);
            }
        }

        return result;
    }
}