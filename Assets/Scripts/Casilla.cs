using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{

    private SpriteRenderer rend;
    public Sprite[] tileGraphics;
    public LayerMask inaccesible;
    public float tamanoCasilla;
    
    

    public Color colorHighlight;

    public bool accesible;
    public int mapaX;
    public int mapaY;

    public Vector2Int Location;
    GameMaster gm;


    
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, tileGraphics.Length);
        rend.sprite = tileGraphics[rand];
        
        
        gm = FindObjectOfType<GameMaster>();
    }


     void OnMouseEnter()
    {
        if (esAccesible())
        {
            transform.localScale += Vector3.one * tamanoCasilla;

        }
        
    }

     void OnMouseExit()
    {
        if (esAccesible())
        {
            transform.localScale -= Vector3.one * tamanoCasilla;
        }
    }

    public bool esAccesible()
    {
        Collider2D obstaculo = Physics2D.OverlapCircle(transform.position, 0.2f, inaccesible);
        if (obstaculo != null)
        {
            return false;
        }
        else
            return true;
    }

    public void highLight()
    {
        rend.color = Color.black;
        accesible = true;
    }

    public void Reset()
    {
        rend.color = Color.white;
        accesible = false;
    }
    public void OnMouseDown()
    {
        if (accesible && gm.unidadSeleccionada != null)
        {
            gm.unidadSeleccionada.Mover(this.transform.position);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
