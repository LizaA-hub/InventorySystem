using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public Sprite _default , _hovered , _pressed;
    public Image buttonImage;

#region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        buttonImage.sprite = _default;
    }

#endregion

#region Public Functions
    public void Hovered(){
        buttonImage.sprite = _hovered;
    }

    public void Pressed(){
        buttonImage.sprite = _pressed;
    }

    public void Default(){
        buttonImage.sprite = _default;
    }
#endregion

}
