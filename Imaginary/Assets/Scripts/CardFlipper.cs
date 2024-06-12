using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    public Sprite CardFront;
    public Sprite CardBack;
    public bool IsFront => gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite == CardFront;

    public void Flip()
    {
        //when Flip() is called, store the value of the current sprite attached to this gameobject
        Sprite currentSprite = gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite;
        

        //conditional logic to determine whether to display the card front or back sprite
        if (currentSprite == CardFront)
        {
            gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CardBack;
        }
        else
        {
            gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CardFront;
        }
    }
}
