using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EvaluationEngine : MonoBehaviour
{
    public UnitRelatedEvaluation UnitRelatedEvaluation;
    public BuildingRelatedEvaluation BuildingRelatedEvaluation;

    public static T LoadAssetByName<T>(string assetName) where T : class
    {
        string[] guids = AssetDatabase.FindAssets(assetName, null);
        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        T asset = AssetDatabase.LoadAssetAtPath
            (assetPath, typeof(T)) as T;

        return asset;
    }

    public int Evaluate(string moveType, Object source, Object target)
    {
        return 0;
    }

    public int EvaluateRecruitment()
    {
        return 4;
    }

    public int EvaluateBuilding()
    {
        return 5;
    }
}
