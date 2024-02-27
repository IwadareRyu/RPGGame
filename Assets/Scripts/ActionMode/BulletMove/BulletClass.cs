using System.Collections;

public class BulletClass
{
    public virtual IEnumerator BulletMove(BulletMoveScripts bulletMove, float bulletSpeed) { yield return null; }

    public virtual IEnumerator BulletMove(BulletMoveScripts bulletMove, float bulletSpeed, float bulletRota) { yield return null; }
}
