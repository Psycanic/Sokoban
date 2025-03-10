using UnityEngine;

public class Smooth : MonoBehaviour, IMovable
{
    private GridObject gridObject;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
    }

    public bool Move(Vector2Int direction)
    {
        Vector2Int originalPos = gridObject.gridPosition;
        Vector2Int targetPos = originalPos + direction;

        // check bounds
        if (!ObjectManager.reference.IsWithinGrid(targetPos))
        {
            Debug.Log("Smooth: target position " + targetPos + " is out of grid");
            return false;
        }

        // check if occupied
        GridObject targetObj = ObjectManager.reference.GetObjectAtGridPosition(targetPos);
        if (targetObj != null)
        {
            // cannot push if clingy
            if (targetObj.GetComponent<Clingy>() != null)
            {
                Debug.Log("Smooth: target " + targetPos + " is occupied by Clingy, cannot push directly");
                return false;
            }

          
            IMovable movable = targetObj.GetComponent<IMovable>();
            if (movable != null && movable.Move(direction))
            {
                gridObject.gridPosition = targetPos;
            }
            else
            {
                Debug.Log("Smooth: failed to push object at " + targetPos);
                return false;
            }
        }
        else
        {
            gridObject.gridPosition = targetPos;
        }

        //check if clingy is behind
        Vector2Int behindPos = originalPos - direction;
        GridObject behindObj = ObjectManager.reference.GetObjectAtGridPosition(behindPos);
        if (behindObj != null)
        {
            Clingy clingy = behindObj.GetComponent<Clingy>();
            if (clingy != null)
            {
                if (!clingy.Pull(direction))
                    Debug.Log("Smooth: failed to pull Clingy at " + behindPos);
            }
        }

        return true;
    }
}
