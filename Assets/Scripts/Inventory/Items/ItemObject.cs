using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

[RequireComponent(typeof(PoolObject))]
public class ItemObject : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float moveSpeed;

    public Item item;

    protected enum States
    {
        inHand,
        inFlight,
        inGround
    }

    private Tween tweenRotation;
    private Sequence moveSequece;
    private Vector3 rotation = new Vector3(0,180,0);
    private Vector3 moveTarget = new Vector3(0,0.5f,0);

    private void OnEnable()
    {
        Vector3 startPos = transform.position;
        tweenRotation = transform.DOLocalRotate(rotation, rotateSpeed, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        moveSequece = DOTween.Sequence();
        moveSequece.Append(transform.DOLocalMove(startPos + moveTarget, moveSpeed));
        moveSequece.Append(transform.DOLocalMove(startPos, moveSpeed));
        moveSequece.SetLoops(-1).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        tweenRotation?.Kill();
        moveSequece?.Kill();
    }
}
