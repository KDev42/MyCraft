using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HitBox))]
public class Move : MonoBehaviour
{
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float jumpForce = 5f;
    //[SerializeField] float boundsTolerance = 0.1f;
    [SerializeField] float characterWidth = 0.25f;
    [SerializeField] float characterHeight = 2;
    [SerializeField] HitBox hitBox;

    private bool isGrounded;
    private bool isSprinting;
    private float verticalMomentum = 0;
    private Vector3 velocity;

    private GameWorld gameWorld;

    private float gravity => WorldConstants.gravity;
    private bool isFly;

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            isFly = !isFly;
        }
    }

    public Vector3 CalculateVelocity(Vector2 directionMove, Vector2 directionLook, GameWorld gameWorld)
    {
        //hitBox.BoxVertices();
        this.gameWorld = gameWorld;
        // Affect vertical momentum with gravity.
        if (verticalMomentum > gravity)
            verticalMomentum += Time.fixedDeltaTime * gravity;

        // if we're sprinting, use the sprint multiplier.
        if (isSprinting)
            velocity = ((transform.forward * directionMove.y) + (transform.right * directionMove.x)) * Time.fixedDeltaTime * sprintSpeed;
        else
            velocity = ((transform.forward * directionMove.y) + (transform.right * directionMove.x)) * Time.fixedDeltaTime * walkSpeed;

        // Apply vertical momentum (falling/jumping).

        if(!isFly)
        velocity += Vector3.up * verticalMomentum * Time.fixedDeltaTime;

        //if ((velocity.z > 0 && Front) 
        //    || (velocity.z < 0 && Back))
        //    velocity.z = 0;
        //if ((velocity.x > 0 && Right) 
        //    || (velocity.x < 0 && Left))
        //    velocity.x = 0;

        //if (velocity.y < 0)
        //    velocity.y = CheckDownSpeed(velocity.y);
        //else if (velocity.y > 0)
        //    velocity.y = CheckUpSpeed(velocity.y);

        //Vector3 vel = hitBox.MoveHitBox(new Vector3(directionMove.x, velocity.y, directionMove.y), ref isGrounded);
        Vector3 vel;

        //if (directionMove.x != 0 || directionMove.y != 0)
            vel = hitBox.GetHitSides(velocity, ref isGrounded);
        //else
        //    vel = new Vector3(0, 0, 0);

        return MultiplayVectors(velocity, vel);
    }

    private Vector3 MultiplayVectors(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            verticalMomentum = jumpForce;
            isGrounded = false;
            //jumpRequest = false;
        }
    }

    public void CheckPosition()
    {
        if (gameWorld.IsSolidBlock(transform.position))
        {
            PushUp();
        }
    }

    private Vector3Int CurrentCoordinate( Vector3 position)
    {
        return gameWorld.GetBlockCoordinate(position);
    }

    private void PushUp()
    {
        //Jump();
    }

    private float CheckDownSpeed(float downSpeed)
    {
        if ((gameWorld.IsSolidBlock(new Vector3(transform.position.x - characterWidth, transform.position.y + downSpeed, transform.position.z - characterWidth)) ||
            gameWorld.IsSolidBlock(new Vector3(transform.position.x + characterWidth, transform.position.y + downSpeed, transform.position.z - characterWidth)) ||
            gameWorld.IsSolidBlock(new Vector3(transform.position.x + characterWidth, transform.position.y + downSpeed, transform.position.z + characterWidth)) ||
            gameWorld.IsSolidBlock(new Vector3(transform.position.x - characterWidth, transform.position.y + downSpeed, transform.position.z + characterWidth))) &&
            gameWorld.HasObstacles(transform.position, DirectionType.down, 2, 1))
        {
            isGrounded = true;
            return 0;
        }
        else
        {
            isGrounded = false;
            return downSpeed;
        }
    }

    private float CheckUpSpeed(float upSpeed)
    {
        if ((gameWorld.IsSolidBlock(new Vector3(transform.position.x - characterWidth, transform.position.y + characterHeight + upSpeed, transform.position.z - characterWidth)) ||
            gameWorld.IsSolidBlock(new Vector3(transform.position.x + characterWidth, transform.position.y + characterHeight + upSpeed, transform.position.z - characterWidth)) ||
            gameWorld.IsSolidBlock(new Vector3(transform.position.x + characterWidth, transform.position.y + characterHeight + upSpeed, transform.position.z + characterWidth)) ||
            gameWorld.IsSolidBlock(new Vector3(transform.position.x - characterWidth, transform.position.y + characterHeight + upSpeed, transform.position.z + characterWidth))) ||
            gameWorld.HasObstacles(transform.position, DirectionType.up, 2, 1))
        {
            return 0;
        }
        else
        {
            return upSpeed;
        }
    }
}
