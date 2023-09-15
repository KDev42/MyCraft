using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

[RequireComponent(typeof(PoolObject))]
[RequireComponent(typeof(HitBox))]
public class ItemObject : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float dropSpeed;

    public Item item;

    public ItemStates State { get; set; }

    private bool isGrounded;
    private Tween tweenRotation;
    private Sequence moveSequece;
    private Vector3 rotation = new Vector3(0,360,0);
    private Vector3 moveTarget = new Vector3(0,0.5f,0);
    private HitBox hitBox => GetComponent<HitBox>();

    private void Update()
    {
        if (State == ItemStates.inGround)
        {
            float fallingSpeed = dropSpeed * Time.deltaTime;
            Vector3 fallingVector = new Vector3(0, -fallingSpeed, 0);
            hitBox.GetHitSides(fallingVector, ref isGrounded);

            if (!isGrounded)
                transform.position += fallingVector;
        }
    }

    private void OnEnable()
    {
        if(State == ItemStates.inGround)
        {
            tweenRotation = transform.DOLocalRotate(rotation, rotateSpeed, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
            moveSequece = DOTween.Sequence();
        }
    }

    private void OnDisable()
    {
        if (State == ItemStates.inGround)
        {
            tweenRotation?.Kill();
            moveSequece?.Kill();
        }
    }
}

public enum ItemStates
{
    inHand,
    inFlight,
    inGround
}

