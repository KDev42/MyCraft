using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Move))]
public class Player : MonoBehaviour
{
    [SerializeField] GameWorld world;
    [SerializeField] Transform mainCamera;
    [SerializeField] [Range(0, 90)] float yMaxAngle;
    [SerializeField] [Range(-90, 0)] float yMinAngle;
    [SerializeField] Move move;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] LayerMask itemLayer;
    [SerializeField] Transform body;
    [SerializeField] Hand hand;
    [SerializeField] Transform rightHand;

    public float checkIncrement = 0.02f;
    public float reach = 8f;
    public Transform highlightBlock;
    public Transform placeBlock;

    private bool canMine;
    private bool canContinueMine;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private Vector2 sens;
    private Vector2 direction;
    private Vector2 lookDirection;
    //private Vector3 velocity;
    private Vector3 impactPoint;
    private Vector3Int minedBlockCoordinate;
    private Coroutine delayMine;
    private MiningBlock miningBlock = new MiningBlock();

    //private  BlockType selectedBlockType = BlockType.grass; //test
    private DirectionType directionType;

    private Database database;
    private GameData gameData;

    [Inject]
    private void Construct(Database database, GameData gameData)
    {
        this.database = database;
        this.gameData = gameData;
    }

    private void Awake()
    {
        EventsHolder.moveInput += GetMotionInput;
        EventsHolder.attackDirectionInput += GetAttackDirectionInput;
        EventsHolder.jump += Jump;
        EventsHolder.attack += Attack;
        EventsHolder.startMine += StartMine;
        EventsHolder.stopMine += StopMine;
    }

    private void Start()
    {
        mainCamera.parent = transform;
        mainCamera.localPosition = cameraOffset;
        mainCamera.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void Update()
    {
        sens = new Vector2(gameData.gameSettings.sens, gameData.gameSettings.sens);
        // Destroy block.
        //if (Input.GetMouseButtonDown(1))
        //    world.DestroyBlock(highlightBlock.position);

        //move.CalculateVelocity(direction, lookDirection, world);

        //transform.Translate(velocity, Space.World);

        //move.CheckPosition();
        PlaceCursorBlocks();
        CheckMinigBlock();

        CameraRotation();

        yRotation = lookDirection.x * sens.x ;
        body.Rotate(Vector3.up * yRotation);
        //yRotation += lookDirection.x * sens.x * Time.deltaTime;
        //transform.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    private void FixedUpdate()
    {
        move.CalculateVelocity(direction, lookDirection, world);
    }

    private void GetMotionInput(Vector2 direction)
    {
        this.direction = new Vector3(direction.x,  direction.y,0).normalized;
    }

    private void GetAttackDirectionInput(Vector2 direction)
    {
        lookDirection = new Vector3(direction.x, direction.y) /100;
    }

    private void Attack()
    {
        if (highlightBlock.gameObject.activeSelf && world.CanBuild(placeBlock.position) )
        {
            // Place block.
            //if (Input.GetMouseButtonDown(1))
            SpawnBlock();
        }
    }

    private void SpawnBlock()
    {
        Item item = database.itemDatabase.GetItem(hand.itemType);
        if (item !=null && item.itemForme == ItemForme.block)
        {
            ItemBlock itemBlock = item as ItemBlock;
            world.AddBlock(placeBlock.position, itemBlock.blockType);

            //PlayerData.RemoveItem(hand.itemType, 1);
        }
    }

    private void CheckMinigBlock()
    {
        directionType = world.GetPlaneOrientation(impactPoint);
        miningBlock.directionType = directionType;
        if (minedBlockCoordinate != world.GetBlockCoordinate(impactPoint) && canContinueMine)
        {
            StopMine();
            StartMine();
        }
    }

    private void StartMine()
    {
        if (canMine)
        {
            canContinueMine = true;
            minedBlockCoordinate = world.GetBlockCoordinate(highlightBlock.position);
            BlockInfo blockInfo = world.GetBlockInfo(highlightBlock.position);

            miningBlock.blockInfo = blockInfo;
            miningBlock.blocPosition = highlightBlock.position;
            miningBlock.directionType = directionType;

            hand.StartMine(BlockBroken, miningBlock);
        }
    }

    private void StopMine()
    {
        canContinueMine = false;
        hand.StopMine();
        if (delayMine != null)
            StopCoroutine(delayMine);
    }

    private void BlockBroken()
    {
        world.DestroyBlock(highlightBlock.position);
            delayMine =  StartCoroutine(DelayMine());
    }

    IEnumerator DelayMine()
    {
        yield return new WaitForSeconds(0.2f);
        if (canContinueMine)
            StartMine();
    }

    private void Jump()
    {
        move.Jump();
    }

    private void CameraRotation()
    {
        xRotation -= lookDirection.y *sens.y;
        xRotation = Mathf.Clamp(xRotation, yMinAngle, yMaxAngle);

        mainCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        rightHand.localRotation = Quaternion.Euler(xRotation, 0, -90);

        //mainCamera.Rotate(Vector3.up * yRotation);
        //rightHand.Rotate(Vector3.up * yRotation);
        //yRotation = lookDirection.x * sens.x * Time.deltaTime;
        //mainCamera.Rotate(Vector3.up * yRotation);
        //rightHand.Rotate(Vector3.up * yRotation);
    }

    private void PlaceCursorBlocks()
    {
        float step = checkIncrement;
        Vector3 lastPos = new Vector3();

        while (step < reach)
        {
            impactPoint = mainCamera.position + (mainCamera.forward * step);

            if (world.HasBlock(impactPoint))
            {
                highlightBlock.position = new Vector3(Mathf.FloorToInt(impactPoint.x), Mathf.FloorToInt(impactPoint.y), Mathf.FloorToInt(impactPoint.z));
                placeBlock.position = lastPos;

                canMine = true;
                highlightBlock.gameObject.SetActive(true);
                //placeBlock.gameObject.SetActive(true);

                return;
            }

            lastPos = new Vector3(Mathf.FloorToInt(impactPoint.x), Mathf.FloorToInt(impactPoint.y), Mathf.FloorToInt(impactPoint.z));

            step += checkIncrement;
        }

        canMine = false;
        highlightBlock.gameObject.SetActive(false);
        //placeBlock.gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & itemLayer) != 0)
        {
            ItemObject itemObject = other.GetComponent<ItemObject>();
            PlayerData.AddToHandInventory( itemObject.item, out bool canAdd);
            if (canAdd)
            {
                other.GetComponent<PoolObject>().ReturnToPool();
            }
        }
    }
    //private void GetPlayerInputs()
    //{
    //    horizontal = Input.GetAxis("Horizontal");
    //    vertical = Input.GetAxis("Vertical");
    //    mouseHorizontal = Input.GetAxis("Mouse X");
    //    mouseVertical = Input.GetAxis("Mouse Y");

    //    if (Input.GetButtonDown("Sprint"))
    //        isSprinting = true;
    //    if (Input.GetButtonUp("Sprint"))
    //        isSprinting = false;

    //    if (isGrounded && Input.GetButtonDown("Jump"))
    //        jumpRequest = true;
    //}
}
