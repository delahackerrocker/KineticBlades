using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TargetCluster : MonoBehaviour
{
    public AI_Target front;
    public AI_Target right;
    public AI_Target left;
    public AI_Target rear;

    protected AI_Target[] allTargets;

    protected AI_Target[] primaryTargets;

    // Start is called before the first frame update
    void Start()
    {
        allTargets = new AI_Target[4];

        allTargets[0] = front;
        allTargets[1] = right;
        allTargets[2] = left;
        allTargets[3] = rear;

        primaryTargets = new AI_Target[4];

        primaryTargets[0] = front;
        primaryTargets[1] = right;
        primaryTargets[2] = left;
        primaryTargets[3] = front;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AI_Target GetRandomTarget()
    {
        int randomTargetID = Random.Range(0, 3);
        AI_Target randomTarget = primaryTargets[randomTargetID];

        return randomTarget;
    }
}
