using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Count down a duration.
/// </summary>
[Serializable]
public struct Duration : IEquatable<Duration>
{
    /// <summary>
    /// Unique integer value.
    /// </summary>
    public readonly int ID;
    /// <summary>
    /// If the duration still is active or not.
    /// </summary>
    public bool IsActive {
        get
        {
            if (!_paused)
                Update();

            return _isActive;
        }
    }
    /// <summary>
    /// How much time is left in seconds.
    /// </summary>
    public float TimeLeft
    {
        get
        {
            if (!_paused)
                Update();

            return _timeLeft;
        }
    }
    /// <summary>
    /// If the duration is currently paused or not.
    /// </summary>
    public bool IsPaused { get => _paused; }
    /// <summary>
    /// Initialized time length.
    /// </summary>
    public float InitTime { get => _initTime; }

    private float _oldGameTime;
    private float _pauseGameTime;
    [SerializeField] private float _timeLeft;
    [SerializeField] private float _initTime;
    [SerializeField] private bool _paused;
    [SerializeField] private bool _isActive;

    public bool Equals(Duration other) => ID == other.ID;
    public override bool Equals(object obj) => (obj != null) ? obj.GetHashCode() == GetHashCode() : false;
    public override int GetHashCode() => ID;
    public override string ToString() => IsActive.ToString();

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
        _timeLeft = _initTime;
        _oldGameTime = Time.time;
        Resume();
    }

    public void Done()
    {
        _timeLeft = 0;
        Paused();
    }

    public void Set(float duration)
    {
        _oldGameTime = Time.time;
        _pauseGameTime = _oldGameTime;
        _isActive = true;
        _paused = false;
        _timeLeft = duration;
        _initTime = duration;
    }

    public Duration(float duration)
    {
        ID = Guid.NewGuid().GetHashCode();
        _oldGameTime = Time.time;
        _pauseGameTime = _oldGameTime;
        _isActive = true;
        _paused = false;
        _timeLeft = duration;
        _initTime = duration;
    }

    public static bool operator ==(Duration lhs, Duration rhs) => lhs.ID == rhs.ID;
    public static bool operator !=(Duration lhs, Duration rhs) => lhs.ID != rhs.ID;
    public static bool operator >(Duration lhs, Duration rhs) => lhs.TimeLeft > rhs.TimeLeft;
    public static bool operator <(Duration lhs, Duration rhs) => lhs.TimeLeft < rhs.TimeLeft;
    public static bool operator <=(Duration lhs, Duration rhs) => lhs.TimeLeft <= rhs.TimeLeft;
    public static bool operator >=(Duration lhs, Duration rhs) => lhs.TimeLeft >= rhs.TimeLeft;
    public static float operator +(Duration lhs, Duration rhs) => lhs._timeLeft + rhs.TimeLeft;
    public static float operator -(Duration lhs, Duration rhs) => lhs._timeLeft - rhs.TimeLeft;
    public static float operator +(Duration lhs, float rhs) => lhs._timeLeft + rhs;
    public static float operator -(Duration lhs, float rhs) => lhs._timeLeft - rhs;
    public static float operator +(float lhs, Duration rhs) => lhs + rhs.TimeLeft;
    public static float operator -(float lhs, Duration rhs) => lhs - rhs.TimeLeft;

    private void Update()
    {
        if (_paused)
            return;

        float currentTime = Time.time;
        float deltaTime = currentTime - _oldGameTime;
        _oldGameTime = currentTime;

        if (_timeLeft - deltaTime >= 0)
        {
            _isActive = true;
             _timeLeft -= deltaTime;
        }
        else
        {
            _isActive = false;
            _timeLeft = 0;
        }
    }
}
