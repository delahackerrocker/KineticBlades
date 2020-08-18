using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Team : MonoBehaviour
{
    public bool isTeamOne = false;
    // Teams don't have to have human players right now
    public bool hasHumanPlayer = false;
    public GameObject playerGO;

    public AI_Team aiOpposingTeam;

    public AI_NPC aiNPCTemplate;

    public GameObject[] spawnPoints;

    protected AI_NPC[] aiNPCs;
    protected int npcIndex = 0;
    protected int maxNPCs = 5;

    protected AI_NPC[] aiEnemyNPCs;

    void Start()
    {
        aiNPCs = new AI_NPC[maxNPCs];
        aiEnemyNPCs = new AI_NPC[maxNPCs];

        if (playerGO != null) hasHumanPlayer = true;

        Invoke("Create_Team", 1f);
        Invoke("SendTargets", 2f);
        Invoke("AssignTargets", 3f);
    }

    public void Create_Team()
    {
        for (var index=0; index < maxNPCs; index++)
        {
            Create_AINPC(index);
        }
    }

    public void SendTargets()
    {
        aiOpposingTeam.aiEnemyNPCs = aiNPCs;
    }

    public void AssignTargets()
    {
        for (var index = 0; index < maxNPCs; index++)
        {
            AssignTarget(index, index);
        }
    }

    public void AssignTarget(int currentNPC, int currentTarget )
    {
        aiNPCs[currentNPC].lookTarget = aiEnemyNPCs[currentTarget].transform;
        aiNPCs[currentNPC].aiTargetingStack.target = aiEnemyNPCs[currentTarget].transform;
        aiNPCs[currentNPC].NewTargetStackTarget();
    }

    public void MyTargetDied(AI_NPC needsNewTarget)
    {
        int shouldITargetPlayer = Random.Range(0, 1);
        if (shouldITargetPlayer == 0)
        {

            int newTarget = -1;

            for (int index = 0; index < maxNPCs; index++)
            {
                if (aiEnemyNPCs[index] == null)
                {
                    // this guy is dead
                }
                else
                {
                    newTarget = index;
                }
            }

            if (newTarget == -1)
            {
                if (isTeamOne)
                {
                    // the players team has won
                }
                else
                {
                    // target the player now because no AI targets left
                    GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");

                    needsNewTarget.lookTarget = thePlayer.transform;
                    needsNewTarget.aiTargetingStack.target = thePlayer.transform;
                    needsNewTarget.NewTargetStackTarget();
                }
            }
            else
            {
                needsNewTarget.lookTarget = aiEnemyNPCs[newTarget].transform;
                needsNewTarget.aiTargetingStack.target = aiEnemyNPCs[newTarget].transform;
                needsNewTarget.NewTargetStackTarget();
            }
        } else
        {
            if (isTeamOne)
            {
                // this character is team one
            }
            else
            {
                // we're going to taret the player
                GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");

                needsNewTarget.lookTarget = thePlayer.transform;
                needsNewTarget.aiTargetingStack.target = thePlayer.transform;
                needsNewTarget.NewTargetStackTarget();
            }
        }
    }

    public void Create_AINPC(int index)
    {
        npcIndex = index;
        aiNPCs[npcIndex] = Instantiate(aiNPCTemplate, spawnPoints[index].transform.position, Quaternion.identity) as AI_NPC;
        aiNPCs[npcIndex].myTeam = this;
        aiNPCs[npcIndex].iAmTeamOne = isTeamOne;
    }

    public Vector3 RandomPosition(Vector3 nearThisPosition)
    {
        Vector3 newPosition = new Vector3(nearThisPosition.x + npcIndex, nearThisPosition.y, nearThisPosition.z + npcIndex);
        return newPosition;
    }
}
