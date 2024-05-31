using System.Collections;
using UnityEngine;

/// <summary>曲がる球の処理</summary>
public class RotationMove : BulletClass
{
    public override IEnumerator BulletMove(BulletMoveScripts bulletMove, float bulletSpeed, float bulletRota)
    {
        //ActiveTime秒、for文を回す。
        for (float i = 0f; i <= bulletMove.ActiveTime; i += Time.deltaTime)
        {
            //回転処理
            bulletMove.Rotation(bulletRota);
            //弾の動き
            bulletMove.Move(bulletSpeed);
            //当たり判定
            if (bulletMove.ChackHit()) { break; }
            yield return new WaitForFixedUpdate();
        }
        //初期化処理
        bulletMove.Reset();
    }
}
