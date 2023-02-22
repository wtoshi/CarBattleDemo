using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBar : MonoBehaviour
{
    public Image flagImg;
    public TextMeshProUGUI entityName;

    private void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void Init(Sprite countryImg, string playerName)
    {
        flagImg.sprite = countryImg;
        entityName.text = playerName;
    }
}
