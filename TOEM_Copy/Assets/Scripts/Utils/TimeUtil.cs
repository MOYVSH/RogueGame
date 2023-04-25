using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeUtil
{

    private static DateTime timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// 13λʱ���ת��Ϊʱ��
    /// </summary>
    /// <param name="_utcTime">ʱ���</param>
    /// <returns></returns>
    public static DateTime ConvertUtcToDateTime(string _utcTime)
    {
        DateTime dt = TimeZoneInfo.ConvertTimeToUtc(timeStampStartTime);
        long lTime = long.Parse(_utcTime + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dt.Add(toNow);
    }

    /// <summary>
    /// ʱ��ת��Ϊ13λʱ���
    /// </summary>
    /// <param name="_time">��ȡ��ʱ��</param>
    /// <returns></returns>
    public static long ConvertDateTimeToUtc_13(DateTime _time)
    {
        TimeSpan timeSpan = _time.ToUniversalTime() - timeStampStartTime;
        return Convert.ToInt64(timeSpan.TotalMilliseconds);
    }


    ///// <summary>
    ///// ����ʮλʱ���
    ///// </summary>
    ///// <param name="dateTime">���ڵ�ʱ��</param>
    ///// <returns></returns>
    //public static long DateTimeToTimeStamp(DateTime dateTime)
    //{
    //    return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalSeconds;
    //}

    /// <summary>
    /// ��ȡ��ǰʱ���
    /// </summary>
    /// <param name="bflag">Ϊ��ʱ��ȡ10λʱ���,Ϊ��ʱ��ȡ13λʱ���.bool bflag = true</param>
    /// <returns></returns>
    public static long GetCurrentTimeStamp(bool bflag)
    {
        TimeSpan ts = DateTime.UtcNow - timeStampStartTime;
        long ret = 0;
        if (bflag)
            ret = Convert.ToInt64(ts.TotalSeconds);
        else
            ret = Convert.ToInt64(ts.TotalMilliseconds);
        return ret;
    }

    public static TimeSpan GetCurrentTimeSpan()
    { 
        return DateTime.UtcNow - timeStampStartTime;
    }

    /// <summary>
    /// ��ȡ��ʱ��֮���ʱ���
    /// </summary>
    /// <param name="_time1">ʱ��1</param>
    /// <param name="_time2">ʱ��2</param>
    /// <returns></returns>
    public static TimeSpan GetTwoTimeDuration(DateTime _time1, DateTime _time2)
    {
        TimeSpan span1 = new TimeSpan(_time1.Ticks);
        TimeSpan span2 = new TimeSpan(_time2.Ticks);
        TimeSpan result = span1.Subtract(span2).Duration();

        //int hours = result.Hours;               //Сʱ
        //int minutes = result.Minutes;           //����
        //int seconds = result.Seconds;           //��

        return span1.Subtract(span2).Duration();
    }
}
