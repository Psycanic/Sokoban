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
            return false;
        }

        GridObject target = ObjectManager.reference.GetObjectAtGridPosition(targetPos);
        if (target == null)
        {
            gridObject.gridPosition = targetPos;
        }
        else
        {
           
            IMovable movable = target.GetComponent<IMovable>();
            if (movable != null && movable.Move(direction))
            {
                gridObject.gridPosition = targetPos;
            }
            else
            {
                return false;
            }
        }

        // check clingy
        Vector2Int behindPos = originalPos - direction;
        GridObject behindObj = ObjectManager.reference.GetObjectAtGridPosition(behindPos);
        if (behindObj != null)
        {
            Clingy clingy = behindObj.GetComponent<Clingy>();
            
        }

        return true;
    }
}
