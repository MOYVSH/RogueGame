using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeUpdateManager : UnitySingleton<TimeUpdateManager>
{
    public Action OnDayRefresh = delegate { };

    //public void StartTimer()
    //{
    //    if (_timer == null)
    //    {
    //        _timer = new TimedItem();
    //        _timer.OnExpired += OnTimeExpired;
    //    }
    //    _timer.RefreshDateExpiredCountdown
    //        (DataFormater.GetDayRefreshTime(CorrectionTime.Instance.GetCorrectionCurrentTimeStamp()));
    //}

    //public void KillTimer()
    //{
    //    if (_timer != null)
    //    {
    //        _timer.Destroy();
    //        _timer = null;
    //    }
    //}

    //private void OnTimeExpired()
    //{
    //    OnDayRefresh();
    //    StartTimer();
    //}

    //private TimeUpdateManager()
    //{
    //    // Keep empty
    //}

    //private TimedItem _timer;
}
