using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool tileRevelada = false;  // Indicador de carta virada ou não
    public Sprite originalCarta;        // Sprite da carta desejada
    public Sprite backCarta;            // Sprite do avesso da carta

    // Start is called before the first frame update
    void Start()
    {
        EsconderCarta();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        print("Voce pressionou em um tile");
        
        /*
        if(tileRevelada)
            EsconderCarta();
        else
            RevelaCarta();
        */

        GameObject.Find("gameManager").GetComponent<ManageCartas>().CartaSelecionada(gameObject);
    }

    public void EsconderCarta()
    {
        GetComponent<SpriteRenderer>().sprite = backCarta;
        tileRevelada = false;
    }

    public void RevelaCarta()
    {
        GetComponent<SpriteRenderer>().sprite = originalCarta;
        tileRevelada = true;
    }
    public void SetCartaOriginal(Sprite novaCarta)
    {
        originalCarta = novaCarta;
    }

}
