using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStatsManager : MonoBehaviour
{
    public IAManager player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int mana = player.getMana();
        int coins = player.getCoins();

        string texto = "Mana: " + mana + " Coins: " + coins;
        gameObject.GetComponent<TMP_Text>().SetText(texto);
    }
}
