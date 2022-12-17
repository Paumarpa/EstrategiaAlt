using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public bool LOCKED_BARRACKS = true;
    public bool LOCKED_TOWER = true;
    GameObject[] buttons;
    void Start(){
        buttons = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject button in buttons)
        {
            for (int i = 0; i < button.transform.childCount; i++)
            {
                if(button.transform.GetChild(i).name == "bloqueado"){
                    Debug.Log("ENCONTRADO");
                    button.transform.GetChild(i).gameObject.SetActive(true);
                    button.GetComponent<Image>().color = Color.black;
                }
            }
        }
    }
    public void Unlock(string nameButton){
        foreach (GameObject button in buttons)
        {
            if(button.name == nameButton){
                for (int i = 0; i < button.transform.childCount; i++)
                {
                    if(button.transform.GetChild(i).name == "bloqueado"){
                        Debug.Log("ENCONTRADO");
                        button.transform.GetChild(i).gameObject.SetActive(false);
                        button.GetComponent<Image>().color = Color.white;
                    }
                }
            }
        }
    }
}
