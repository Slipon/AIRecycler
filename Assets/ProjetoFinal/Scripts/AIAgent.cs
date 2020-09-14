using MLAgents;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : Agent
{
    private Animator animator;
    private GameObject agent;

    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    //public GameObject DroppedTrashPrefab;

    //private Dictionary<string, string> dict;
    //private string lastPickedTag;
    //private GameObject lastPicked;
    //private GameObject trashbinOfTarget;

    private TerrainArea terrainArea;
    new private Rigidbody rigidbody;

    private bool isDone;
    private float dropRadius = 1f;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        agent = GameObject.FindGameObjectWithTag("Agent");
        animator = GetComponent<Animator>();
        
        terrainArea = GetComponentInParent<TerrainArea>();
        rigidbody = GetComponent<Rigidbody>();

        //dict = new Dictionary<string, string>();
        //dict.Add("TrashYellow", "TrashBinYellow");
        //dict.Add("TrashRed", "TrashBinRed");
        //dict.Add("TrashGreen", "TrashBinGreen");
        //dict.Add("TrashBlue", "TrashBinBlue");
    }

    public override void AgentAction(float[] vectorAction)
    {
        float forwardAmount = vectorAction[0];
        float turnAmount = 0f;

        if (vectorAction[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            turnAmount = 1f;
        }

        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        if (maxStep > 0) AddReward(-1f / maxStep);
    }

    public override float[] Heuristic() // Used only for keyboard input and test
    {
        float forwardAction = 0f;
        float turnAction = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            animator.SetBool("running", true);
            forwardAction = 1f;

        }
        else
        {
            animator.SetBool("running", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            // turn left
            turnAction = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // turn right
            turnAction = 2f;
        }

        return new float[] { forwardAction, turnAction };
    }

    public override void AgentReset()
    {
        isDone = false;
        terrainArea.ResetArea();
        dropRadius = Academy.Instance.FloatProperties.GetPropertyWithDefault("drop_radius", 0f); //Curriculum learning
    }

    public override void CollectObservations()
    {
        //if(trashbinOfTarget != null)
        //{
        // Whether the agent has successfully drop trash in the correct trash bin (1 float = 1 value)
        AddVectorObs(isDone);

        // Distance to the trash bin (1 float = 1 value)
        AddVectorObs(Vector3.Distance(GameObject.FindGameObjectWithTag("TrashBinYellow").transform.position, transform.position));

        // Direction to trash bin (1 Vector3 = 3 values)
        AddVectorObs((GameObject.FindGameObjectWithTag("TrashBinYellow").transform.position - transform.position).normalized);

        // Direction agent is facing (1 Vector3 = 3 values)
        AddVectorObs(transform.forward);
        //}

    }

    private void FixedUpdate()
    {
        // Request a decision every 5 steps. RequestDecision() automatically calls RequestAction(),
        // but for the steps in between, we need to call it explicitly to take action using the results
        // of the previous decision
        if (GetStepCount() % 5 == 0)
        {
            RequestDecision();
        }
        else
        {
            RequestAction();
        }

        // Test if the agent is close enough to drop the trash in the trash bin
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("TrashBinYellow").transform.position) < dropRadius)
        {
            DropTrash();
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (dict.ContainsKey(collision.transform.tag))
        {
            lastPickedTag = dict[collision.transform.tag];
            lastPicked = collision.gameObject;
            CollectTrash(collision.gameObject);
            trashbinOfTarget = GameObject.FindGameObjectWithTag(lastPickedTag);

        }
        else if (lastPickedTag != null && lastPickedTag.Equals(collision.transform.tag))
        {
            DropTrash();
        }
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("TrashYellow"))
        {
            CollectTrash(collision.gameObject);
        }
        else if (collision.transform.CompareTag("TrashBinYellow"))
        {
            DropTrash();
        }
    }

    private void CollectTrash(GameObject trashObject)
    {
        if (isDone) return;
        isDone = true;

        terrainArea.RemoveSpecificTrash(trashObject);

        AddReward(1f);
    }

    private void DropTrash()
    {
        if (!isDone) return;
        isDone = false;

        /*
        GameObject droppedTrash = Instantiate(DroppedTrashPrefab);
        droppedTrash.transform.parent = transform.parent;
        droppedTrash.transform.position = agent.transform.position;
        Destroy(droppedTrash, 4f);

        GameObject recycle = Instantiate<GameObject>(recyclePrefab);
        recycle.transform.parent = transform.parent;
        recycle.transform.position = trashBin.transform.position + Vector3.up;
        Destroy(recycle, 4f);
        */

        AddReward(1f);

        if (terrainArea.TrashRemaining <= 0)
        {
            Done();
        }
    }


}
