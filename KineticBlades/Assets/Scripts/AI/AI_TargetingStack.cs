using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TargetingStack : MonoBehaviour
{
    public AI_NPC aiNPC;

    public Transform target;

    public AI_TargetCluster inner;
    public AI_TargetCluster middle;
    public AI_TargetCluster outer;

    protected AI_TargetCluster[] defensiveTargetClusters;

    void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        defensiveTargetClusters = new AI_TargetCluster[5];
        defensiveTargetClusters[0] = outer;
        defensiveTargetClusters[1] = middle;
        defensiveTargetClusters[2] = outer;
        defensiveTargetClusters[3] = inner;
        defensiveTargetClusters[4] = outer;
    }


    void Update()
    {
        this.transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        this.transform.rotation = target.transform.rotation;
    }

    public AI_Target GetRandomTarget()
    {
        int randomClusterID = Random.Range(0, defensiveTargetClusters.Length-1);
        AI_Target randomTarget = defensiveTargetClusters[randomClusterID].GetRandomTarget();

        return randomTarget;
    }
}