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
    MinimumEnergyToMate,MinimumLifeToEat,RelativeBenefitParameter,RelativeCostParameter;
    public KeyFrame(){}

    public KeyFrame (float time, int count, int countMale, int countFemale, float viewRadius, float impulseTime,
    float kyuukaku, float soshakuJikan, float matingRestJikan, float matingAge, float jumyo, float potentialBenefitOfMovement,
    float minimumEnergyToMate, float minimumLifeToEat, float relativeBenefitParameter, float relativeCostParameter)
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
    }
}
public class SaveToCsv : MonoBehaviour
{
    float timer = 0;
    private List<KeyFrame> keyFrames = new List<KeyFrame>(60000);
    // Start is called before the first frame update
    void Start()
    {
        keyFrames.Add(new KeyFrame (Time.time, Fish.FishCount, Fish.FishCountMale, Fish.FishCountFemale,Fish.meanViewRadius,Fish.meanImpulseTime,Fish.meanKyuukaku,Fish.meanSoshakuJikan,Fish.meanMatingRestJikan,Fish.meanMatingAge,Fish.meanJumyo,Fish.meanPotentialBenefitOfMovement,
        Fish.meanMinimumEnergyToMate,Fish.meanMinimumLifeToEat,Fish.meanRelativeBenefitParameter,Fish.meanRelativeCostParameter));
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>1){
            keyFrames.Add(new KeyFrame (Time.time, Fish.FishCount, Fish.FishCountMale, Fish.FishCountFemale,Fish.meanViewRadius,Fish.meanImpulseTime,Fish.meanKyuukaku,Fish.meanSoshakuJikan,Fish.meanMatingRestJikan,Fish.meanMatingAge,Fish.meanJumyo,Fish.meanPotentialBenefitOfMovement,
            Fish.meanMinimumEnergyToMate,Fish.meanMinimumLifeToEat,Fish.meanRelativeBenefitParameter,Fish.meanRelativeCostParameter));
            timer = 0;
        }
        timer += Time.deltaTime;
    }
    public string ToCSV()
    {
        var sb = new StringBuilder("Time.time,Fish.FishCount,Fish.FishCountMale,Fish.FishCountFemale,Fish.meanViewRadius,Fish.meanImpulseTime,Fish.meanKyuukaku,Fish.meanSoshakuJikan,Fish.meanMatingRestJikan,Fish.meanMatingAge,Fish.meanJumyo,Fish.meanPotentialBenefitOfMovement,Fish.meanMinimumEnergyToMate,Fish.meanMinimumLifeToEat,Fish.meanRelativeBenefitParameter,Fish.meanRelativeCostParameter");
        foreach(var frame in keyFrames)
        {
            sb.Append('\n').Append(frame.Time.ToString())
            .Append(',').Append(frame.Count.ToString()).Append(',').Append(frame.CountMale.ToString()).Append(',').Append(frame.CountFemale.ToString())
            .Append(',').Append(frame.ViewRadius.ToString()).Append(',').Append(frame.ImpulseTime.ToString()).Append(',').Append(frame.Kyuukaku.ToString())
            .Append(',').Append(frame.SoshakuJikan.ToString()).Append(',').Append(frame.MatingRestJikan.ToString()).Append(',').Append(frame.MatingAge.ToString())
            .Append(',').Append(frame.Jumyo.ToString()).Append(',').Append(frame.PotentialBenefitOfMovement.ToString()).Append(',').Append(frame.MinimumEnergyToMate.ToString())
            .Append(',').Append(frame.MinimumLifeToEat.ToString()).Append(',').Append(frame.RelativeBenefitParameter.ToString()).Append(',').Append(frame.RelativeCostParameter.ToString());
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
