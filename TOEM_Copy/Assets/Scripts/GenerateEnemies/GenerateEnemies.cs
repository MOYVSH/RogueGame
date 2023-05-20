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

        private int currentRound = 0;  //��ǰ�ִ�
        private float currentTime = 0; //��ǰʱ�� ����Ϊ��λ
        private bool currentIsRounding = false; //��ǰ״̬ �Ƿ����ִ���

        //��ǰ�ִ���Ϣ
        private RoundData thisRoundData
        {
            get
            {
                return GD.RoundDatas[currentRound - 1];
            }
        }
        //����ˢ������
        private AnimationCurve thisCurve
        {
            get
            {
                return thisRoundData.Curve; 
            }
        }
        //������ʱ��
        private float thisRoundTime
        {
            get
            {
                return thisRoundData.RoundTime;    
            }
        }
        //���ֵĲ��μ��
        private float thisIntervalTime
        {
            get
            {
                return thisRoundData.IntervalTime; 
            }
        }
        //����ˢ�ֵ���ֵ
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
                //Debug.LogError("��ʱ��������һ��");
                // ��ʱ��������һ��
                int count = (int)Math.Ceiling(thisCurve.Evaluate(currentTime / 10) * 10);
                for (int i = 0; i < count; i++)
                {
                    GameManager.Instance.GenerateHostile(thisRoundData.GeneratePoints[i % thisRoundData.GeneratePoints.Count()], 0);
                }
            }
            //else if (GameManager.Instance.Hostiles.Count <= thisThreshold)
            //{
            //    // ����������������һ��
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

