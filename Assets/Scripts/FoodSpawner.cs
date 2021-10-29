using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject food;
    public GameObject foodSpawnAreaObj; 
    public float SpawnTime;
    float timer = 0;
    Vector2 foodSpawnArea;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float rng = Random.Range(0f,1f);
        float[] bunpu = new float[5]{30f,25f,20f,15f,10f};
        if(rng<=Mathf.Exp(-SpawnTime/timer)){
            Vector3 SpawnPosition = foodSpawnAreaObj.transform.position + new Vector3(Random.Range(-foodSpawnAreaObj.transform.localScale.x/2,foodSpawnAreaObj.transform.localScale.x/2),Random.Range(-foodSpawnAreaObj.transform.localScale.y/2,foodSpawnAreaObj.transform.localScale.y/2),0);
            GameObject newFood = Instantiate(food,SpawnPosition,Quaternion.identity);
            Food newFoodScript = newFood.GetComponent<Food>();
            // float rng = Random.Range(0f,100f);
            // float sum = 0;
            // int i = 0;
            // while(sum<=rng){
            //     sum += bunpu[i];
            //     i++;
            //     if(sum==100f)break;
            // }
            // newFoodScript.value = (float)i;

            timer = 0;
        }
        timer += Time.deltaTime;
    }
}
