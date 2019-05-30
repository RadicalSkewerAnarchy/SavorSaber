using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SerializableCollections;

public enum TimeOfDay
{
    Dawn,
    Sunrise,
    Morning,
    Day,
    Evening,
    Sunset,
    Dusk,
    Night,
}

public class DayNightController : MonoBehaviour, IPausable
{
    public static DayNightController instance;
    public const int hoursPerDay = 24;
    private static readonly int numPhases = EnumUtils.Count<TimeOfDay>();

    public bool IsDayTime
    {
        get
        {
            return CurrTimeOfDay == TimeOfDay.Morning || CurrTimeOfDay == TimeOfDay.Day || CurrTimeOfDay == TimeOfDay.Evening;
        }
    }

    public bool IsNightTime
    {
        get
        {
            return CurrTimeOfDay == TimeOfDay.Dusk || CurrTimeOfDay == TimeOfDay.Night || CurrTimeOfDay == TimeOfDay.Dawn;
        }
    }

    public bool Paused { get; set; }

    [SerializeField] private float _lengthOfDay = 240;
    public float LengthOfDay
    {
        get { return _lengthOfDay; }
        set
        {
            _lengthOfDay = value;
            _gameHour = _lengthOfDay / 24;
        }
    }
    [SerializeField] private float _gameHour = 10;
    public float GameHour
    {
        get { return _gameHour; }
        set
        {
            _gameHour = value;
            _lengthOfDay = GameHour * 24;
        }
    }

    /// <summary> The time in Game Hours it takes to transition for one TimeOfDay to the next</summary>
    public float transitionTime = 1;
    [SerializeField] private TimeOfDay currTimeOfDay = TimeOfDay.Day;
    public TimeOfDay CurrTimeOfDay { get => currTimeOfDay; private set => currTimeOfDay = value; }

    // Weather and Season dat

    /// <summary> The weather object containing lighting data for clear weather. 
    /// Should have lightingOverride values defined for ALL TimeOfDay cases 
    /// May be swapped out after a season change </summary>
    public WeatherData clearWeather;
    public WeatherData currWeather;
    /// <summary>
    /// A dictionary containing the length of every time of day phase in game hours
    /// May be swapped out after a season change
    /// </summary>
    public TimeTableDict timeTable = new TimeTableDict()
    {
        { TimeOfDay.Dawn, 2 },
        { TimeOfDay.Sunrise, 1 },
        { TimeOfDay.Morning, 2 },
        { TimeOfDay.Day, 7 },
        { TimeOfDay.Evening, 2 },
        { TimeOfDay.Sunset, 1 },
        { TimeOfDay.Dusk, 2 },
        { TimeOfDay.Night, 7 },
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(AdvanceTime(true));
    }

    private IEnumerator AdvanceTime(bool noTransition = false)
    {
        float currTime = 0;
        float goalTime = GameHour * Mathf.Max(0, (timeTable[CurrTimeOfDay] - (noTransition ? transitionTime / 2 : transitionTime)));
        while (currTime < goalTime)
        {           
            yield return new WaitWhile(() => Paused);
            yield return new WaitForEndOfFrame();
            currTime += Time.deltaTime;                     
        }
        TransitionToTimeOfDay(CurrTimeOfDay.Next(), transitionTime);
    }

    public void Next()
    {
        StopAllCoroutines();
        TransitionToTimeOfDay(currTimeOfDay.Next(), transitionTime);
    }

    public void TransitionToTimeOfDay(TimeOfDay t, float transitionTime)
    {
        StartCoroutine(transition(t, transitionTime));
    }

    private IEnumerator transition(TimeOfDay t, float transitionTime)
    {
        float currTime = 0;
        float goalTime = GameHour * transitionTime;
        Color startColor = currWeather.lightingOverrides.ContainsKey(CurrTimeOfDay) ? 
                           currWeather.lightingOverrides[CurrTimeOfDay].lightColor : clearWeather.lightingOverrides[CurrTimeOfDay].lightColor;
        Color endColor = currWeather.lightingOverrides.ContainsKey(t) ?
                         currWeather.lightingOverrides[t].lightColor : clearWeather.lightingOverrides[t].lightColor;
        while (currTime < goalTime)
        {
            yield return new WaitWhile(() => Paused);
            yield return new WaitForEndOfFrame();
            currTime += Time.deltaTime;
            var percentage = currTime / goalTime;
            RenderSettings.ambientLight = Color.Lerp(startColor, endColor, percentage);
            if (percentage > 0.5f)
                CurrTimeOfDay = t;
        }
        StartCoroutine(AdvanceTime());
    }

    public void SetTimeOfDay(TimeOfDay t)
    {
        StopAllCoroutines();
        if (Paused)
        {
            RenderSettings.ambientLight = currWeather.lightingOverrides.ContainsKey(t) ?
             currWeather.lightingOverrides[t].lightColor : clearWeather.lightingOverrides[t].lightColor;
            CurrTimeOfDay = t;
            StartCoroutine(AdvanceTime(true));
        }
        else
            StartCoroutine(transition(t, 0.15f));
    }
    public void SetTimeOfDayImmediate(TimeOfDay t)
    {
        StopAllCoroutines();
        CurrTimeOfDay = t;
        RenderSettings.ambientLight = currWeather.lightingOverrides.ContainsKey(t) ?
            currWeather.lightingOverrides[t].lightColor : clearWeather.lightingOverrides[t].lightColor;
        CurrTimeOfDay = t;
        StartCoroutine(AdvanceTime(true));
    }

    public void GoToNight()
    {
        SetTimeOfDayImmediate(TimeOfDay.Night);
    }

    [System.Serializable] public class TimeTableDict : SDictionary<TimeOfDay, float>
    {
        public bool CheckSum()
        {
            return this.Sum(item => item.Value) == hoursPerDay;
        }
    }
}
