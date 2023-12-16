using UnityEngine;

public class CheckInPoint : MonoBehaviour
{
    private bool isOccupied = false;

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void OccupyPoint()
    {
        isOccupied = true;
    }

    public void VacatePoint()
    {
        isOccupied = false;
    }
}