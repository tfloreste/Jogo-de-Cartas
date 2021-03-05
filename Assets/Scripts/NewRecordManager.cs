using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRecordManager : MonoBehaviour
{

    public Text newRecordTentativas;
    // Start is called before the first frame update
    void Start()
    {
        newRecordTentativas.text = "Tentativas: " + PlayerPrefs.GetInt("Record", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
