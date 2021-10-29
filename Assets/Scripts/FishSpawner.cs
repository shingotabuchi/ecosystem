using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{   
    public int fishToSpawn;
    public GameObject maleFishPrefab,femaleFishPrefab;
    void Start()
    {
        float ScreenHalfHeight,ScreenHalfWidth,FishHeight,FishWidth;
        ScreenHalfHeight = Camera.main.orthographicSize;
        ScreenHalfWidth = ScreenHalfHeight*Camera.main.aspect;
        FishHeight = maleFishPrefab.transform.localScale.y;
        FishWidth = maleFishPrefab.transform.localScale.x;
        for (int i = 0; i < fishToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-ScreenHalfWidth + 10*FishWidth,ScreenHalfWidth - 10*FishWidth),Random.Range(-ScreenHalfHeight + 10*FishHeight,ScreenHalfHeight - 10*FishHeight),0);
            GameObject newFish = Instantiate(maleFishPrefab,spawnPosition,Quaternion.Euler(0,0,90));
            Fish newFishScript = newFish.GetComponent<Fish>();
            newFishScript.viewRadius = Random.Range(1f,4f);
            newFishScript.impulseTime = Random.Range(1f,15f);
            newFishScript.kyuukaku = Random.Range(10f,50f);
            newFishScript.soshakuJikan = Random.Range(1f,15f);
            newFishScript.matingRestJikan = Random.Range(1f,15f);
            newFishScript.matingAge = Random.Range(20f,100f);
            newFishScript.jumyou = Random.Range(100f,200f);
            newFishScript.potentialBenefitOfMovement = Random.Range(0.5f,5f);
            newFishScript.minimunEnergyToMate = Random.Range(0f,30f);
            newFishScript.minimunLifeToEat = Random.Range(0f,30f);
            newFishScript.RelativeBenefitParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
            newFishScript.RelativeCostParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
        }
        for (int i = 0; i < fishToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-ScreenHalfWidth + 10*FishWidth,ScreenHalfWidth - 10*FishWidth),Random.Range(-ScreenHalfHeight + 10*FishHeight,ScreenHalfHeight - 10*FishHeight),0);
            GameObject newFish = Instantiate(femaleFishPrefab,spawnPosition,Quaternion.Euler(0,0,90));
            Fish newFishScript = newFish.GetComponent<Fish>();
            newFishScript.viewRadius = Random.Range(1f,4f);
            newFishScript.impulseTime = Random.Range(1f,15f);
            newFishScript.kyuukaku = Random.Range(10f,50f);
            newFishScript.soshakuJikan = Random.Range(1f,15f);
            newFishScript.matingRestJikan = Random.Range(1f,15f);
            newFishScript.matingAge = Random.Range(20f,100f);
            newFishScript.jumyou = Random.Range(100f,200f);
            newFishScript.potentialBenefitOfMovement = Random.Range(0.5f,5f);
            newFishScript.minimunEnergyToMate = Random.Range(0f,30f);
            newFishScript.minimunLifeToEat = Random.Range(0f,30f);
            newFishScript.RelativeBenefitParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
            newFishScript.RelativeCostParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
        }
    }
}
