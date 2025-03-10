using UnityEngine;

public class Wall : MonoBehaviour, IMovable
{
    private GridObject gridObject;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
    }

    // Wall ”¿‘∂≤ªƒ‹“∆∂Ø
    public bool Move(Vector2Int direction)
    {
        return false;
    }
}
