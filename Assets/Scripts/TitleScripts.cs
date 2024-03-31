using Cinemachine;
using UnityEngine;

public class TitleScripts : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _titleCamera;

    // Start is called before the first frame update
    void Start()
    {
        FightManager.Instance.CanvasDisplay();
        PauseManager.PauseResume();
        AudioManager.Instance.BGMPlay(BGM.Title);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ActionDecition"))
        {
            AudioManager.Instance.SEPlay(SE.Click);
            AudioManager.Instance.BGMPlay(BGM.RPGPart);
            FightManager.Instance.CanvasDisplay();
            PauseManager.PauseResume();
            _titleCamera.enabled = false;
            this.enabled = false;
        }
    }

    public void ReturnTitle()
    {
        FightManager.Instance.CanvasDisplay();
        PauseManager.PauseResume();
        _titleCamera.enabled = true;
    }
}
