using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxBody))]
[RequireComponent(typeof(CollisionWithChunk))]
public class PhysicalBody : MonoBehaviour
{
    [SerializeField] BoxBody boxBody;
    [SerializeField] CollisionWithChunk collisionWithChunk;

    //private void OnValidate()
    //{
    //    if (boxBody == null)
    //        boxBody = GetComponent<BoxBody>();
    //    if (collisionWithChunk == null)
    //        collisionWithChunk = GetComponent<CollisionWithChunk>();
    //}

    public void Move(Vector3 direction, GameWorld gameWorld, ref bool isGrounded)
    {
        Vector3 moveDirection = collisionWithChunk.PossibleMovement(gameWorld, transform.position, direction, transform.rotation, boxBody.size, ref isGrounded);
        transform.position += moveDirection;

        //isGrounded = moveDirection.y != 0;
    }
}
