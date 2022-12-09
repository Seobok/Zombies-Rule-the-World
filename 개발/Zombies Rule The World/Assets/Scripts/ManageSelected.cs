using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageSelected : MonoBehaviour
{
    public TextMeshProUGUI itemTitle;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI confirmText;
    
    [HideInInspector] public float contagious;
    [HideInInspector] public float cureDevelopProbability;
    [HideInInspector] public int price;

    [HideInInspector] public GameObject card;
    
    public void Cancel()
    {
        ObjectPools.Instance.ReleaseObjectToPool(gameObject);
    }

    public void Purchase()
    {
        if (GameManager.Instance.gene < price)
        {
            //살수없음
        }
        else
        {
            //구매
            GameManager.Instance.gene -= price;
            GameManager.Instance.AddContagious(contagious);
            GameManager.Instance.AddCureDevelopProbability(cureDevelopProbability);

            card.GetComponent<Image>().color = Color.black;
            card.GetComponent<Button>().interactable = false;
            
            ObjectPools.Instance.ReleaseObjectToPool(gameObject);
        }
    }
}
