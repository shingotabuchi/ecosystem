using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorSpawner : MonoBehaviour
{   
    public int PredatorToSpawn;
    public GameObject malePredatorPrefab,femalePredatorPrefab;
    public GameObject Map;
    void Awake()
    {
        float ScreenHalfHeight,ScreenHalfWidth,PredatorHeight,PredatorWidth;
        ScreenHalfHeight = Map.transform.localScale.x/2;
        ScreenHalfWidth = Map.transform.localScale.y/2;
        PredatorHeight = malePredatorPrefab.transform.localScale.y;
        PredatorWidth = malePredatorPrefab.transform.localScale.x;
        for (int i = 0; i < PredatorToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-ScreenHalfWidth + 10*PredatorWidth,ScreenHalfWidth - 10*PredatorWidth),Random.Range(-ScreenHalfHeight + 10*PredatorHeight,ScreenHalfHeight - 10*PredatorHeight),0);
            GameObject newPredator = Instantiate(malePredatorPrefab,spawnPosition,Quaternion.Euler(0,0,90));
            Predator newPredatorScript = newPredator.GetComponent<Predator>();
            newPredatorScript.viewRadius = Random.Range(2f,4f);
            newPredatorScript.impulseTime = Random.Range(0.5f,10f);
            newPredatorScript.soshakuJikan = Random.Range(1f,10f);
            newPredatorScript.matingRestJikan = Random.Range(1f,14f);
            newPredatorScript.matingAge = Random.Range(10f,50f);
            newPredatorScript.jumyou = Random.Range(150f,250f);
            newPredatorScript.potentialBenefitOfMovement = Random.Range(4f,11f);
            newPredatorScript.minimunEnergyToMate = Random.Range(0f,50f);
            newPredatorScript.minimunLifeToEat = Random.Range(0f,40f);
            newPredatorScript.RelativeBenefitParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
            newPredatorScript.RelativeCostParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
        }
        for (int i = 0; i < PredatorToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-ScreenHalfWidth + 10*PredatorWidth,ScreenHalfWidth - 10*PredatorWidth),Random.Range(-ScreenHalfHeight + 10*PredatorHeight,ScreenHalfHeight - 10*PredatorHeight),0);
            GameObject newPredator = Instantiate(femalePredatorPrefab,spawnPosition,Quaternion.Euler(0,0,90));
            Predator newPredatorScript = newPredator.GetComponent<Predator>();
            newPredatorScript.viewRadius = Random.Range(2f,4f);
            newPredatorScript.impulseTime = Random.Range(0.5f,10f);
            newPredatorScript.soshakuJikan = Random.Range(1f,10f);
            newPredatorScript.matingRestJikan = Random.Range(1f,14f);
            newPredatorScript.matingAge = Random.Range(10f,50f);
            newPredatorScript.jumyou = Random.Range(150f,250f);
            newPredatorScript.potentialBenefitOfMovement = Random.Range(4f,11f);
            newPredatorScript.minimunEnergyToMate = Random.Range(0f,50f);
            newPredatorScript.minimunLifeToEat = Random.Range(0f,40f);
            newPredatorScript.RelativeBenefitParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
            newPredatorScript.RelativeCostParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
        }
    }
}
