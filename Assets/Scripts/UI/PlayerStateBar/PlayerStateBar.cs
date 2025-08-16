using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStateBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image energyImage;
    private void Update()
    {
        if(healthDelayImage.fillAmount>healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime ;
        }
    }
    public void onHealthChange(float percentage)
    {

        healthImage.fillAmount = percentage;
       
    }
}
