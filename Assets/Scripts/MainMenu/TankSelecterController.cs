using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankSelecterController : MonoBehaviour
{
    public Image frameImage;
    public Button[] allTankItemButton;

    int oldSelected;
    // Start is called before the first frame update
    void Start()
    {
        FillTankItem();
    }

    
    void FillTankItem()
    {
        if (PlayerPrefs.HasKey("TankSelected"))
        {
            SetFrameSelectedPosition(PlayerPrefs.GetInt("TankSelected"));
        }
        else
        {
            PlayerPrefs.SetInt("TankSelected", 0);
        }
    }

    public void ClickSelectedItem(int index)
    {
        if (oldSelected != index)
        {
            PlayerPrefs.SetInt("TankSelected", index);
            SetFrameSelectedPosition(index);
        }
    }

    void SetFrameSelectedPosition(int index)
    {
        frameImage.transform.SetParent(allTankItemButton[index].transform);
        frameImage.transform.localPosition = Vector3.zero;
        oldSelected = index;
    }
}
