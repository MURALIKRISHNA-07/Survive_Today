using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadTimer : MonoBehaviour
{
    [SerializeField] private Image timer;
    [SerializeField] private Text timerText;


    public  float count = 5;
    private float interval = 0f;
    private  float totatTime = 0;

    private void OnEnable()
    {
        timer.fillAmount = 0f;
        interval = 0f;
        totatTime = 0;
        StartCoroutine(StartCountDown());

    }
  
    IEnumerator StartCountDown()
    {
        while (interval < count)
        {
            interval += Time.deltaTime;

            yield return new WaitForEndOfFrame();
            timerText.text = "Reloading..";
            timer.fillAmount += Time.deltaTime / count;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }

}
