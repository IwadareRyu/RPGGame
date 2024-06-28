using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IViewCharaUI
{
    abstract void HPViewAccess();

    abstract void ChantingViewAccess(float currentChanting, float maxChanting);

}
