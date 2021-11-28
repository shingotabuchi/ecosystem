using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class KeyFrame
{
    public float Time;
    public int Count,CountMale,CountFemale;
    public float ViewRadius,ImpulseTime,Kyuukaku,SoshakuJikan,
    MatingRestJikan,MatingAge,Jumyo,PotentialBenefitOfMovement,
    MinimumEnergyToMate,MinimumLifeToEat,RelativeBenefitParameter,RelativeCostParameter,
    MinimumDistanceFromPredator,energyToGiveBirth;
    public float NumberOfChildren;
    public float MaxEnergy, InitialEnergy;
    public KeyFrame(){}

    public KeyFrame (float time, int count, int countMale, int countFemale, float viewRadius, float impulseTime,
    float kyuukaku, float soshakuJikan, float matingRestJikan, float matingAge, float jumyo, float potentialBenefitOfMovement,
    float minimumEnergyToMate, float minimumLifeToEat, float relativeBenefitParameter, float relativeCostParameter, float minimumDistanceFromPredator, float EnergyToGiveBirth, float numberOfChildren, float maxEnergy, float initialEnergy)
    {
        Time = time;
        Count = count;
        CountMale = countMale;
        CountFemale = countFemale;
        ViewRadius = viewRadius;
        ImpulseTime = impulseTime;
        Kyuukaku = kyuukaku;
        SoshakuJikan = soshakuJikan;
        MatingRestJikan = matingRestJikan;
        MatingAge = matingAge;
        Jumyo = jumyo;
        PotentialBenefitOfMovement = potentialBenefitOfMovement;
        MinimumEnergyToMate = minimumEnergyToMate;
        MinimumLifeToEat = minimumLifeToEat;
        RelativeBenefitParameter = relativeBenefitParameter;
        RelativeCostParameter = relativeCostParameter;
        MinimumDistanceFromPredator = minimumDistanceFromPredator;
        energyToGiveBirth = EnergyToGiveBirth;
        NumberOfChildren = numberOfChildren;
        MaxEnergy = maxEnergy;
        InitialEnergy = initialEnergy;
    }
}
public class KeyFramePredator
{
    public float Time;
    public int Count,CountMale,CountFemale;
    public float ViewRadius,ImpulseTime,SoshakuJikan,
    MatingRestJikan,MatingAge,Jumyo,PotentialBenefitOfMovement,
    MinimumEnergyToMate,MinimumLifeToEat,RelativeBenefitParameter,RelativeCostParameter,energyToGiveBirth;
    public float NumberOfChildren;
    public float MaxEnergy, InitialEnergy;
    public KeyFramePredator(){}

