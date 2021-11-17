using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fish : MonoBehaviour
{
    ////諸事情で必要なやつ
    Vector3 forward;
    float timer = 0;
    Vector3 scale;
    Rigidbody fishBody;
    Vector3 lastposition;
    float ScreenHalfHeight,ScreenHalfWidth,FishHeight,FishWidth;
    Vector3 up = new Vector3(0,0,-1);
    public GameObject stats;
    public GameObject highlight;
    float highlightTimer = 0;
    public float highlightShuuki;
    bool isOverSprite = false;
    bool isSelected = false;
    public static bool noneSelected = true;
    float lifeDecreaseRate;
    public GameObject maleFishPrefab,femaleFishPrefab;
    public GameObject Map;

    ////さかなの挙動用のへんすう
    public fishState state;
    public CriticalState critState;
    GameObject food;
    GameObject mate;
    Vector3 foodPosition;
    bool firstdirection;
    public float energy;
    public float life;
    float matingTimer;
    float runningTimer;

    ////さかなの挙動用の定数（共通）    
    public int viewKaizoudo;
    public float impulseAngle;
    public float impulseForce;
    public float rotateSpeed;
    public float moveCost;
    public float stillCost;
    public float matingJikan;
    public float valueAsFood;

    ////さかなの挙動用の定数（個別）    
    public float viewRadius;
    public float impulseTime;
    public float kyuukaku;
    public float soshakuJikan;
    public float matingRestJikan;
    public float matingAge;
    public float jumyou;
    public float potentialBenefitOfMovement;
    public float minimunEnergyToMate;
    public float minimunLifeToEat;
    public float RelativeBenefitParameter;
    public float RelativeCostParameter;
    public float minimumDistanceFromPredator;
    public float debugEnergy,debugCost;
    public Sex sex;
    public float EnergyToGiveBirth;
    public int NumberOfChildren;
    public float MaxEnergy;
    public float InitialEnergy;

    ////csvに記録するためのデータ
    public static int FishCount,FishCountMale,FishCountFemale;
    public static float meanViewRadius,meanImpulseTime,meanKyuukaku,meanSoshakuJikan,
    meanMatingRestJikan,meanMatingAge,meanJumyo,meanPotentialBenefitOfMovement,
    meanMinimumEnergyToMate,meanMinimumLifeToEat,meanRelativeBenefitParameter,meanRelativeCostParameter,
    meanMinimumDistanceFromPredator;
    public enum Sex
    {
        Male = 0,
        Female,
    }
    public enum fishState
    {
        SettlingDown = 0,
        Impulse,
        FoundFood,
        Eating,
        FoundMate,
        Mating,
        RunAway,
    };
    public enum CriticalState
    {
        None = 0,
        EnergyCritical,
        LifeCritical,
    }
    // Start is called before the first frame update
    void Start()
    {
        energy = InitialEnergy;
        life = 100;
        UpdateDataOnBirth();
        matingTimer = matingRestJikan;
        runningTimer = 0f;
        lifeDecreaseRate = life/jumyou;
        firstdirection = true;
        scale = transform.localScale;
        fishBody = GetComponent<Rigidbody>();
        state = fishState.SettlingDown;
        ScreenHalfHeight = Map.transform.localScale.x/2;
        ScreenHalfWidth = Map.transform.localScale.y/2;
        FishHeight = transform.localScale.y;
        FishWidth = transform.localScale.x;
    }
    IEnumerator SelectedAnimation(){
        while(true){
            Renderer renderer = highlight.GetComponent<Renderer>();
            Color color = new Color(renderer.material.color.r,renderer.material.color.g,renderer.material.color.b,0.5f+0.3f*Mathf.Sin(2f*Mathf.PI*highlightTimer/highlightShuuki));
            renderer.material.SetColor("_Color", color);
            highlightTimer += 1f/60f;
            yield return null;
        }
    }
    void OnMouseEnter()
    {
        isOverSprite = true;
    }
    void OnMouseExit()
    {
        isOverSprite = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0)){
            if(isOverSprite&&!isSelected&&noneSelected){
                noneSelected = false;
                highlightTimer = 0;
                highlight.SetActive(true);
                StatScript statScript = stats.GetComponent<StatScript>();
                statScript.selectedFish = gameObject;
                StartCoroutine("SelectedAnimation");
                isSelected = true;
            }
            else if(isSelected){
                noneSelected = true;
                isSelected = false;
                highlightTimer = 0;
                highlight.SetActive(false);
                StatScript statScript = stats.GetComponent<StatScript>();
                statScript.selectedFish = null;
                StopCoroutine("SelectedAnimation");
            }
        }

        CheckForPredator();

        float rng;
        Vector3 powerDirection;
        forward = transform.up;
        
        float age = (100f-life)/lifeDecreaseRate;
        if(age>=matingAge&&matingTimer>=matingRestJikan&&gameObject.layer == LayerMask.NameToLayer("Fish")&&critState!=CriticalState.EnergyCritical){
            float fugo;
            if(firstdirection) fugo = 1f;
            else fugo = -1f;
            for (int i = 0; i < viewKaizoudo; i++)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position,RotateZ(fugo*forward,i*60f/(float)viewKaizoudo),out hit,viewRadius,1 << LayerMask.NameToLayer("Fish"))){
                    Fish fishScript = hit.transform.gameObject.GetComponent<Fish>();
                    if(fishScript.sex==Sex.Female&&sex==Sex.Male){
                        if(fishScript.PleaseMate(gameObject)){
                            state = fishState.FoundMate;
                            mate = hit.transform.gameObject;
                            gameObject.layer = LayerMask.NameToLayer("FoundMateFish");
                            break;
                        }
                    }
                }
                if(Physics.Raycast(transform.position,RotateZ(fugo*forward,-i*60f/(float)viewKaizoudo),out hit,viewRadius,1 << LayerMask.NameToLayer("Fish"))){
                    Fish fishScript = hit.transform.gameObject.GetComponent<Fish>();
                    if(fishScript.sex==Sex.Female&&sex==Sex.Male){
                        if(fishScript.PleaseMate(gameObject)){
                            state = fishState.FoundMate;
                            mate = hit.transform.gameObject;
                            gameObject.layer = LayerMask.NameToLayer("FoundMateFish");
                            break;
                        }
                    }
                }
            }
        }

        matingTimer += Time.deltaTime;
        energy -= stillCost;
        switch(state){
            case fishState.SettlingDown:
                if(transform.rotation.eulerAngles.z<90){
                    transform.Rotate(0,0,rotateSpeed*Time.deltaTime);
                }
                if(transform.rotation.eulerAngles.z>90){
                    transform.Rotate(0,0,-rotateSpeed*Time.deltaTime);
                }
                if(Mathf.Abs(transform.rotation.eulerAngles.z)>85&&Mathf.Abs(transform.rotation.eulerAngles.z)<95){
                    if(potentialBenefitOfMovement*(RelativeBenefitCoefficient(energy) + RelativeBenefitCoefficient(life))-moveCost*RelativeCostCoefficient(energy)>0){
                        state++;
                    }
                }
                break;
            case fishState.Impulse:
                rng = Random.Range(0f,1f);
                if(rng<=Mathf.Exp(-impulseTime/timer)){
                    int direction = Random.Range(0,2);
                    // int direction = 0;
                    float force = Random.Range(0f,impulseForce);
                    // float force = impulseForce;
                    if(direction == 0){
                        firstdirection = true;
                        transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                        float powerAngle = Random.Range(-impulseAngle,impulseAngle);
                        powerDirection = RotateZ(forward,powerAngle);
                        transform.Rotate(0,0,powerAngle);
                        // fishBody.AddForce(powerDirection*impulseForce,ForceMode.Impulse);
                        fishBody.AddForce(powerDirection*force,ForceMode.Impulse);
                    }
                    else{
                        firstdirection = false;
                        transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
                        float powerAngle = Random.Range(-impulseAngle,impulseAngle);
                        powerDirection = -RotateZ(forward,powerAngle);
                        transform.Rotate(0,0,powerAngle);
                        // fishBody.AddForce(powerDirection*impulseForce,ForceMode.Impulse);
                        fishBody.AddForce(powerDirection*force,ForceMode.Impulse);
                    }
                    energy -= moveCost;
                    timer = 0;
                    state = fishState.SettlingDown;
                    break;
                }
                timer+=Time.deltaTime;
                break;
            case fishState.FoundFood:
                if(food==null){
                    gameObject.layer = LayerMask.NameToLayer("Fish");
                    state = fishState.SettlingDown;
                }
                Vector3 direcToFood = foodPosition-transform.position;
                float muki;
                float mukiForMuki = Vector3.SignedAngle(direcToFood,new Vector3(-1,0,0),up);
                if(Mathf.Abs(mukiForMuki)>90){
                    firstdirection = false;
                    transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
                    muki = Vector3.SignedAngle(direcToFood,-forward,up);
                }
                else{
                    firstdirection = true;
                    transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                    muki = Vector3.SignedAngle(direcToFood,forward,up);
                }
                
                if(firstdirection){
                    transform.Rotate(0,0,muki);
                }
                else{
                    transform.Rotate(0,0,muki);
                }
                powerDirection = direcToFood.normalized;
                // powerDirection = RotateZ(forward,muki);
                rng = Random.Range(0f,1f);
                if(rng<=Mathf.Exp(-impulseTime/timer)){
                    fishBody.AddForce(powerDirection*impulseForce,ForceMode.Impulse);
                    // if(firstdirection){
                    //     fishBody.AddForce(powerDirection*impulseForce,ForceMode.Impulse);
                    // }
                    // else{
                    //     fishBody.AddForce(-powerDirection*impulseForce,ForceMode.Impulse);
                    // }
                    timer = 0;
                    energy -= moveCost;
                }
                timer+=Time.deltaTime;
                break;
             case fishState.Eating:
                if(transform.rotation.eulerAngles.z<90){
                    transform.Rotate(0,0,rotateSpeed*Time.deltaTime);
                }
                if(transform.rotation.eulerAngles.z>90){
                    transform.Rotate(0,0,-rotateSpeed*Time.deltaTime);
                }
                rng = Random.Range(0f,1f);
                if(rng<=Mathf.Exp(-soshakuJikan/timer)){
                    gameObject.layer = LayerMask.NameToLayer("Fish");
                    state = fishState.SettlingDown;
                    timer = 0;
                    break;
                }
                else gameObject.layer = LayerMask.NameToLayer("FoundFoodFish");
                timer += Time.deltaTime;
                break;
            case fishState.FoundMate:
                if(mate==null){
                    gameObject.layer = LayerMask.NameToLayer("Fish");
                    state = fishState.SettlingDown;
                }
                Vector3 direcToMate = mate.transform.position-transform.position;
                if(direcToMate.magnitude<0.5){
                    timer = 0;
                    state++;
                    break;
                }
                mukiForMuki = Vector3.SignedAngle(direcToMate,new Vector3(-1,0,0),up);
                if(Mathf.Abs(mukiForMuki)>90){
                    firstdirection = false;
                    transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
                    muki = Vector3.SignedAngle(direcToMate,-forward,up);
                }
                else{
                    firstdirection = true;
                    transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                    muki = Vector3.SignedAngle(direcToMate,forward,up);
                }
                
                transform.Rotate(0,0,muki);
                powerDirection = direcToMate.normalized;
                // powerDirection = RotateZ(forward,muki);
                rng = Random.Range(0f,1f);
                if(rng<=Mathf.Exp(-impulseTime/timer)){
                    fishBody.AddForce(powerDirection*impulseForce,ForceMode.Impulse);
                    // if(firstdirection){
                    //     fishBody.AddForce(powerDirection*impulseForce,ForceMode.Impulse);
                    // }
                    // else{
                    //     fishBody.AddForce(-powerDirection*impulseForce,ForceMode.Impulse);
                    // }
                    timer = 0;
                    energy -= moveCost;
                }
                timer+=Time.deltaTime;
                break;
            case fishState.Mating:
                if(mate==null){
                    gameObject.layer = LayerMask.NameToLayer("Fish");
                    state = fishState.SettlingDown;
                    break;
                }
                if(transform.rotation.eulerAngles.z<90){
                    transform.Rotate(0,0,rotateSpeed*Time.deltaTime);
                }
                if(transform.rotation.eulerAngles.z>90){
                    transform.Rotate(0,0,-rotateSpeed*Time.deltaTime);
                }
                if(timer > matingJikan){
                    if(sex == Sex.Female){
                        for (int i = 0; i < NumberOfChildren; i++)
                        {
                            int sexRng = Random.Range(0,2);
                            GameObject newFish;
                            if(sexRng==0)newFish = Instantiate(maleFishPrefab,transform.position,Quaternion.Euler(0,0,90));
                            else newFish = Instantiate(femaleFishPrefab,transform.position,Quaternion.Euler(0,0,90));
                            PassDownGenes(newFish,gameObject,mate);
                            newFish.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                            newFish.layer = LayerMask.NameToLayer("Fish");
                            Fish newFishScript = newFish.GetComponent<Fish>();
                            newFishScript.InitialEnergy = EnergyToGiveBirth/NumberOfChildren;
                        }
                        energy -= EnergyToGiveBirth;
                    }
                    gameObject.layer = LayerMask.NameToLayer("Fish");
                    matingTimer = 0;
                    state = fishState.SettlingDown;
                    timer = 0;
                    mate = null;
                    break;
                }
                else gameObject.layer = LayerMask.NameToLayer("FoundMateFish");
                timer += Time.deltaTime;
                break;
            case fishState.RunAway:
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, minimumDistanceFromPredator, (1 << LayerMask.NameToLayer("Predator"))|(1 << LayerMask.NameToLayer("FoundMatePredator"))|(1 << LayerMask.NameToLayer("FoundFoodPredator")));
                Vector3 DirectionToPredators = Vector3.zero;
                if(hitColliders.Length==0){
                    gameObject.layer = LayerMask.NameToLayer("Fish");
                    runningTimer = 1f;
                    state = fishState.SettlingDown;
                    break;
                } 
                foreach(var hitCollider in hitColliders){
                    Vector3 rawdirec = hitCollider.transform.position-transform.position;
                    float dist = rawdirec.magnitude;
                    // float coefficient = 1 - dist/minimumDistanceFromPredator;
                    float coefficient = 1;
                    DirectionToPredators += coefficient * rawdirec;
                }
                Vector3 runAwayDirec = -DirectionToPredators;
                runAwayDirec = Vector3.Normalize(runAwayDirec);
                Debug.DrawRay(transform.position,runAwayDirec*minimumDistanceFromPredator, Color.blue);
                mukiForMuki = Vector3.SignedAngle(runAwayDirec,new Vector3(-1,0,0),up);
                if(Mathf.Abs(mukiForMuki)>90){
                    firstdirection = false;
                    transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
                    muki = Vector3.SignedAngle(runAwayDirec,-forward,up);
                }
                else{
                    firstdirection = true;
                    transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                    muki = Vector3.SignedAngle(runAwayDirec,forward,up);
                }
                
                transform.Rotate(0,0,muki);
                powerDirection = runAwayDirec;
                // powerDirection = RotateZ(forward,muki);
                rng = Random.Range(0f,1f);
                if(rng<=Mathf.Exp(-impulseTime/timer)){
                    fishBody.AddForce(powerDirection*impulseForce,ForceMode.Impulse);
                    // if(firstdirection){
                    //     fishBody.AddForce(powerDirection*impulseForce,ForceMode.Impulse);
                    // }
                    // else{
                    //     fishBody.AddForce(-powerDirection*impulseForce,ForceMode.Impulse);
                    // }
                    timer = 0;
                    energy -= moveCost;
                }
                timer+=Time.deltaTime;
                break;
        }

        if(Mathf.Abs(transform.position.x)>ScreenHalfWidth-5*FishWidth||Mathf.Abs(transform.position.y)>ScreenHalfHeight-5*FishHeight){
            transform.position = lastposition;
        }
        else lastposition = transform.position;

        life -= lifeDecreaseRate*Time.deltaTime;
        if(life<0||energy<0){
            UpdateDataOnDeath();
            Destroy(gameObject);
        }
        if(energy>minimunEnergyToMate&&life<minimunLifeToEat&&sex==Sex.Male){
            critState = CriticalState.LifeCritical;
        }
        //男女平等
        // if(energy>minimunEnergyToMate&&life<minimunLifeToEat){
        //     critState = CriticalState.LifeCritical;
        // }
        else if(energy<minimunEnergyToMate&&life>minimunLifeToEat){
            critState = CriticalState.EnergyCritical;
        }
        else critState = CriticalState.None;
    }
    public void CheckForPredator(){
        if(gameObject.layer != LayerMask.NameToLayer("RunningAwayFish")&&runningTimer<=0f){
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, minimumDistanceFromPredator, (1 << LayerMask.NameToLayer("Predator"))|(1 << LayerMask.NameToLayer("FoundMatePredator"))|(1 << LayerMask.NameToLayer("FoundFoodPredator")));
            if(hitColliders.Length>0){
                state = fishState.RunAway;
                // timer = 100000000000f;
                gameObject.layer = LayerMask.NameToLayer("RunningAwayFish");
            } 
        }
        runningTimer -= Time.deltaTime;
    }
    public void UpdateDataOnDeath(){
        Fish.meanViewRadius = (Fish.meanViewRadius * Fish.FishCount - viewRadius)/(Fish.FishCount-1);
        Fish.meanImpulseTime = (Fish.meanImpulseTime * Fish.FishCount - impulseTime)/(Fish.FishCount-1);
        Fish.meanKyuukaku = (Fish.meanKyuukaku * Fish.FishCount - kyuukaku)/(Fish.FishCount-1);
        Fish.meanSoshakuJikan = (Fish.meanSoshakuJikan * Fish.FishCount - soshakuJikan)/(Fish.FishCount-1);
        Fish.meanMatingRestJikan = (Fish.meanMatingRestJikan * Fish.FishCount - matingRestJikan)/(Fish.FishCount-1);
        Fish.meanMatingAge = (Fish.meanMatingAge * Fish.FishCount - matingAge)/(Fish.FishCount-1);
        Fish.meanJumyo = (Fish.meanJumyo * Fish.FishCount - jumyou)/(Fish.FishCount-1);
        Fish.meanPotentialBenefitOfMovement = (Fish.meanPotentialBenefitOfMovement * Fish.FishCount - potentialBenefitOfMovement)/(Fish.FishCount-1);
        Fish.meanMinimumEnergyToMate = (Fish.meanMinimumEnergyToMate * Fish.FishCount - minimunEnergyToMate)/(Fish.FishCount-1);
        Fish.meanMinimumLifeToEat = (Fish.meanMinimumLifeToEat * Fish.FishCount - minimunLifeToEat)/(Fish.FishCount-1);
        Fish.meanRelativeBenefitParameter = (Fish.meanRelativeBenefitParameter * Fish.FishCount - RelativeBenefitParameter)/(Fish.FishCount-1);
        Fish.meanRelativeCostParameter = (Fish.meanRelativeCostParameter * Fish.FishCount - RelativeCostParameter)/(Fish.FishCount-1);
        Fish.meanMinimumDistanceFromPredator = (Fish.meanMinimumDistanceFromPredator * Fish.FishCount - minimumDistanceFromPredator)/(Fish.FishCount-1);
        Fish.FishCount--;
        if(sex==Sex.Male) Fish.FishCountMale--;
        else Fish.FishCountFemale--;
    }
    void UpdateDataOnBirth(){
        Fish.meanViewRadius = (Fish.meanViewRadius * Fish.FishCount + viewRadius)/(Fish.FishCount+1);
        Fish.meanImpulseTime = (Fish.meanImpulseTime * Fish.FishCount + impulseTime)/(Fish.FishCount+1);
        Fish.meanKyuukaku = (Fish.meanKyuukaku * Fish.FishCount + kyuukaku)/(Fish.FishCount+1);
        Fish.meanSoshakuJikan = (Fish.meanSoshakuJikan * Fish.FishCount + soshakuJikan)/(Fish.FishCount+1);
        Fish.meanMatingRestJikan = (Fish.meanMatingRestJikan * Fish.FishCount + matingRestJikan)/(Fish.FishCount+1);
        Fish.meanMatingAge = (Fish.meanMatingAge * Fish.FishCount + matingAge)/(Fish.FishCount+1);
        Fish.meanJumyo = (Fish.meanJumyo * Fish.FishCount + jumyou)/(Fish.FishCount+1);
        Fish.meanPotentialBenefitOfMovement = (Fish.meanPotentialBenefitOfMovement * Fish.FishCount + potentialBenefitOfMovement)/(Fish.FishCount+1);
        Fish.meanMinimumEnergyToMate = (Fish.meanMinimumEnergyToMate * Fish.FishCount + minimunEnergyToMate)/(Fish.FishCount+1);
        Fish.meanMinimumLifeToEat = (Fish.meanMinimumLifeToEat * Fish.FishCount + minimunLifeToEat)/(Fish.FishCount+1);
        Fish.meanRelativeBenefitParameter = (Fish.meanRelativeBenefitParameter * Fish.FishCount + RelativeBenefitParameter)/(Fish.FishCount+1);
        Fish.meanRelativeCostParameter = (Fish.meanRelativeCostParameter * Fish.FishCount + RelativeCostParameter)/(Fish.FishCount+1);
        Fish.meanMinimumDistanceFromPredator = (Fish.meanMinimumDistanceFromPredator * Fish.FishCount + minimumDistanceFromPredator)/(Fish.FishCount+1);
        Fish.FishCount++;
        if(sex==Sex.Male) Fish.FishCountMale++;
        else Fish.FishCountFemale++;
    }
    void PassDownGenes(GameObject baby, GameObject mom, GameObject dad){
        Fish babyScript,momScript,dadScript;
        babyScript = baby.GetComponent<Fish>();
        momScript = mom.GetComponent<Fish>();
        dadScript = dad.GetComponent<Fish>();
        int rng;
        rng = Random.Range(0,2);
        if(rng==0) babyScript.viewRadius = momScript.viewRadius + Random.Range(-0.5f,0.5f);
        else babyScript.viewRadius = dadScript.viewRadius + Random.Range(-0.5f,0.5f);
        if(babyScript.viewRadius<0)babyScript.viewRadius = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.impulseTime = momScript.impulseTime + Random.Range(-0.1f,0.1f);
        else babyScript.impulseTime = dadScript.impulseTime + Random.Range(-0.1f,0.1f);
        if(babyScript.impulseTime<0)babyScript.impulseTime = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.kyuukaku = momScript.kyuukaku + Random.Range(-2f,2f);
        else babyScript.kyuukaku = dadScript.kyuukaku + Random.Range(-2f,2f);
        if(babyScript.kyuukaku<0)babyScript.kyuukaku = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.soshakuJikan = momScript.soshakuJikan + Random.Range(-1f,1f);
        else babyScript.soshakuJikan = dadScript.soshakuJikan + Random.Range(-1f,1f);
        if(babyScript.soshakuJikan<0)babyScript.soshakuJikan = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.matingRestJikan = momScript.matingRestJikan + Random.Range(-1f,1f);
        else babyScript.matingRestJikan = dadScript.matingRestJikan + Random.Range(-1f,1f);
        if(babyScript.matingRestJikan<0)babyScript.matingRestJikan = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.matingAge = momScript.matingAge + Random.Range(-5f,5f);
        else babyScript.matingAge = dadScript.matingAge + Random.Range(-5f,5f);
        if(babyScript.matingAge<0)babyScript.matingAge = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.jumyou = momScript.jumyou + Random.Range(-10f,10f);
        else babyScript.jumyou = dadScript.jumyou + Random.Range(-10f,10f);
        if(babyScript.jumyou<0)babyScript.jumyou = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.potentialBenefitOfMovement = momScript.potentialBenefitOfMovement + Random.Range(-0.5f,0.5f);
        else babyScript.potentialBenefitOfMovement = dadScript.potentialBenefitOfMovement + Random.Range(-0.5f,0.5f);
        if(babyScript.potentialBenefitOfMovement<0)babyScript.potentialBenefitOfMovement = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.minimunEnergyToMate = momScript.minimunEnergyToMate + Random.Range(-3f,3f);
        else babyScript.minimunEnergyToMate = dadScript.minimunEnergyToMate + Random.Range(-3f,3f);
        if(babyScript.minimunEnergyToMate<0)babyScript.minimunEnergyToMate = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.minimunLifeToEat = momScript.minimunLifeToEat + Random.Range(-3f,3f);
        else babyScript.minimunLifeToEat = dadScript.minimunLifeToEat + Random.Range(-3f,3f);
        if(babyScript.minimunLifeToEat<0)babyScript.minimunLifeToEat = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.RelativeBenefitParameter = Mathf.Pow(10,Mathf.Log10(momScript.RelativeBenefitParameter) + Random.Range(-0.1f,0.1f));
        else babyScript.RelativeBenefitParameter = Mathf.Pow(10,Mathf.Log10(dadScript.RelativeBenefitParameter) + Random.Range(-0.1f,0.1f));

        rng = Random.Range(0,2);
        if(rng==0) babyScript.RelativeCostParameter =  Mathf.Pow(10,Mathf.Log10(momScript.RelativeCostParameter) + Random.Range(-0.1f,0.1f));
        else babyScript.RelativeCostParameter = Mathf.Pow(10,Mathf.Log10(dadScript.RelativeCostParameter) + Random.Range(-0.1f,0.1f));
        
        rng = Random.Range(0,2);
        if(rng==0) babyScript.minimumDistanceFromPredator =  Mathf.Pow(10,Mathf.Log10(momScript.minimumDistanceFromPredator) + Random.Range(-0.5f,0.5f));
        else babyScript.minimumDistanceFromPredator = Mathf.Pow(10,Mathf.Log10(dadScript.minimumDistanceFromPredator) + Random.Range(-0.5f,0.5f));
        if(babyScript.minimumDistanceFromPredator<0)babyScript.minimumDistanceFromPredator = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.EnergyToGiveBirth = momScript.EnergyToGiveBirth + Random.Range(-5f,5f);
        else babyScript.EnergyToGiveBirth = dadScript.EnergyToGiveBirth + Random.Range(-5f,5f);
        if(babyScript.EnergyToGiveBirth<0)babyScript.EnergyToGiveBirth = 0;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.NumberOfChildren = momScript.NumberOfChildren + (1 - 2*Random.Range(0,2))*Random.Range(0,11)/10;
        else babyScript.NumberOfChildren = dadScript.NumberOfChildren + (1 - 2*Random.Range(0,2))*Random.Range(0,11)/10;
        if(babyScript.NumberOfChildren<1)babyScript.NumberOfChildren = 1;

        rng = Random.Range(0,2);
        if(rng==0) babyScript.MaxEnergy = momScript.MaxEnergy + Random.Range(-5f,5f);
        else babyScript.MaxEnergy = dadScript.MaxEnergy + Random.Range(-5f,5f);
        if(babyScript.MaxEnergy<0)babyScript.MaxEnergy = 0;
    }

    float RelativeBenefitCoefficient(float enrg){
        return ( Mathf.Exp(-enrg*RelativeBenefitParameter) - Mathf.Exp(-MaxEnergy*RelativeBenefitParameter) ) / ( 1-Mathf.Exp(-MaxEnergy*RelativeBenefitParameter) );
    }
    float RelativeCostCoefficient(float enrg){
        if(( Mathf.Exp(-enrg*RelativeCostParameter) - Mathf.Exp(-MaxEnergy*RelativeCostParameter) ) / ( 1-Mathf.Exp(-MaxEnergy*RelativeCostParameter) )>=0) return ( Mathf.Exp(-enrg*RelativeCostParameter) - Mathf.Exp(-MaxEnergy*RelativeCostParameter) ) / ( 1-Mathf.Exp(-MaxEnergy*RelativeCostParameter) );
        else return 0f;
        
    }

    void OnTriggerEnter(Collider other){
        if(other.transform.gameObject.layer==LayerMask.NameToLayer("Food")){
            Food foodScript = other.transform.gameObject.GetComponent<Food>();
            energy += foodScript.value;
            Destroy(other.transform.gameObject);
            timer = 0;
            state = fishState.Eating;
            // gameObject.layer = LayerMask.NameToLayer("Fish");
        }
    }
    public void SenseFood(GameObject Food, float scent, float value){
        if(kyuukaku<=scent){
            float kyori = Vector3.Distance(Food.transform.position,transform.position);
            float cost = (kyori/1.5f)* moveCost;
            if(critState == CriticalState.LifeCritical) return;
            
            if(value*RelativeBenefitCoefficient(energy)-cost*RelativeCostCoefficient(energy)>0){
                debugEnergy = value*RelativeBenefitCoefficient(energy);
                debugCost = cost*RelativeCostCoefficient(energy);
                food = Food;
                state = fishState.FoundFood;
                foodPosition = food.transform.position;
                timer = 100000000000f;
                gameObject.layer = LayerMask.NameToLayer("FoundFoodFish");
            }
        }
    }
    public bool PleaseMate(GameObject male){
        Fish maleScript = male.GetComponent<Fish>();
        float age = (100f-life)/lifeDecreaseRate;
        if(age>=matingAge&&matingTimer>=matingRestJikan&&gameObject.layer == LayerMask.NameToLayer("Fish")&&critState!=CriticalState.EnergyCritical&&energy>EnergyToGiveBirth + minimunEnergyToMate){
            gameObject.layer =  LayerMask.NameToLayer("FoundMateFish");
            mate = male;
            state = fishState.FoundMate;
            return true;
        } 
        else return false;
    }
    Vector3 RotateZ(Vector3 direc,float Theta){
        return new Vector3(direc.x*Mathf.Cos(Theta/180*Mathf.PI)-direc.y*Mathf.Sin(Theta/180*Mathf.PI),direc.y*Mathf.Cos(Theta/180*Mathf.PI)+direc.x*Mathf.Sin(Theta/180*Mathf.PI),direc.z);
    }
}
