using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using MyBox;

public class ButtonBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerMoveHandler
{

    [Separator("GRAPHIC")]
    [SerializeField] GameObject mouseHover;
    [SerializeField] GameObject mouseClick;
    [Separator("click")]
    [SerializeField] AudioClip clickClip;
    [SerializeField] AudioClip hoverClip;

    [Separator("TIMERES")]
    [ConditionalField(nameof(mouseClick))] public float clickTimerTotal;
    float clickTimerCurrent;

    private void OnDisable()
    {
        if(mouseHover != null) mouseHover.SetActive(false);
        if(mouseClick != null) mouseClick.SetActive(false);
        clickTimerCurrent = 0;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (clickClip != null && GameHandler.instance != null) GameHandler.instance.CreateSFX(clickClip);

        clickTimerCurrent = clickTimerTotal;
        mouseClick.SetActive(true);

    }

    private void Update()
    {
        if (clickTimerCurrent <= 0) return;
        UnityEngine.Debug.Log("this");

        clickTimerCurrent -= Time.deltaTime;

        if (clickTimerCurrent <= 0) mouseClick.SetActive(false);

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }   

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (mouseHover != null) mouseHover.SetActive(true);
        if (hoverClip != null && GameHandler.instance != null) GameHandler.instance.CreateSFX(hoverClip);
    }


    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (mouseHover != null) mouseHover.SetActive(false);
    }

    public virtual void OnPointerMove(PointerEventData eventData)
    {

    }
}
