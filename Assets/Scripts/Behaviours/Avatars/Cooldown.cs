using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

[Serializable]
public struct Cooldown : IEquatable<Cooldown>
{
    /// <summary>
    /// Unique integer value.
    /// </summary>
    public readonly int ID;
    /// <summary>
    /// If the cooldown still is active or not.
    /// </summary>
    public bool IsActive {
        get
        {
            if (!_paused)
                UpdateCooldown();

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
                UpdateCooldown();

            return _timeLeft;
        }
    }
    /// <summary>
    /// If the cooldown is currently paused or not.
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

    public bool Equals(Cooldown other) => ID == other.ID;
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

    public Cooldown(float time)
    {
        ID = Guid.NewGuid().GetHashCode();
        _oldGameTime = Time.time;
        _pauseGameTime = _oldGameTime;
        _isActive = true;
        _paused = false;
        _timeLeft = time;
        _initTime = time;
    }

    public static bool operator ==(Cooldown lhs, Cooldown rhs) => lhs.ID == rhs.ID;
    public static bool operator !=(Cooldown lhs, Cooldown rhs) => lhs.ID != rhs.ID;
    public static bool operator >(Cooldown lhs, Cooldown rhs) => lhs.TimeLeft > rhs.TimeLeft;
    public static bool operator <(Cooldown lhs, Cooldown rhs) => lhs.TimeLeft < rhs.TimeLeft;
    public static bool operator <=(Cooldown lhs, Cooldown rhs) => lhs.TimeLeft <= rhs.TimeLeft;
    public static bool operator >=(Cooldown lhs, Cooldown rhs) => lhs.TimeLeft >= rhs.TimeLeft;
    public static float operator +(Cooldown lhs, Cooldown rhs) => lhs._timeLeft + rhs.TimeLeft;
    public static float operator -(Cooldown lhs, Cooldown rhs) => lhs._timeLeft - rhs.TimeLeft;
    public static float operator +(Cooldown lhs, float rhs) => lhs._timeLeft + rhs;
    public static float operator -(Cooldown lhs, float rhs) => lhs._timeLeft - rhs;
    public static float operator +(float lhs, Cooldown rhs) => lhs + rhs.TimeLeft;
    public static float operator -(float lhs, Cooldown rhs) => lhs - rhs.TimeLeft;

    private void UpdateCooldown()
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