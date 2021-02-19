using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointToPoint : MonoBehaviour
{
    public GameObject objectToMove;
    public Animator animator;
    public ParticleSystem angry;

    public Transform[] motionPoints;
    public float speedMotion;
    public bool forward;
    private int pointIndex;
    private Vector3 pointAim;

    public float rotationSpeed;
    private bool continueToMove = true;

    private bool watchOver = true;
    private void Start()
    {
        pointAim = motionPoints[pointIndex].position;
    }

    private void Update()
    {
        if (watchOver)
            LineOfSight();

        if(continueToMove)
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, pointAim, speedMotion);

        if (objectToMove.transform.position == pointAim)
        {
            if (forward)
            {
                if (pointIndex < motionPoints.Length)
                    pointIndex++;
                if (pointIndex == motionPoints.Length)
                    pointIndex = 0;
            }
            else
            {
                if (pointIndex > 0)
                    pointIndex--;
                else
                {
                    if (pointIndex == 0)
                        pointIndex = motionPoints.Length - 1;
                }
            }
            pointAim = motionPoints[pointIndex].position;

            if(continueToMove)
                objectToMove.transform.DOLookAt(pointAim, rotationSpeed).OnComplete(MoveNext);

            continueToMove = false;
        }
    }
    private void MoveNext()
    {
        continueToMove = true;
    }

    private void LineOfSight()
    {
        RaycastHit[] raycastHits = new RaycastHit[10];
        Ray[] rays = new Ray[10];
        float rayDirection = -0.5f;

        for(int i = 0; i < rays.Length; i++)
        {
            rays[i] = new Ray(objectToMove.transform.position, objectToMove.transform.forward + (objectToMove.transform.right * rayDirection));
            rayDirection += 0.1f;

            if (Physics.Raycast(rays[i], out raycastHits[i], 5f))
            {
                if(raycastHits[i].transform.CompareTag("Player"))
                {
                    StopGame();
                    MoveToPlayer(objectToMove.transform.position, raycastHits[i].transform.position);
                }
            }
        }
    }

    public void StopGame()
    {
        PlayerController.instance.Catched();
        watchOver = false;
        continueToMove = false;
        objectToMove.transform.DOKill();
    }

    public void MoveToPlayer(Vector3 policeManPosition, Vector3 playerPosition)
    {
        Vector3 movePoint = policeManPosition + ((playerPosition - policeManPosition) / 2);
        angry.Play();
        objectToMove.transform.DOMove(movePoint, 1f).OnComplete(Arrested);
        objectToMove.transform.DOLookAt(playerPosition, 1f);
    }

    private void Arrested()
    {
        animator.SetTrigger("Arrested");
        UIWinLose.instance.Lose();
    }
}
