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

    private Vector3 particlePosition;
    private Vector3 minedBlockPosition;
    private Quaternion particleRotation;
    private float particleIndent = 0.55f;
    private float blockMiningTime;
    private float startMinedBlock;
    private Action finishMineBlock;
    private MiningBlock miningBlock;

    private ParticlesFactory particlesFactory;

    [Inject]
    private void Construct(ParticlesFactory particlesFactory)
    {
        this.particlesFactory = particlesFactory;
    }

    public bool CanMine(BlockInfo blockInfo, Item item)
    {
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
        particlesFactory.SpawnParticles(particlePosition, particleRotation, miningBlock.blockInfo);

        if (Time.time - startMinedBlock >= blockMiningTime)
        {
            MineIsFinish();
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
        cracks.transform.position = minedBlockPosition + new Vector3(0.5f, 0.5f, 0.5f);
        cracks.gameObject.SetActive(true);
        cracks.ChangeTexture(Time.time - startMinedBlock, blockMiningTime);
    }
}

public class MiningBlock
{
    public BlockInfo blockInfo;
    public Vector3 blocPosition;
    public DirectionType directionType;
}