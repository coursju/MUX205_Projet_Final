using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Vector3 checkpointPosition;
    // Start is called before the first frame update
    void Start()
    {
        checkpointPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCheckpointPosition(Vector3 newPosition)
    {
        checkpointPosition = transform.position;
    }

    public Vector3 GetCheckpointPosition()
    {
        return checkpointPosition;
    }
}
