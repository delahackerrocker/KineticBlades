using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Team : MonoBehaviour
{
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

        Invoke("Create_Team", 1f);
        Invoke("SendTargets", 2f);
        Invoke("AssignTargets", 3f);
    }

    void Update()
    {
        
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

    public void Create_AINPC(int index)
    {
        npcIndex = index;
        aiNPCs[npcIndex] = Instantiate(aiNPCTemplate, spawnPoints[index].transform.position, Quaternion.identity) as AI_NPC;
    }

    public Vector3 RandomPosition(Vector3 nearThisPosition)
    {
        Vector3 newPosition = new Vector3(nearThisPosition.x + npcIndex, nearThisPosition.y, nearThisPosition.z + npcIndex);
        return newPosition;
    }
}
