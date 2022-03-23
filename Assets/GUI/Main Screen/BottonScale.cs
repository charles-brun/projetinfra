using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottonScale : MonoBehaviour
{
    public RectTransform BtnRectRaw;
    public Rect BtnRect;


    public Vector2 ResBase = new Vector2(1920,1080);
    Vector2 BtnPosBase;
    Vector2 BtnScaleBase;
    Vector2 ClientScreenRes;

    public float scalex;
    public float scaley;
    public float posx;
    public float posy;

    // Start is called before the first frame update
    void Start()
    {
        BtnRect = BtnRectRaw.rect;
        BtnScaleBase = BtnRect.size;
        BtnPosBase = BtnRectRaw.anchoredPosition;

        // Set differents rapport à utiliser
        scalex = ResBase.x / BtnScaleBase.x;
        scaley = ResBase.y / BtnScaleBase.y;
        posx = ResBase.x / BtnPosBase.x;
        posy = ResBase.y / BtnPosBase.y;


    }

    // Update is called once per frame
    void Update()
    {
        ClientScreenRes = new Vector2(Screen.width, Screen.height);
        BtnRectRaw.sizeDelta = new Vector2(ClientScreenRes.x / scalex, ClientScreenRes.y / scaley);
        BtnRectRaw.anchoredPosition = new Vector2(ClientScreenRes.x / posx, ClientScreenRes.y / posy);
    }
}
