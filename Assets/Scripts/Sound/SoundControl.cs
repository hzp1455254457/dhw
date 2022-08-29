using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    // Start is called before the first frame update
  public  Toggle Musictoggle;
    public Toggle Shaketoggle;
    public GameObject[] musicGos;
    public GameObject[] shakeGos;
    private void Start()
    {
      
        Musictoggle.onValueChanged.AddListener(SoundsControl);
        Shaketoggle.onValueChanged.AddListener(ShakeControl);
        Init();
    }
    public void ShakeControl(bool value)
    {
        AudioManager.Instance.SetShake(value);
        SetShakeUI(value);
    }
    
    public void SoundsControl(bool value)
    {
        AudioManager.Instance.SetSound(value);
        AudioManager.Instance.SetMusic(value);
        SetMusicUI(value);
    }
    private void Init()
    {
        Musictoggle.isOn = AudioManager.Instance.isOpenMusic;
       
        Shaketoggle.isOn = AudioManager.Instance.isOpenShake;
        SetStatus(Musictoggle.isOn, Shaketoggle.isOn);
    }
    void SetStatus(bool value,bool value1)
    {
        SetMusicUI(value);
        SetShakeUI(value1);
    }

    private void SetShakeUI(bool value1)
    {
        shakeGos[0].SetActive(value1);
        shakeGos[1].SetActive(!value1);
    }

    private void SetMusicUI(bool value)
    {
        musicGos[0].SetActive(value);
        musicGos[1].SetActive(!value);
    }
}
