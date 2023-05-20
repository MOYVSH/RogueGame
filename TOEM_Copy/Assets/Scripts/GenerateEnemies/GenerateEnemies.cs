using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTimer;

namespace GenerateEnemies
{
    public class GenerateEnemies : MonoBehaviour
    {
        public GenerateData GD;

        private int currentRound = 0;  //当前轮次
        private float currentTime = 0; //当前时间 以秒为单位
        private bool currentIsRounding = false; //当前状态 是否在轮次内

        //当前轮次信息
        private RoundData thisRoundData
        {
            get
            {
                return GD.RoundDatas[currentRound - 1];
            }
        }
        //本轮刷怪曲线
        private AnimationCurve thisCurve
        {
            get
            {
                return thisRoundData.Curve; 
            }
        }
        //本轮总时长
        private float thisRoundTime
        {
            get
            {
                return thisRoundData.RoundTime;    
            }
        }
        //本轮的波次间隔
        private float thisIntervalTime
        {
            get
            {
                return thisRoundData.IntervalTime; 
            }
        }
        //本轮刷怪的阈值
        private int thisThreshold
        {
            get
            {
                return thisRoundData.Threshold;      
            }
        }

        CountTickClock clockGenerateEnemies;

        private void Start()
        {
            BeginRound();
        }
        public void BeginRound()
        {
            if (currentIsRounding == true)
            {
                return;
            }
            currentRound = 1;
            currentTime = 0;
            currentIsRounding = true;

            if (clockGenerateEnemies == null)
            {
                clockGenerateEnemies = UnityTimerMgr.CreateSecondClock(GenerateHandle);
            }
            //Debug.LogError("BeginRound");
            clockGenerateEnemies.Start();
        }

        public void EndRound()
        {
            //Debug.LogError("EndRound");
            currentIsRounding = false;
            clockGenerateEnemies.Stop();
        }

        public void GenerateHandle(long Stamp)
        {
            if (currentTime >= thisRoundTime)
            {
                NextRound();
                return;
            }
            currentTime += 1;
            Debug.LogError(currentTime % thisIntervalTime);
            if (currentTime % thisIntervalTime == 0)
            {
                //Debug.LogError("到时间了生成一次");
                // 到时间了生成一次
                int count = (int)Math.Ceiling(thisCurve.Evaluate(currentTime / 10) * 10);
                for (int i = 0; i < count; i++)
                {
                    GameManager.Instance.GenerateHostile(thisRoundData.GeneratePoints[i % thisRoundData.GeneratePoints.Count()], 0);
                }
            }
            //else if (GameManager.Instance.Hostiles.Count <= thisThreshold)
            //{
            //    // 敌人数量过低生成一次
            //}
        }

        public void NextRound()
        {
            currentRound++;
            if (currentRound > GD.RoundCount)
            {
                EndRound();
            }
        }
    }
}

