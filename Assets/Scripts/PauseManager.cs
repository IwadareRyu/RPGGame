using System;

public class PauseManager
{
    static bool _pause = false;
    public static event Action<bool> OnPauseResume;

    public static void PauseResume()
    {
        _pause = !_pause;
        OnPauseResume(_pause);
    }
}
