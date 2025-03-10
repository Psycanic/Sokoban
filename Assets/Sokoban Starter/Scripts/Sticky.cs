using UnityEngine;

public class Sticky : MonoBehaviour, IMovable
{
    private GridObject gridObject;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
    }

    public bool Move(Vector2Int direction)
    {
        return FollowMove(direction);
    }

    // Follow
    public bool FollowMove(Vector2Int direction)
    {
        Vector2Int originalPos = gridObject.gridPosition;
        Vector2Int targetPos = originalPos + direction;

        if (!ObjectManager.reference.IsWithinGrid(targetPos))
        {
            Debug.Log("Sticky: target " + targetPos + " is out of grid");
            return false;
        }

        GridObject target = ObjectManager.reference.GetObjectAtGridPosition(targetPos);
        if (target == null)
        {
            gridObject.gridPosition = targetPos;
            Debug.Log("Sticky: followed to free " + targetPos);
        }
        else
        {
           
            IMovable movable = target.GetComponent<IMovable>();
            if (movable != null && movable.Move(direction))
            {
                gridObject.gridPosition = targetPos;
                Debug.Log("Sticky: chain followed to " + targetPos);
            }
            else
            {
                Debug.Log("Sticky: blocked at " + targetPos);
                return false;
            }
        }

        // check clingy
        Vector2Int behindPos = originalPos - direction;
        GridObject behindObj = ObjectManager.reference.GetObjectAtGridPosition(behindPos);
        if (behindObj != null)
        {
            Clingy clingy = behindObj.GetComponent<Clingy>();
            if (clingy != null)
            {
                if (!clingy.Pull(direction))
                    Debug.Log("Sticky: failed to pull Clingy at " + behindPos);
            }
        }

        return true;
    }
}
