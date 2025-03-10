using UnityEngine;

public class Player : MonoBehaviour
{
    private GridObject gridObject;

    void Start()
    {
        gridObject = GetComponent<GridObject>();
    }

    void Update()
    {
        Vector2Int direction = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W))
            direction = new Vector2Int(0, -1);
        else if (Input.GetKeyDown(KeyCode.S))
            direction = new Vector2Int(0, 1);
        else if (Input.GetKeyDown(KeyCode.A))
            direction = new Vector2Int(-1, 0);
        else if (Input.GetKeyDown(KeyCode.D))
            direction = new Vector2Int(1, 0);

        if (direction != new Vector2Int(0,0))
        {
            TryMove(direction);
        }
    }

    private void TryMove(Vector2Int direction)
    {
        Debug.Log("[Player] TryMove start: direction = " + direction);

        Vector2Int currentPos = gridObject.gridPosition;
        Vector2Int targetPos = currentPos + direction;

        if (!ObjectManager.reference.IsWithinGrid(targetPos))
        {
            Debug.Log("Player: target position " + targetPos + " is out of grid");
            return;
        }

        GridObject targetObj = ObjectManager.reference.GetObjectAtGridPosition(targetPos);
        bool moveSuccess = false;
        GameObject pushedObject = null;
        if (targetObj != null)
        {
            IMovable movable = targetObj.GetComponent<IMovable>();
            if (movable != null && movable.Move(direction))
            {
                moveSuccess = true;
                pushedObject = targetObj.gameObject;
            }
            else
            {
                Debug.Log("Player: chain move failed at " + targetPos);
                return;
            }
        }
        else
        {
            moveSuccess = true;
        }

        if (moveSuccess)
        {
            gridObject.gridPosition = targetPos;
            Debug.Log("Player moved to " + targetPos);

            Vector2Int[] neighbors = { new Vector2Int(0, -1), new Vector2Int(0, 1),
                                   new Vector2Int(-1, 0), new Vector2Int(1, 0) };
            foreach (Vector2Int offset in neighbors)
            {
                Vector2Int neighborPos = currentPos + offset;
                GridObject neighborObj = ObjectManager.reference.GetObjectAtGridPosition(neighborPos);
                if (neighborObj != null && neighborObj.gameObject != pushedObject)
                {
                    Sticky sticky = neighborObj.GetComponent<Sticky>();
                    if (sticky != null)
                    {
                        sticky.FollowMove(direction);
                    }
                }
            }

            
            //check for surrounding clingy
            Vector2Int[] adjacentOffsets = new Vector2Int[]
            { new Vector2Int(0, -1),new Vector2Int(0, 1),new Vector2Int(-1, 0),new Vector2Int(1, 0)};

            foreach (Vector2Int offset in adjacentOffsets)
            {
                // check if its buhind
                if (offset == -direction)
                {
                    Vector2Int neighborPos = currentPos + offset;
                    GridObject neighborObj = ObjectManager.reference.GetObjectAtGridPosition(neighborPos);
                    if (neighborObj != null)
                    {
                        Clingy clingy = neighborObj.GetComponent<Clingy>();
                        if (clingy != null)
                        {
                            if (!clingy.Pull(direction))
                                Debug.Log("Player: Clingy at " + neighborPos + " failed to be pulled");
                        }
                    }
                }
            }

        }
        Debug.Log("[Player] TryMove end");

    }


    /* move player
    public bool Move(Vector2Int direction)
    {
        Vector2Int targetPos = gridObject.gridPosition + direction;
        if (!ObjectManager.reference.IsWithinGrid(targetPos)) return false;
        if (ObjectManager.reference.GetObjectAtGridPosition(targetPos) != null) return false;
        gridObject.gridPosition = targetPos;
        Debug.Log("Player (pushed) moved to " + targetPos);
        return true;
    }*/
}
