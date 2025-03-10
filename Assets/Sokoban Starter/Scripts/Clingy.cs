using UnityEngine;

public class Clingy : MonoBehaviour, IMovable
{
    private GridObject gridObject;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
    }


    public bool Move(Vector2Int direction)
    {
        return false;
    }

    public bool Pull(Vector2Int direction)
    {
        Vector2Int targetPos = gridObject.gridPosition + direction;
        if (!ObjectManager.reference.IsWithinGrid(targetPos))
        {
            return false;
        }

        GridObject target = ObjectManager.reference.GetObjectAtGridPosition(targetPos);
        if (target == null)
        {
            gridObject.gridPosition = targetPos;
            return true;
        }
        else
        {
            IMovable movable = target.GetComponent<IMovable>();
            if (movable != null && movable.Move(direction))
            {
                gridObject.gridPosition = targetPos;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
