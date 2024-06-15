using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public GameObject[] seats;
    private List<Customer> seatedCustomers = new List<Customer>(); // 테이블에 앉은 손님들을 추적하는 리스트

    
    public float timeBeforeLeaving = 30f; // 테이블을 떠날 때까지의 대기 시간
    private bool isOccupied = false;

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void OccupyTable(Customer customer)
    {
        isOccupied = true;
        seatedCustomers.Add(customer);
    }

    public void VacateTable(Customer customer)
    {
        isOccupied = false;
        seatedCustomers.Remove(customer);
    }

    public Transform GetAvailableSeat()
    {
        foreach (GameObject seat in seats)
        {
            if (!seat.activeSelf)
            {
                seat.SetActive(true);
                return seat.transform;
            }
        }

        return null;
    }

    private IEnumerator TableTimer()
    {
        yield return new WaitForSeconds(timeBeforeLeaving);
        VacateTable(null); // 모든 손님이 동시에 퇴장하도록 수정해야 함
    }
}
