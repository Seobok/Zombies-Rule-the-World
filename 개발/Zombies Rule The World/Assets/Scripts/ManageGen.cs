using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageGen : MonoBehaviour
{
    public GameObject geneNum;
    private TextMeshProUGUI _geneNumText;

    public GameObject geneGraph;
    private Image _geneGraphImage;

    private void Awake()
    {
        _geneNumText = geneNum.GetComponent<TextMeshProUGUI>();
        _geneGraphImage = geneGraph.GetComponent<Image>();
        
        ChangeGene();
        GameManager.Instance.AddChangeGeneCallBack(ChangeGene);
    }

    public void ChangeGene()
    {
        _geneNumText.text = GameManager.Instance.gene.ToString();
        _geneGraphImage.fillAmount = (float)GameManager.Instance.gene / 100; //TODO ??
    }
}
