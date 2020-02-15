using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private SpriteRenderer overlaySprite;
    // Start is called before the first frame update
    void Start()
    {
        overlaySprite = transform.Find("Overlay").GetComponent<SpriteRenderer>();
        if (overlaySprite)
        {
            HideOverlay();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        ShowOverlay();
    }

    private void ShowOverlay()
    {
        
        Color overlaySpriteColor = overlaySprite.color;
        overlaySpriteColor.a = 0.35f;
        overlaySprite.color = overlaySpriteColor;
    }

    private void OnMouseExit()
    {
        HideOverlay();
    }

    private void HideOverlay()
    {
        Color overlaySpriteColor = overlaySprite.color;
        overlaySpriteColor.a = 0.0f;
        overlaySprite.color = overlaySpriteColor;
    }
}
