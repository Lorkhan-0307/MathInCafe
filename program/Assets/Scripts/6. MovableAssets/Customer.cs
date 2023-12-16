<<<<<<< HEAD
using System;
=======
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
<<<<<<< HEAD
    public Transform checkInPosition; // 계산대 위치
    public Transform[] tablePositions; // 테이블 위치들
    public float moveSpeed = 2f;
    public float waitTimeAtCheckIn = 5f;
    public float timeAtTable = 30f;
    
    private CheckInPoint currentCheckInPoint;

    private Animator animator;
    private bool isMoving = true;
    private bool isWaiting = false;
    private bool isAtTable = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(MoveToCheckInPoint("CheckInPoint1"));
    }
    
    private IEnumerator MoveToCheckInPoint(string checkInPointName)
    {
        isMoving = true;
        isWaiting = false;

        
        CheckInPoint targetCheckInPoint = GameObject.Find(checkInPointName).GetComponent<CheckInPoint>();

        if (int.TryParse(checkInPointName.Substring(checkInPointName.Length - 1), out int currentIdx))
        {
            // 2~5CheckInPoint인 경우
            if (currentIdx > 1)
            {
                string previousCheckInPointName = "CheckInPoint" + (currentIdx - 1);
                CheckInPoint previousCheckInPoint = GameObject.Find(previousCheckInPointName).GetComponent<CheckInPoint>();

                // 앞 체크 인 포인트가 점유되지 않으면 해당 칸으로 이동
                if (!previousCheckInPoint.IsOccupied())
                {
                    targetCheckInPoint = previousCheckInPoint;
                }
            }
        }

        while (targetCheckInPoint.IsOccupied())
        {
            // 다음 CheckInPoint로 이동하도록 반복 호출
            int nextIndex = currentIdx + 1;
            string nextCheckInPointName = "CheckInPoint" + nextIndex;
            targetCheckInPoint = GameObject.Find(nextCheckInPointName).GetComponent<CheckInPoint>();
        }

        currentCheckInPoint = targetCheckInPoint;
        currentCheckInPoint.OccupyPoint();
        isWaiting = false;

        // 이동 로직 변경
        yield return MoveToPosition(currentCheckInPoint.transform);

        // 이동이 끝난 후
        if (currentCheckInPoint == targetCheckInPoint)
        {
            // 1CheckInPoint라면 대기 후 Table로 이동
            if (checkInPointName == "CheckInPoint1")
            {
                yield return new WaitForSeconds(5f);
                StartCoroutine(MoveToTable());
            }
            else
            {
                // 2~5CheckInPoint라면 앞 CheckInPoint의 점유 해제를 기다린 후 해당 CheckInPoint로 이동
                int currentIndex = int.Parse(checkInPointName.Substring(checkInPointName.Length - 1));
                string previousCheckInPointName = "CheckInPoint" + (currentIndex - 1);
                CheckInPoint previousCheckInPoint = GameObject.Find(previousCheckInPointName).GetComponent<CheckInPoint>();

                yield return new WaitUntil(() => !previousCheckInPoint.IsOccupied());
                currentCheckInPoint.VacatePoint();
                StartCoroutine(MoveToCheckInPoint(checkInPointName));
            }
        }
    }
    

    private IEnumerator CustomerBehaviorRoutine()
    {
        // 입구에서 출발하여 계산대로 이동
        yield return MoveToPosition(checkInPosition);

        // 줄 서기
        while (!IsAtFrontOfQueue())
        {
            yield return null;
        }

        // 계산대에서 대기
        isWaiting = true;
        IdleAnimation();
        yield return new WaitForSeconds(waitTimeAtCheckIn);
        isWaiting = false;

        
    }

    private IEnumerator MoveToTable()
    {
        // 테이블로 이동
        Transform selectedTable = GetAvailableTable();
        if (selectedTable != null)
        {
            MovingAnimation();
            isMoving = true;
            yield return MoveToPosition(selectedTable);
            IdleAnimation();
            isMoving = false;
            isAtTable = true;
            StartCoroutine(TableTimer());
        }
    }
    private bool IsAtFrontOfQueue()
    {
        // 다른 손님들이 줄을 섰을 때 내가 그 줄의 가장 앞에 있는지 검사
        Customer[] otherCustomers = FindObjectsOfType<Customer>();
        float myYPosition = transform.position.y;

        foreach (Customer customer in otherCustomers)
        {
            if (customer != this)
            {
                float otherYPosition = customer.transform.position.y;
                if (otherYPosition < myYPosition)
                {
                    return false; // 다른 손님이 나보다 앞에 있음
                }
            }
        }

        return true; // 가장 앞에 서 있음
    }

    private Transform GetAvailableTable()
    {
        Transform closestTable = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform tablePosition in tablePositions)
        {
            // 테이블 위치를 기준으로 아직 점유되지 않은 테이블 중 가장 가까운 테이블을 찾음
            if (!tablePosition.GetComponent<Table>().IsOccupied())
            {
                float distanceToTable = Vector3.Distance(transform.position, tablePosition.position);
                if (distanceToTable < closestDistance)
                {
                    closestDistance = distanceToTable;
                    closestTable = tablePosition;
                }
            }
        }

        return closestTable;
    }
    
    private Table GetCurrentTable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Table")))
        {
            return hit.collider.GetComponent<Table>();
        }

        return null;
    }

    private IEnumerator TableTimer()
    {
        yield return new WaitForSeconds(timeAtTable);

        // 테이블 점유권 해제 및 이동 로직 추가
        Table table = GetCurrentTable();
        if (table != null)
        {
            table.VacateTable(this); // 해당 손님에 대한 테이블 점유 해제
        }

        isAtTable = false;
        // 다시 계산대로 이동
        MovingAnimation();
        yield return MoveToPosition(checkInPosition);

        // 계산대 대기 및 순찰 로직
        // ...
    }

    private IEnumerator MoveToPosition(Transform targetPosition)
    {
        while (transform.position != targetPosition.position  && isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void MovingAnimation()
    {
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsWalking", true);
    }

    private void IdleAnimation()
    {
        animator.SetBool("IsIdle",true);
        animator.SetBool("IsWalking", false);
    }
=======
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform[] tableParent;
    [SerializeField] private Transform checkInPoint;
    public float moveSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PatrolRoutine()
    {
        while(true)
        {
        
        }
    }
>>>>>>> ba33f47ac510858c7f05097a7cb78186c155afac
}
