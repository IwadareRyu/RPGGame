using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineBullet : MonoBehaviour
{
    [Header("アクションモードの時間")]
    [SerializeField] float _actionTime = 30f;
    float _time;
    [SerializeField] Transform[] _targetPos;
    [SerializeField] Transform _bullet;
    [Header("duration:スタートから何秒後 target:どちらに攻撃するか")]
    [SerializeField] TimeBullet[] _timeBullet;
    int _timeBulletIndex = 0;
    bool _isPlaying;

    void Update()
    {
        if (!_isPlaying) { return; }

        _time += Time.deltaTime;

        if(_timeBulletIndex < _timeBullet.Length && _time > _timeBullet[_timeBulletIndex].duration)
        {
            var targetPos = _targetPos[(int)_timeBullet[_timeBulletIndex].target];
            Instantiate(_bullet,targetPos.position,targetPos.rotation);
            _timeBulletIndex++;
        }
    }

    public float ActionStart()
    {
        _isPlaying = true;
        return _actionTime;
    }

    public void ActionEnd()
    {
        _isPlaying = false;
    }


    [Serializable]
    struct TimeBullet
    {
        public float duration;
        public TargetPlayer target;
    }

    enum TargetPlayer
    {
        Magician,
        Attacker,
    }
}
