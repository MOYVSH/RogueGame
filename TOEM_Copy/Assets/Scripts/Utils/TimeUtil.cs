using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeUtil
{

    private static DateTime timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// 13位时间戳转化为时间
    /// </summary>
    /// <param name="_utcTime">时间戳</param>
    /// <returns></returns>
    public static DateTime ConvertUtcToDateTime(string _utcTime)
    {
        DateTime dt = TimeZoneInfo.ConvertTimeToUtc(timeStampStartTime);
        long lTime = long.Parse(_utcTime + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dt.Add(toNow);
    }

    /// <summary>
    /// 时间转化为13位时间戳
    /// </summary>
    /// <param name="_time">获取的时间</param>
    /// <returns></returns>
    public static long ConvertDateTimeToUtc_13(DateTime _time)
    {
        TimeSpan timeSpan = _time.ToUniversalTime() - timeStampStartTime;
        return Convert.ToInt64(timeSpan.TotalMilliseconds);
    }


    ///// <summary>
    ///// 生成十位时间戳
    ///// </summary>
    ///// <param name="dateTime">现在的时间</param>
    ///// <returns></returns>
    //public static long DateTimeToTimeStamp(DateTime dateTime)
    //{
    //    return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalSeconds;
    //}

    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.bool bflag = true</param>
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
    /// 获取两时间之间的时间差
    /// </summary>
    /// <param name="_time1">时间1</param>
    /// <param name="_time2">时间2</param>
    /// <returns></returns>
    public static TimeSpan GetTwoTimeDuration(DateTime _time1, DateTime _time2)
    {
        TimeSpan span1 = new TimeSpan(_time1.Ticks);
        TimeSpan span2 = new TimeSpan(_time2.Ticks);
        TimeSpan result = span1.Subtract(span2).Duration();

        //int hours = result.Hours;               //小时
        //int minutes = result.Minutes;           //分钟
        //int seconds = result.Seconds;           //秒

        return span1.Subtract(span2).Duration();
    }
}
