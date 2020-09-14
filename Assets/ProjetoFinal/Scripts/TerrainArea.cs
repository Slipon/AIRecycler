using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class TerrainArea : MonoBehaviour
{
    public AIAgent aiAgent;

    public GameObject TrashbinBlue;
    //public GameObject TrashbinRed;
    public GameObject TrashbinYellow;
    //public GameObject TrashbinGreen;
    
    public TextMeshPro cumulativeRewardText;

    public Trash plasticPrefab;
    public Trash batteriesPrefab;
    public Trash paperPrefab;
    public Trash glassPrefab;

    public List<GameObject> trashList;

    private void Start()
    {
        ResetArea();
        PlaceTrashBins();
    }

    private void Update()
    {
        cumulativeRewardText.text = aiAgent.GetCumulativeReward().ToString("0.00");
    }

    public void ResetArea() {
        RemoveAllTrash();
        PlaceAIAgent();
        SpawnTrash(0,0,4,4); //RED, GREEN, YELLOW, BLUE
    }

    public void RemoveSpecificTrash(GameObject trashObject) {
        trashList.Remove(trashObject);
        Destroy(trashObject);
    }

    public int TrashRemaining
    {
        get { return trashList.Count; }
    }

    public static Vector3 ChooseRandomPositionTrash() {
        Vector3 Min;
        Vector3 Max;
        float xCoord;
        float zCoord;
        float yCoord;

        //Set Range
        Min = new Vector3(8.375f, 0.016f, -5.663f);
        Max = new Vector3(16.619f, 0.018f, 7.596f);

        //Generate random value for each coordinate
        xCoord = Random.Range(Min.x, Max.x);
        zCoord = Random.Range(Min.z, Max.z);
        yCoord = Random.Range(Min.y, Max.y);

        return new Vector3(xCoord, yCoord, zCoord);
    }

    public static Vector3 ChooseRandomPositionTrashBin() {
        Vector3 Min;
        Vector3 Max;
        float xCoord;
        float zCoord;
        float yCoord;

        //Set Range
        Min = new Vector3(7.4f, 0.24f, -6.9f);
        Max = new Vector3(7.6f, 0.26f, 8.8f);

        //Generate random value for each coordinate
        xCoord = Random.Range(Min.x, Max.x);
        zCoord = Random.Range(Min.z, Max.z);
        yCoord = Random.Range(Min.y, Max.y);

        return new Vector3(xCoord, yCoord, zCoord);
    }

    private void RemoveAllTrash()
    {
        if (trashList != null)
        {
            for (int i = 0; i < trashList.Count; i++)
            {
                if (trashList[i] != null)
                {
                    Destroy(trashList[i]);
                }
            }
        }
        trashList = new List<GameObject>();
    }

    private void PlaceAIAgent()
    {
        Rigidbody rigidbody = aiAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        aiAgent.transform.position = new Vector3(13f, 0.5f, 8f);
        aiAgent.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void PlaceTrashBins()
    {
    
        //Plastic/Blue TrashBin
        GameObject blueTrashBinObject = Instantiate(TrashbinBlue.gameObject);
        blueTrashBinObject.transform.position = ChooseRandomPositionTrashBin();
        blueTrashBinObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        blueTrashBinObject.transform.SetParent(transform);
    /*
        //Glass/Green TrashBin
        GameObject greenTrashBinObject = Instantiate(TrashbinGreen.gameObject);
        greenTrashBinObject.transform.position = ChooseRandomPositionTrashBin();
        greenTrashBinObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        greenTrashBinObject.transform.SetParent(transform);

        //Batteries/Red TrashBin
        GameObject redTrashBinObject = Instantiate(TrashbinRed.gameObject);
        redTrashBinObject.transform.position = ChooseRandomPositionTrashBin();
        redTrashBinObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        redTrashBinObject.transform.SetParent(transform);

    */
        //Plastic/Yellow TrashBin
        GameObject yellowTrashBinObject = Instantiate(TrashbinYellow.gameObject);
        yellowTrashBinObject.transform.position = ChooseRandomPositionTrashBin();
        yellowTrashBinObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        yellowTrashBinObject.transform.SetParent(transform);
    }

    private void SpawnTrash(int redTrashAmount, int greenTrashAmount, int yellowTrashAmount, int blueTrashAmount)
    {
        // Spawn and place the yellow trash
        for (int i = 0; i < yellowTrashAmount; i++)
        {        
            GameObject trashObject = Instantiate(plasticPrefab.gameObject);
            trashObject.transform.position = ChooseRandomPositionTrash();
            trashObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            trashObject.transform.SetParent(transform);
            trashList.Add(trashObject);
        }

        // Spawn and place the green trash
        for (int i = 0; i < greenTrashAmount; i++)
        {
            GameObject trashObject = Instantiate(glassPrefab.gameObject);
            trashObject.transform.position = ChooseRandomPositionTrash();
            trashObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            trashObject.transform.SetParent(transform);
            trashList.Add(trashObject);
        }

        // Spawn and place the red trash
        for (int i = 0; i < redTrashAmount; i++)
        {
            GameObject trashObject = Instantiate(batteriesPrefab.gameObject);
            trashObject.transform.position = ChooseRandomPositionTrash();
            trashObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            trashObject.transform.SetParent(transform);
            trashList.Add(trashObject);
        }

        // Spawn and place the blue trash
        for (int i = 0; i < blueTrashAmount; i++)
        {
            GameObject trashObject = Instantiate(paperPrefab.gameObject);
            trashObject.transform.position = ChooseRandomPositionTrash();
            trashObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            trashObject.transform.SetParent(transform);
            trashList.Add(trashObject);
        }
    }

}
