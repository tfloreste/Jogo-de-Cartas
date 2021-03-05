using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ManageCartas : MonoBehaviour
{
    public GameObject carta;    // A carta a ser descoberta
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; // Indicadores para cada carta escolhida
    private GameObject carta1, carta2;          // Game objects das primeira e segunda cartas seleciobnadas
    private string linhaCarta1, linhaCarta2;    // Linha da carta selecionada

    bool timerAcionado;     // Indicador de timer acionado
    float timer;            // Variável para contagem de tempo

    int numTentativas = 0;  // Número de tentativas na rodada
    int numAcertos = 0;     // Número de acertos na rodada
    AudioSource somOk;      // Som a ser tocado ao acertar um match

    int record = 0;         // Guarda o menor número de tentativas de todos os jogos

    // Start is called before the first frame update
    void Start()
    {
        MostrarCartas();
        UpdateTentativas();

        somOk = GetComponent<AudioSource>();
        record = PlayerPrefs.GetInt("Record", 0);

        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "Record = " + record;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timerAcionado)
        {
            timer += Time.deltaTime;
            print(timer);

            if(timer > 1)
            {
                timerAcionado = false;

                if(carta1.tag == carta2.tag)
                {
                    Destroy(carta1);
                    Destroy(carta2);

                    somOk.Play();
                    numAcertos++;
                    if( numAcertos == 26)
                    {
                        int ultimoRecord = PlayerPrefs.GetInt("Record", 0);
                        if (ultimoRecord == 0 || numTentativas < ultimoRecord)
                        {
                            PlayerPrefs.SetInt("Record", numTentativas);
                            SceneManager.LoadScene("NewRecordScene");
                        }
                        else
                        {
                            SceneManager.LoadScene("EndGameScene");
                        }

                    }
                        
                }
                else
                {
                    carta1.GetComponent<Tile>().EsconderCarta();
                    carta2.GetComponent<Tile>().EsconderCarta();
                }

                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                timer = 0;
            }
        }
    }

    void MostrarCartas()
    {
        int[] arrayEmbaralhado = CriarArrayEmbaralhado();
        int[] arrayEmbaralhado2 = CriarArrayEmbaralhado();
        int[] arrayEmbaralhado3 = CriarArrayEmbaralhado();
        int[] arrayEmbaralhado4 = CriarArrayEmbaralhado();
        //Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity);
        for (int i = 0; i < 13; i++)
        {
            // AddUmaCarta(i);
            AddUmaCarta(0, i, arrayEmbaralhado[i], "clubs");
            AddUmaCarta(1, i, arrayEmbaralhado2[i],"hearts");
            AddUmaCarta(2, i, arrayEmbaralhado3[i],"spades");
            AddUmaCarta(3, i, arrayEmbaralhado4[i],"diamonds");
        }
            
    }

    void AddUmaCarta(int linha, int rank, int valor, string naipe)
    {
        GameObject centro = GameObject.Find("centroDaTela");
        float escalaCartaOriginalX = carta.transform.localScale.x;
        float escalaCartaOriginalY = carta.transform.localScale.y;

        float fatorEscalaX = (650*escalaCartaOriginalX)/100.0f;
        float fatorEscalaY = (945*escalaCartaOriginalY)/100.0f;

        //float x = centro.transform.position.x + (rank - 13/2)*2.0f;
        float x = centro.transform.position.x + (rank - 13/2)*fatorEscalaX;
        float y = centro.transform.position.y + (linha - 4/2)*fatorEscalaY;
        float z = centro.transform.position.z;

        Vector3 novaPosicao = new Vector3(x, y, z);
        //GameObject c = (GameObject)Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject c = (GameObject)Instantiate(carta, novaPosicao, Quaternion.identity);
        c.tag = "" + valor;
        c.name = linha + " " + valor;
        
        string nomeDaCarta = "";
        string numeroCarta = "";

        /* If/Else para array ordenado
        **
        if (rank == 0)
            numeroCarta = "ace";
        else if(rank == 10)
            numeroCarta = "jack";
        else if(rank == 11)
            numeroCarta = "queen";
        else if(rank == 12)
            numeroCarta = "king";
        else
            numeroCarta = "" + (rank+1);
        */
        if (valor == 0)
            numeroCarta = "ace";
        else if(valor == 10)
            numeroCarta = "jack";
        else if(valor == 11)
            numeroCarta = "queen";
        else if(valor == 12)
            numeroCarta = "king";
        else
            numeroCarta = "" + (valor+1);


        // nomeDaCarta = numeroCarta + "_of_clubs";
        nomeDaCarta = numeroCarta + "_of_" + naipe;

        Sprite s1 = (Sprite)Resources.Load<Sprite>(nomeDaCarta);
        print("s1: " + s1);

        c.GetComponent<Tile>().SetCartaOriginal(s1);
    }

    public int[] CriarArrayEmbaralhado()
    {
        int[] novoArray = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
        int temp;

        for (int t = 0; t < 13; t++)
        {
            temp = novoArray[t];
            int r = Random.Range(t, 13);
            novoArray[t] = novoArray[r];
            novoArray[r] = temp;
        }

        return novoArray;
    }

    public void CartaSelecionada(GameObject carta)
    {
        string linha = carta.name.Substring(0, 1);

        if (!primeiraCartaSelecionada)
        {    
            linhaCarta1 = linha;

            primeiraCartaSelecionada = true;
            carta1 = carta;
            carta1.GetComponent<Tile>().RevelaCarta();

        }
        else if(primeiraCartaSelecionada && !segundaCartaSelecionada && linhaCarta1 != linha)
        {
            linhaCarta2 = linha;

            segundaCartaSelecionada = true;
            carta2 = carta;
            carta2.GetComponent<Tile>().RevelaCarta();

            VerificaCartas();

        }
    }

    public void VerificaCartas()
    {
        DisparaTimer();

        numTentativas++;
        UpdateTentativas();
    }

    public void DisparaTimer()
    {
        timerAcionado = true;
    }

    void UpdateTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas = " + numTentativas;
    }
}
