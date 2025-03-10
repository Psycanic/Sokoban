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
            Debug.Log("Clingy: target " + targetPos + " is out of grid");
            return false;
        }

        GridObject target = ObjectManager.reference.GetObjectAtGridPosition(targetPos);
        if (target == null)
        {
            gridObject.gridPosition = targetPos;
            Debug.Log("Clingy: pulled to free " + targetPos);
            return true;
        }
        else
        {
            IMovable movable = target.GetComponent<IMovable>();
            if (movable != null && movable.Move(direction))
            {
                gridObject.gridPosition = targetPos;
                Debug.Log("Clingy: chain pulled to " + targetPos);
                return true;
            }
            else
            {
                Debug.Log("Clingy: blocked at " + targetPos);
                return false;
            }
        }
    }
}
