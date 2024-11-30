using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatCodeField : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cheatCodeText;
    [SerializeField] private GameObject placeHolderText;

    void Update()
    {
        if (cheatCodeText.text.Length > 1)
        {
            placeHolderText.SetActive(false);
        }
    }
}
