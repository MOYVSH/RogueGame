using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GenerateEnemies
{
    [CreateAssetMenu(fileName = "Data1", menuName = "ScriptableObjects/SpawnManagerScriptableObject1", order = 1)]
    public class GenerateData : ScriptableObject
    {
        // 轮次
        public int RoundCount;
        public List<RoundData> RoundDatas;
    }

    [Serializable]
    public class RoundData
    {
        public float RoundTime;
        public float IntervalTime;
        public int Threshold;
        // 波次
        public AnimationCurve Curve;
        public List<Vector3> GeneratePoints;
        public List<EnemyInfo> Enemys;
    }

    [Serializable]
    public class EnemyInfo
    {
        public string Name;
        public int Count;
    }
}