using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStatsManager : MonoBehaviour
{
    public IAManager player1;
    public IAManager player2;

    public GameMaster master;

    public TMP_Text player1Text;
    public TMP_Text player2Text;

    public TMP_Text turnText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string texto;

        int mana = player1.getMana();
        int coins = player1.getCoins();

        Strategy strategy = player1.getStrategy();

        if (strategy != null){
            texto = "Mana: " + mana + " Coins: " + coins + " " + strategy.getType();
        }else{
            texto = "Mana: " + mana + " Coins: " + coins;
        }

        player1Text.SetText(texto);

        mana = player2.getMana();
        coins = player2.getCoins();

        strategy = player2.getStrategy();

        if (strategy != null){
            texto = "Mana: " + mana + " Coins: " + coins + " " + strategy.getType();
        }else{
            texto = "Mana: " + mana + " Coins: " + coins;
        }

        player2Text.SetText(texto);

        turnText.SetText("Turno: " + master.turno);
    }
}
