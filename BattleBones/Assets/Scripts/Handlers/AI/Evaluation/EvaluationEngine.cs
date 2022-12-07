using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EvaluationEngine : MonoBehaviour
{
    public UnitRelatedEvaluation UnitRelatedEvaluation;
    public BuildingRelatedEvaluation BuildingRelatedEvaluation;
    public string aiName;

    void Awake()
    {
        switch(aiName)
        {
            case "DefensiveCharacter":
                UnitRelatedEvaluation.CustomEval = new DefensiveCharacter();
                break;
            default:
                UnitRelatedEvaluation.CustomEval = new AggressiveCharacter();
                break;
        }
    }

    //public static T LoadAssetByName<T>(string assetName) where T : class
    //{
    //    string[] guids = AssetDatabase.FindAssets(assetName, null);
    //    string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
    //    T asset = AssetDatabase.LoadAssetAtPath
    //        (assetPath, typeof(T)) as T;

    //    return asset;
    //}

    public int Evaluate(string moveType, Object source, Object target)
    {
        int eval = 0;

        switch (moveType)
        {
            case "unitAttack" or "move":
                eval = UnitRelatedEvaluation.Evaluate(moveType, source, target);
                break;
            case "recruitment" or "buildingAttack" or "construction":
                eval = BuildingRelatedEvaluation.Evaluate(moveType, source, target);
                break;
            default:
                break;
        }

        return eval;
    }
}
