using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class RaidCountdown : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    private static DateTime targetDate = new DateTime(2019, 9, 20, 19, 0, 0, DateTimeKind.Utc);

    public void Update()
    {
        TimeSpan span = TimeTillRaid();
        text.text = string.Format("{0:D2} Days\n{1:D2} Hours\n{2:D2} Minutes\n{3:D2} Seconds", span.Days, span.Hours, span.Minutes, span.Seconds);
    }

    public static TimeSpan TimeTillRaid()
    {
        return targetDate - DateTime.UtcNow;
    }
}
