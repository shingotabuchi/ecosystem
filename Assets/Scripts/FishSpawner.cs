using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{   
    public int fishToSpawn;
    public GameObject maleFishPrefab,femaleFishPrefab;
    void Awake()
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
            newFishScript.matingAge = Random.Range(10f,50f);
            newFishScript.jumyou = Random.Range(100f,200f);
            newFishScript.potentialBenefitOfMovement = Random.Range(0.5f,10f);
            newFishScript.minimunEnergyToMate = Random.Range(0f,50f);
            newFishScript.minimunLifeToEat = Random.Range(0f,50f);
            newFishScript.RelativeBenefitParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
            newFishScript.RelativeCostParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
            print(1);

            // Fish.meanViewRadius += newFishScript.viewRadius;
            // Fish.meanImpulseTime += newFishScript.impulseTime;
            // Fish.meanKyuukaku += newFishScript.kyuukaku;
            // Fish.meanSoshakuJikan += newFishScript.soshakuJikan;
            // Fish.meanMatingRestJikan += newFishScript.matingRestJikan;
            // Fish.meanMatingAge += newFishScript.matingAge;
            // Fish.meanJumyo += newFishScript.jumyou;
            // Fish.meanPotentialBenefitOfMovement += newFishScript.potentialBenefitOfMovement;
            // Fish.meanMinimumEnergyToMate += newFishScript.minimunEnergyToMate;
            // Fish.meanMinimumLifeToEat += newFishScript.minimunLifeToEat;
            // Fish.meanRelativeBenefitParameter += newFishScript.RelativeBenefitParameter;
            // Fish.meanRelativeCostParameter += newFishScript.RelativeCostParameter;
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
            newFishScript.matingAge = Random.Range(10f,50f);
            newFishScript.jumyou = Random.Range(100f,200f);
            newFishScript.potentialBenefitOfMovement = Random.Range(0.5f,10f);
            newFishScript.minimunEnergyToMate = Random.Range(0f,50f);
            newFishScript.minimunLifeToEat = Random.Range(0f,50f);
            newFishScript.RelativeBenefitParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
            newFishScript.RelativeCostParameter = Mathf.Pow(10,Random.Range(-2f,-1f));
            print(1);

            // Fish.meanViewRadius += newFishScript.viewRadius;
            // Fish.meanImpulseTime += newFishScript.impulseTime;
            // Fish.meanKyuukaku += newFishScript.kyuukaku;
            // Fish.meanSoshakuJikan += newFishScript.soshakuJikan;
            // Fish.meanMatingRestJikan += newFishScript.matingRestJikan;
            // Fish.meanMatingAge += newFishScript.matingAge;
            // Fish.meanJumyo += newFishScript.jumyou;
            // Fish.meanPotentialBenefitOfMovement += newFishScript.potentialBenefitOfMovement;
            // Fish.meanMinimumEnergyToMate += newFishScript.minimunEnergyToMate;
            // Fish.meanMinimumLifeToEat += newFishScript.minimunLifeToEat;
            // Fish.meanRelativeBenefitParameter += newFishScript.RelativeBenefitParameter;
            // Fish.meanRelativeCostParameter += newFishScript.RelativeCostParameter;
        }

        // Fish.FishCount = fishToSpawn*2;
        // Fish.FishCountMale = fishToSpawn;
        // Fish.FishCountFemale = fishToSpawn;
        // Fish.meanViewRadius /= Fish.FishCount; 
        // Fish.meanImpulseTime /= Fish.FishCount; 
        // Fish.meanKyuukaku /= Fish.FishCount; 
        // Fish.meanSoshakuJikan /= Fish.FishCount; 
        // Fish.meanMatingRestJikan /= Fish.FishCount;
        // Fish.meanMatingAge /= Fish.FishCount; 
        // Fish.meanJumyo /= Fish.FishCount; 
        // Fish.meanPotentialBenefitOfMovement /= Fish.FishCount; 
        // Fish.meanMinimumEnergyToMate /= Fish.FishCount; 
        // Fish.meanMinimumLifeToEat /= Fish.FishCount; 
        // Fish.meanRelativeBenefitParameter /= Fish.FishCount; 
        // Fish.meanRelativeCostParameter /= Fish.FishCount; 
    }
}
