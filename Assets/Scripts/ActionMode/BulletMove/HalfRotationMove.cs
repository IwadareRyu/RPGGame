using System.Collections;
using UnityEngine;

/// <summary>曲線で飛んだあと一定時間後にまっすぐ飛ぶ処理</summary>
public class HalfRotationMove : BulletClass
{
    float _maxRota = 270f;
    public override IEnumerator BulletMove(BulletMoveScripts bulletMove, float bulletSpeed, float bulletRota)
    {
        float tmpRota = 0;
        float nowRota = 0;
        //ActiveTime秒、for文を回す。
        for (float i = 0f; i <= bulletMove.ActiveTime; i += Time.deltaTime)
        {
            // 一定まで回転したら回転処理を行わない。
            if (nowRota < tmpRota + _maxRota)
            {
                //回転処理
                bulletMove.Rotation(bulletRota);
                nowRota += bulletRota;
            }
            //弾の動き
            bulletMove.Move(bulletSpeed);
            //当たり判定
            if (bulletMove.ChackHit()) { break; };
            yield return new WaitForFixedUpdate();
        }
        //初期化処理
        bulletMove.Reset();
    }
}
