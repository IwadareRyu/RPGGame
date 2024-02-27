using System.Collections;
using UnityEngine;

/// <summary>‹È‚ª‚é‹…‚Ìˆ—</summary>
public class RotationMove : BulletClass
{
    public override IEnumerator BulletMove(BulletMoveScripts bulletMove, float bulletSpeed, float bulletRota)
    {
        for (float i = 0f; i <= bulletMove.ActiveTime; i += Time.deltaTime)
        {
            bulletMove.Rotation(bulletRota);
            bulletMove.Move(bulletSpeed);
            if (bulletMove.ChackHit()) { break; }
            yield return new WaitForFixedUpdate();
        }
        bulletMove.Reset();
    }
}
