using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Take time from a start point.
/// </summary>
public struct Stopwatch : IEquatable<Stopwatch>
{
    /// <summary>
    /// Unique integer value.
    /// </summary>
    public readonly int ID;

    /// <summary>
    /// How much time that has passed.
    /// </summary>
    public float Timer
    {
        get
        {
            if (!_paused)
                Update();

            return _timer;
        }
    }
    /// <summary>
    /// If the stopwatch is currently paused or not.
    /// </summary>
    public bool IsPaused { get => _paused; }
    /// <summary>
    /// Initialized time length.
    /// </summary>
    public float InitTime { get => _initTime; }

    private static int _counter;

    private float _oldGameTime;
    private float _pauseGameTime;
    [SerializeField] private float _timer;
    [SerializeField] private float _initTime;
    [SerializeField] private bool _paused;

    public bool Equals(Stopwatch other) => ID == other.ID;
    public override bool Equals(object obj) => (obj != null) ? obj.GetHashCode() == GetHashCode() : false;
    public override int GetHashCode() => ID;
    public override string ToString() => Timer.ToString();

    public bool Paused()
    {
        if (!_paused)
            _pauseGameTime = Time.time;

        return _paused = true;
    }
    public bool Resume()
    {
        if (_paused)
        {
            float currentTime = Time.time;
            float deltaTime = currentTime - _pauseGameTime;
            _pauseGameTime = currentTime;
            _oldGameTime += deltaTime;
        }

        return _paused = false;
    }

    public void Reset()
    {
        _timer = _initTime;
        _oldGameTime = Time.time;
        Resume();
    }

    public void Set(float time)
    {
        _oldGameTime = Time.time;
        _pauseGameTime = _oldGameTime;
        _paused = false;
        _timer = time;
        _initTime = time;
    }

    public Stopwatch(float initTime)
    {
        ID = ++_counter;
        _oldGameTime = Time.time;
        _pauseGameTime = _oldGameTime;
        _paused = false;
        _timer = initTime;
        _initTime = initTime;
    }

    public static Stopwatch Initialize() => new Stopwatch(0);

    public static bool operator ==(Stopwatch lhs, Stopwatch rhs) => lhs.ID == rhs.ID;
    public static bool operator !=(Stopwatch lhs, Stopwatch rhs) => lhs.ID != rhs.ID;
    public static bool operator >(Stopwatch lhs, Stopwatch rhs) => lhs.Timer > rhs.Timer;
    public static bool operator <(Stopwatch lhs, Stopwatch rhs) => lhs.Timer < rhs.Timer;
    public static bool operator <=(Stopwatch lhs, Stopwatch rhs) => lhs.Timer <= rhs.Timer;
    public static bool operator >=(Stopwatch lhs, Stopwatch rhs) => lhs.Timer >= rhs.Timer;
    public static float operator +(Stopwatch lhs, Stopwatch rhs) => lhs._timer + rhs.Timer;
    public static float operator -(Stopwatch lhs, Stopwatch rhs) => lhs._timer - rhs.Timer;
    public static float operator +(Stopwatch lhs, float rhs) => lhs._timer + rhs;
    public static float operator -(Stopwatch lhs, float rhs) => lhs._timer - rhs;
    public static float operator +(float lhs, Stopwatch rhs) => lhs + rhs.Timer;
    public static float operator -(float lhs, Stopwatch rhs) => lhs - rhs.Timer;

    private void Update()
    {
        if (_paused)
            return;

        float currentTime = Time.time;
        float deltaTime = currentTime - _oldGameTime;
        _oldGameTime = currentTime;

        _timer += deltaTime;
    }
}
