using System.Collections;
using UnityEngine;

/// <summary>’e‚ª‰Ôó‚É“®‚­ˆ—</summary>
public class HalfRotationMove : BulletClass
{
    public override IEnumerator BulletMove(BulletMoveScripts bulletMove, float bulletSpeed, float bulletRota)
    {
        float tmpRota = 0;
        float nowRota = 0;
        for (float i = 0f; i <= bulletMove.ActiveTime; i += Time.deltaTime)
        {
            if (nowRota < tmpRota + 270)
            {
                bulletMove.Rotation(bulletRota);
                nowRota += bulletRota;
            }
            bulletMove.Move(bulletSpeed);
            if (bulletMove.ChackHit()) { break; };
            yield return new WaitForFixedUpdate();
        }
        bulletMove.Reset();
    }
}
