using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Score3dtext : MonoBehaviour
{
    [SerializeField] TMP_Text mytext;
    Vector3 initialscale;
    void Start()
    {
        
        initialscale = transform.localScale;
       
    }



    public void ScoreShow(int scorenumber,Transform NewPos) 
    {
        DOTween.Kill(this, true);
        var pos = transform.position;                   // Put the 3d text on bait position
        pos.x = NewPos.position.x;
        pos.z = NewPos.position.z;
        transform.position = pos;

        transform.localScale = initialscale / 2;
        mytext.text = "+" + scorenumber + "00";

        transform.DOScale(initialscale, 0.6f)                               // Text animation
            .OnComplete(()=>transform.DOScale(initialscale/2,0.3f))
            .OnComplete(()=> mytext.text="");
        
    }


}
