using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Camera cam;
    Vector3 orignalPos;
    public bool isPlayingEffect;
    private void Awake()
    {
        if(cam == null)
        {
            cam = Camera.main;
            orignalPos = cam.transform.position;
        }
        
    }

    //we do some cool littlee effects
    //i want to zoom in the playerwhatver th player might be

    public void ResetCamera()
    {
        if(cam == null)
        {
            cam = Camera.main;
            orignalPos = cam.transform.position;
        }
        cam.orthographicSize = 5;
        cam.transform.position = orignalPos;
    }

    public void CameraDeathEffect()
    {
        //we zoom in the player and then do the falling cool little effct.
        StopAllCoroutines();
        StartCoroutine(CameraDeathProcess());
    }

    IEnumerator CameraDeathProcess()
    {
        //zoom in the player
        isPlayingEffect = true;

        Vector3 pos = transform.position + new Vector3(0.5f, 0, -10);

        while (cam.transform.position != pos)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, pos, Time.deltaTime * 10);
            yield return new WaitForSeconds(0.01f);
        }


        while(cam.orthographicSize > 2.8f)
        {
            cam.orthographicSize -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        isPlayingEffect = false;


    }


    public void CameraHitEffect()
    {
        StopAllCoroutines();
        StartCoroutine(HitEffectProcess());
    }

    IEnumerator HitEffectProcess()
    {

        for (int i = 0; i < 15; i++)
        {
            float x = Random.Range(-0.5f, 0.5f);
            float y = Random.Range(-0.5f, 0.5f);

            cam.transform.position = orignalPos + new Vector3(x, y, 0);
            yield return new WaitForSeconds(0.01f);
        }
        cam.transform.position = orignalPos;
    }
}
