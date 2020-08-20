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
    public int maxNPCs = 5;

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
            AssignTarget(index, Random.Range(0, aiOpposingTeam.maxNPCs));
        }
    }

    public void AssignTarget(int currentNPC, int currentTarget )
    {
        aiNPCs[currentNPC].lookTarget = aiEnemyNPCs[currentTarget].transform;
        aiNPCs[currentNPC].aiTargetingStack.target = aiEnemyNPCs[currentTarget].transform;

        if (isTeamOne) aiNPCs[currentNPC].NewTargetStackTarget();
    }

    public void MyTargetDied(AI_NPC needsNewTarget)
    {
        if (isTeamOne)
        {
            int randomTarget = Random.Range(0, aiOpposingTeam.maxNPCs - 1);
            if (aiEnemyNPCs[randomTarget] == null)
            {
                needsNewTarget.lookTarget = aiEnemyNPCs[randomTarget].transform;
                needsNewTarget.aiTargetingStack.target = aiEnemyNPCs[randomTarget].transform;
                needsNewTarget.NewTargetStackTarget();
            }
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
