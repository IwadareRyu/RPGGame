using System.Collections;
using UnityEngine;

/// <summary>‚Ü‚Á‚·‚®”ò‚Ô‹…‚Ìˆ—</summary>
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
