using System.Collections;
using UnityEngine;

/// <summary>まっすぐ飛ぶ球の処理</summary>
public class ForwardMove : BulletClass
{
    public override IEnumerator BulletMove(BulletMoveScripts bulletMove, float bulletSpeed)
    {
        for (float i = 0f; i <= bulletMove.ActiveTime; i += Time.deltaTime)
        {
            bulletMove.Move(bulletSpeed);
            if (bulletMove.ChackHit()) { break; }
            yield return new WaitForFixedUpdate();
        }
        bulletMove.Reset();
    }
}
