using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] float delay = 1f;
    [SerializeField] float targetField = 60;
    [SerializeField] float zoomDuration = 2;
    [SerializeField] Vector2 targetPos = Vector2.zero;
    [SerializeField] float moveDuration = 2;
    Camera cam = null;


    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Start()
    {
        StartCoroutine(TransitionCoroutine());
    }

    IEnumerator TransitionCoroutine()
    {
        yield return new WaitForSeconds(delay);
        cam.DOFieldOfView(targetField, zoomDuration);
        cam.transform.DOMove(targetPos, moveDuration);
    }
}