    public KeyFramePredator (float time, int count, int countMale, int countFemale, float viewRadius, float impulseTime, 
    float soshakuJikan, float matingRestJikan, float matingAge, float jumyo, float potentialBenefitOfMovement,
    float minimumEnergyToMate, float minimumLifeToEat, float relativeBenefitParameter, float relativeCostParameter, float EnergyToGiveBirth, float numberOfChildren, float maxEnergy, float initialEnergy)
    {
        Time = time;
        Count = count;
        CountMale = countMale;
        CountFemale = countFemale;
        ViewRadius = viewRadius;
        ImpulseTime = impulseTime;
        SoshakuJikan = soshakuJikan;
        MatingRestJikan = matingRestJikan;
        MatingAge = matingAge;
        Jumyo = jumyo;
        PotentialBenefitOfMovement = potentialBenefitOfMovement;
        MinimumEnergyToMate = minimumEnergyToMate;
        MinimumLifeToEat = minimumLifeToEat;
        RelativeBenefitParameter = relativeBenefitParameter;
        RelativeCostParameter = relativeCostParameter;
        energyToGiveBirth = EnergyToGiveBirth;
        NumberOfChildren = numberOfChildren;
        MaxEnergy = maxEnergy;
        InitialEnergy = initialEnergy;
    }
}
public class SaveToCsv : MonoBehaviour
{
    float timer = 0;
    private List<KeyFrame> keyFrames = new List<KeyFrame>(60000);
    private List<KeyFramePredator> keyFramesPredator = new List<KeyFramePredator>(60000);
    // Start is called before the first frame update
    void Start()
    {
        keyFrames.Add(new KeyFrame (Time.time, Fish.FishCount, Fish.FishCountMale, Fish.FishCountFemale,Fish.meanViewRadius,Fish.meanImpulseTime,Fish.meanKyuukaku,Fish.meanSoshakuJikan,Fish.meanMatingRestJikan,Fish.meanMatingAge,Fish.meanJumyo,Fish.meanPotentialBenefitOfMovement,
        Fish.meanMinimumEnergyToMate,Fish.meanMinimumLifeToEat,Fish.meanRelativeBenefitParameter,Fish.meanRelativeCostParameter,Fish.meanMinimumDistanceFromPredator,Fish.meanEnergyToGiveBirth,Fish.meanNumberOfChildren,Fish.meanMaxEnergy,Fish.meanInitialEnergy));
        keyFramesPredator.Add(new KeyFramePredator (Time.time, Predator.FishCount, Predator.FishCountMale, Predator.FishCountFemale,Predator.meanViewRadius,Predator.meanImpulseTime,Predator.meanSoshakuJikan,Predator.meanMatingRestJikan,Predator.meanMatingAge,Predator.meanJumyo,Predator.meanPotentialBenefitOfMovement,
        Predator.meanMinimumEnergyToMate,Predator.meanMinimumLifeToEat,Predator.meanRelativeBenefitParameter,Predator.meanRelativeCostParameter,Predator.meanEnergyToGiveBirth,Predator.meanNumberOfChildren,Predator.meanMaxEnergy,Predator.meanInitialEnergy));
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>1){
            keyFrames.Add(new KeyFrame (Time.time, Fish.FishCount, Fish.FishCountMale, Fish.FishCountFemale,Fish.meanViewRadius,Fish.meanImpulseTime,Fish.meanKyuukaku,Fish.meanSoshakuJikan,Fish.meanMatingRestJikan,Fish.meanMatingAge,Fish.meanJumyo,Fish.meanPotentialBenefitOfMovement,
            Fish.meanMinimumEnergyToMate,Fish.meanMinimumLifeToEat,Fish.meanRelativeBenefitParameter,Fish.meanRelativeCostParameter,Fish.meanMinimumDistanceFromPredator,Fish.meanEnergyToGiveBirth,Fish.meanNumberOfChildren,Fish.meanMaxEnergy,Fish.meanInitialEnergy));
            keyFramesPredator.Add(new KeyFramePredator (Time.time, Predator.FishCount, Predator.FishCountMale, Predator.FishCountFemale,Predator.meanViewRadius,Predator.meanImpulseTime,Predator.meanSoshakuJikan,Predator.meanMatingRestJikan,Predator.meanMatingAge,Predator.meanJumyo,Predator.meanPotentialBenefitOfMovement,
            Predator.meanMinimumEnergyToMate,Predator.meanMinimumLifeToEat,Predator.meanRelativeBenefitParameter,Predator.meanRelativeCostParameter,Predator.meanEnergyToGiveBirth,Predator.meanNumberOfChildren,Predator.meanMaxEnergy,Predator.meanInitialEnergy));
            timer = 0;
        }
        timer += Time.deltaTime;
    }
    public string ToCSV()
    {
        var sb = new StringBuilder("Time.time,Fish.FishCount,Fish.FishCountMale,Fish.FishCountFemale,Fish.meanViewRadius,Fish.meanImpulseTime,Fish.meanKyuukaku,Fish.meanSoshakuJikan,Fish.meanMatingRestJikan,Fish.meanMatingAge,Fish.meanJumyo,Fish.meanPotentialBenefitOfMovement,Fish.meanMinimumEnergyToMate,Fish.meanMinimumLifeToEat,Fish.meanRelativeBenefitParameter,Fish.meanRelativeCostParameter,Fish.meanMinimumDistanceFromPredator,Fish.meanEnergyToGiveBirth,Fish.meanNumberOfChildren,Fish.meanMaxEnergy,Fish.meanInitialEnergy,Predator.FishCount, Predator.FishCountMale, Predator.FishCountFemale,Predator.meanViewRadius,Predator.meanImpulseTime,Predator.meanSoshakuJikan,Predator.meanMatingRestJikan,Predator.meanMatingAge,Predator.meanJumyo,Predator.meanPotentialBenefitOfMovement,Predator.meanMinimumEnergyToMate,Predator.meanMinimumLifeToEat,Predator.meanRelativeBenefitParameter,Predator.meanRelativeCostParameter,Predator.meanEnergyToGiveBirth,Predator.meanNumberOfChildren,Predator.meanMaxEnergy,Predator.meanInitialEnergy");
        int i = 0;
        foreach(var frame in keyFrames)
        {
            sb.Append('\n').Append(frame.Time.ToString())
            .Append(',').Append(frame.Count.ToString()).Append(',').Append(frame.CountMale.ToString()).Append(',').Append(frame.CountFemale.ToString())
            .Append(',').Append(frame.ViewRadius.ToString()).Append(',').Append(frame.ImpulseTime.ToString()).Append(',').Append(frame.Kyuukaku.ToString())
            .Append(',').Append(frame.SoshakuJikan.ToString()).Append(',').Append(frame.MatingRestJikan.ToString()).Append(',').Append(frame.MatingAge.ToString())
            .Append(',').Append(frame.Jumyo.ToString()).Append(',').Append(frame.PotentialBenefitOfMovement.ToString()).Append(',').Append(frame.MinimumEnergyToMate.ToString())
            .Append(',').Append(frame.MinimumLifeToEat.ToString()).Append(',').Append(frame.RelativeBenefitParameter.ToString()).Append(',').Append(frame.RelativeCostParameter.ToString())
            .Append(',').Append(frame.MinimumDistanceFromPredator.ToString()).Append(',')
            .Append(frame.energyToGiveBirth.ToString()).Append(',').Append(frame.NumberOfChildren.ToString()).Append(',').Append(frame.MaxEnergy.ToString()).Append(',').Append(frame.InitialEnergy.ToString())

            .Append(',').Append(keyFramesPredator[i].Count.ToString()).Append(',').Append(keyFramesPredator[i].CountMale.ToString()).Append(',').Append(keyFramesPredator[i].CountFemale.ToString())
            .Append(',').Append(keyFramesPredator[i].ViewRadius.ToString()).Append(',').Append(keyFramesPredator[i].ImpulseTime.ToString())
            .Append(',').Append(keyFramesPredator[i].SoshakuJikan.ToString()).Append(',').Append(keyFramesPredator[i].MatingRestJikan.ToString()).Append(',').Append(keyFramesPredator[i].MatingAge.ToString())
            .Append(',').Append(keyFramesPredator[i].Jumyo.ToString()).Append(',').Append(keyFramesPredator[i].PotentialBenefitOfMovement.ToString()).Append(',').Append(keyFramesPredator[i].MinimumEnergyToMate.ToString())
            .Append(',').Append(keyFramesPredator[i].MinimumLifeToEat.ToString()).Append(',').Append(keyFramesPredator[i].RelativeBenefitParameter.ToString()).Append(',').Append(keyFramesPredator[i].RelativeCostParameter.ToString())
            .Append(',').Append(keyFramesPredator[i].energyToGiveBirth.ToString()).Append(',').Append(keyFramesPredator[i].NumberOfChildren.ToString()).Append(',').Append(keyFramesPredator[i].MaxEnergy.ToString()).Append(',').Append(keyFramesPredator[i].InitialEnergy.ToString());
            i++;
        }

        return sb.ToString();
    }
    public void SaveToFile ()
    {
        // Use the CSV generation from before
        var content = ToCSV();

        // The target file path e.g.
    #if UNITY_EDITOR
        var folder = Application.streamingAssetsPath;

        if(! Directory.Exists(folder) )Directory.CreateDirectory(folder);
    #else
        var folder = Application.persistentDataPath;
    #endif

        var filePath = Path.Combine(folder, "export.csv");

        using(var writer = new StreamWriter(filePath, false))
        {
            writer.Write(content);
        }

        // Or just
        //File.WriteAllText(content);

        Debug.Log($"CSV file written to \"{filePath}\"");

    #if UNITY_EDITOR
        AssetDatabase.Refresh();
    #endif
    }
}
