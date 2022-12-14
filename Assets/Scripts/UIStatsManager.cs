using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStatsManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private PlayerOrIA p1;
    private PlayerOrIA p2;

    public GameMaster master;

    public TMP_Text player1TextCoins;
    public TMP_Text player1TextMana;
    public TMP_Text player1TextActions;

    public TMP_Text player2TextCoins;
    public TMP_Text player2TextMana;
    public TMP_Text player2TextActions;


    // Start is called before the first frame update
    void Start()
    {
        p1 = player1.GetComponent<PlayerOrIA>();
        p2 = player2.GetComponent<PlayerOrIA>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player1TextMana.SetText(p1.getMana().ToString());
        player1TextCoins.SetText(p1.getCoins().ToString());
        if(p1.getStrategy() != null){
            player1TextActions.SetText(p1.getStrategy().getType().ToString());
        }

        player2TextMana.SetText(p2.getMana().ToString());
        player2TextCoins.SetText(p2.getCoins().ToString());
        if(p2.getStrategy() != null){
            player2TextActions.SetText(p2.getStrategy().getType().ToString());
        }
    }
}
