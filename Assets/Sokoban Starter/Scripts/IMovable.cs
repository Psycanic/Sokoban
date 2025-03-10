using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IMovable
{
    bool Move(Vector2Int direction);

}
