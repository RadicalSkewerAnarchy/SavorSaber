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

    public System.Action OnNight;
    public System.Action OnDay;

    public bool IsDayTime
    {
        get
        {
            return CurrTimeOfDay == TimeOfDay.Morning 
                || CurrTimeOfDay == TimeOfDay.Day 
                || CurrTimeOfDay == TimeOfDay.Evening 
                || CurrTimeOfDay == TimeOfDay.Sunrise;
        }
    }

    public bool IsNightTime
    {
        get
        {
            return CurrTimeOfDay == TimeOfDay.Dusk 
                || CurrTimeOfDay == TimeOfDay.Night 
                || CurrTimeOfDay == TimeOfDay.Dawn 
                || CurrTimeOfDay == TimeOfDay.Sunset;
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
    public TimeOfDay CurrTimeOfDay { get => currTimeOfDay; set => currTimeOfDay = value; }
    public TimeOfDay timeOfDayTransitionHolder;

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
            {
                CurrTimeOfDay = t;
                if (t == TimeOfDay.Dusk)
                    OnNight?.Invoke();
                else if (t == TimeOfDay.Morning)
                    OnDay?.Invoke();
            }
                
        }
        StartCoroutine(AdvanceTime());
    }

    public void SetTimeOfDay(TimeOfDay t)
    {
        StopAllCoroutines();
        if (Paused)
        {

            CurrTimeOfDay = t;
            UpdateCurrentLighting();
            if (IsNightTime)
                OnNight?.Invoke();
            else
                OnDay?.Invoke();
            StartCoroutine(AdvanceTime(true));
        }
        else
            StartCoroutine(transition(t, 0.075f));
    }
    public void SetTimeOfDayImmediate(TimeOfDay t, bool skipCallbacks = false)
    {
        StopAllCoroutines();
        CurrTimeOfDay = t;
        UpdateCurrentLighting();
        if (!skipCallbacks)
        {
            if (IsNightTime)
                OnNight?.Invoke();
            else
                OnDay?.Invoke();
        }
        StartCoroutine(AdvanceTime(true));
    }

    public void GoToDay()
    {
        SetTimeOfDayImmediate(TimeOfDay.Day);
    }

    public void GoToNight()
    {
        SetTimeOfDayImmediate(TimeOfDay.Night);
    }
    public void DarkNight(WeatherData data)
    {
        currWeather = data;
        GoToNight();
    }

    public void UpdateCurrentLighting()
    {
        RenderSettings.ambientLight = currWeather.lightingOverrides.ContainsKey(CurrTimeOfDay) ?
            currWeather.lightingOverrides[CurrTimeOfDay].lightColor : clearWeather.lightingOverrides[CurrTimeOfDay].lightColor;
    }

    [System.Serializable] public class TimeTableDict : SDictionary<TimeOfDay, float>
    {
        public bool CheckSum()
        {
            return this.Sum(item => item.Value) == hoursPerDay;
        }
    }
}
