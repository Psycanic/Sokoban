using UnityEngine;
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;
using System.Collections;

public class ObjectManager : MonoBehaviour
{   
    public static GridObject[] gridObjects;
    private GridMaker gridMaker;
    private int gridDimensionX;
    private int gridDimensionY;
    public static ObjectManager reference;



    void Awake()
    {
        /*for (int i = 0; i < gridObjects.Length; i++) { 
        gridObjects[i] = GetComponent<GridObject>();
            gridDictionary.Add(gridObjects[i].gridPosition, gridObjects[i]);
        }*/
        reference = this;
        gridMaker = GridMaker.reference;
        gridDimensionX = (int)gridMaker.dimensions.x;
        gridDimensionY = (int)gridMaker.dimensions.y;

        gridObjects = Object.FindObjectsByType<GridObject>(FindObjectsSortMode.None);
        Debug.Log(gridObjects);





    }


    public GridObject GetObjectAtGridPosition(Vector2Int pos)
    {
       
        foreach (GridObject obj in gridObjects)
        {
            if (obj.gridPosition == pos)
                return obj;
        }
        return null;
    }

    public bool IsWithinGrid(Vector2Int targetPos) {
        if (targetPos.x >= 1 && targetPos.x <= gridDimensionX && targetPos.y >= 1 && targetPos.y <= gridDimensionY)
        {
            return true;
        }
        else
        {
            return false;
        }

    }




}
