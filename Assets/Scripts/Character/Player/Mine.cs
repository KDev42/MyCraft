using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;
using FMODUnity;

public class Mine : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Cracks cracks;
    [SerializeField] PoolObject brokeBlockParticle;
    [SerializeField] PoolObject mineBlockParticle;

    private Vector3 particlePosition;
    private Vector3 minedBlockPosition;
    private Quaternion particleRotation;
    private float particleIndent = 0.55f;
    private float blockMiningTime;
    private float startMinedBlock;
    private Action finishMineBlock;
    private MiningBlock miningBlock;

    private ParticlesFactory particlesFactory;
    private GameWorld gameWorld;

    [Inject]
    private void Construct(ParticlesFactory particlesFactory, GameWorld gameWorld)
    {
        this.particlesFactory = particlesFactory;
        this.gameWorld = gameWorld;
    }

    public bool CanMine(BlockInfo blockInfo, Item item)
    {
        if (blockInfo.blockType == BlockType.air || blockInfo.blockType == BlockType.bedrock)
            return false;

        return true;
    }

    public void StartMine( Action callback, Item item, MiningBlock miningBlock)
    {
        this.miningBlock = miningBlock;
        minedBlockPosition = miningBlock.blocPosition;

        startMinedBlock = Time.time;
        blockMiningTime = miningBlock.blockInfo.miningTime;
        finishMineBlock = callback;

        if (item.itemForme == ItemForme.tool)
        {
            ItemTool itemTool = item as ItemTool;
            if (miningBlock.blockInfo.acceleratingTool ==  itemTool.toolType)
            {
                blockMiningTime /= itemTool.accelerationMining;
            }
        }

        if (anim != null)
        {
            anim.SetBool("Mine", true);
        }
        else
        {
            MineIsFinish();
        }
    }

    public void StopMine()
    {
        cracks.gameObject.SetActive(false);
        anim?.SetBool("Mine", false);
    }

    public void MineBlock()
    {
        if(!miningBlock.blockInfo.mineAudio.IsNull)
            RuntimeManager.PlayOneShot(miningBlock.blockInfo.mineAudio);
        CalculateRotation(miningBlock.directionType);
        ActivateCracks();

        if (Time.time - startMinedBlock >= blockMiningTime)
        {
            MineIsFinish();
            particlesFactory.SpawnParticles(brokeBlockParticle, minedBlockPosition + new Vector3(0.5f, 0.5f, 0.5f), particleRotation, miningBlock.blockInfo);
        }
        else
        {
            particlesFactory.SpawnParticles(mineBlockParticle, particlePosition, particleRotation, miningBlock.blockInfo);
        }
    }

    private void CalculateRotation( DirectionType directionType)
    {
        particlePosition = minedBlockPosition +  new Vector3(0.5f, 0.5f, 0.5f); 

        switch (directionType)
        {
            case DirectionType.up:
                particleRotation = Quaternion.Euler(90, 0, 0);

                particlePosition += new Vector3(0, particleIndent, 0);
                break;
            case DirectionType.down:
                particleRotation = Quaternion.Euler(90, 0, 0);
                particlePosition += new Vector3(0, -particleIndent, 0);
                break;

            case DirectionType.forward:
                particleRotation = Quaternion.Euler(0, 0, 0);
                particlePosition += new Vector3(0, 0, particleIndent);
                break;
            case DirectionType.back:
                particleRotation = Quaternion.Euler(0, 0, 0);
                particlePosition += new Vector3(0, 0, -particleIndent);
                break;

            case DirectionType.right:
                particleRotation = Quaternion.Euler(0, 90, 0);
                particlePosition += new Vector3(particleIndent, 0, 0);
                break;
            case DirectionType.left:
                particleRotation = Quaternion.Euler(0, 90, 0);
                particlePosition += new Vector3(-particleIndent, 0, 0);
                break;
        }
    }

    private void MineIsFinish()
    {
        StopMine();
        finishMineBlock();
    }

    private void ActivateCracks()
    {
        Cracks();
        //cracks.transform.position = minedBlockPosition + new Vector3(0.5f, 0.5f, 0.5f);
        //cracks.gameObject.SetActive(true);
        //cracks.ChangeTexture(Time.time - startMinedBlock, blockMiningTime);
    }

    private void Cracks()
    {
        Vector2Int chunkCoordinate = gameWorld.GetChunckCoordinate(minedBlockPosition);
        //Vector3 lockalBlockPosition = gameWorld.GetBlockLocalCoordinate(chunkCoordinate, minedBlockPosition);
        Vector4 coordinate = minedBlockPosition;
        coordinate.w = 0;

        Material chunkMatirial = gameWorld.activeChunkDatas[chunkCoordinate].chunkRenderer.GetComponent<MeshRenderer>().material;

        Debug.Log(coordinate);
        chunkMatirial.SetVector("_CracksPosition", coordinate);
    }
}

public class MiningBlock
{
    public BlockInfo blockInfo;
    public Vector3 blocPosition;
    public DirectionType directionType;
}