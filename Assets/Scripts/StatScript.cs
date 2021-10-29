using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatScript : MonoBehaviour
{
    public GameObject selectedFish = null;
    public Text Name,Energy,Life;
    void Update(){
        if(selectedFish!=null){
            Fish fishScript = selectedFish.GetComponent<Fish>();
            UpdateTexts(selectedFish.name, fishScript.energy, fishScript.life);
        }
        else{
            UpdateTexts("none selected", 0, 0);
        }
    }
    void UpdateTexts(string name, float energy,  float life){
        Name.text = name;
        Energy.text = energy.ToString();
        Life.text = life.ToString();
    }
}
