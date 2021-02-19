using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OneByOne : MonoBehaviour
{
    public GameObject[] objectsToMove;
    public Animator[] animators;
    public ParticleSystem[] angry;
    public float passDistance;
    public float speedMotion;
    private int objectsIndex = 0;
    private int nextObjectIndex = 1;
    private Vector3 aim;

    private bool watchOver = true;
    private bool continueToMove = true;
    private void Start()
    {
        aim = objectsToMove[nextObjectIndex].transform.position;
        animators[objectsIndex].SetTrigger("Walk");
        objectsToMove[objectsIndex].transform.DOLookAt(objectsToMove[objectsIndex + 1].transform.position, 1f);
    }
    private void Update()
    {
        if (watchOver)
            LineOfSight();
        if(continueToMove)
        {
            Vector3 compare = objectsToMove[objectsIndex].transform.position - aim;

            if (compare.x < 0)
                compare.x *= -1;
            if (compare.y < 0)
                compare.y *= -1;
            if (compare.z < 0)
                compare.z *= -1;

            if (compare.x > passDistance || compare.y > passDistance || compare.z > passDistance)
                objectsToMove[objectsIndex].transform.position = Vector3.MoveTowards(objectsToMove[objectsIndex].transform.position, aim, speedMotion);
            else
            {
                animators[objectsIndex].SetTrigger("Stay");

                if (objectsIndex < objectsToMove.Length - 1)
                {
                    MoveToPolicemanPos(objectsToMove[objectsIndex].transform, objectsToMove[nextObjectIndex].transform.position);
                    objectsIndex++;
                    
                }
                else
                {
                    MoveToPolicemanPos(objectsToMove[objectsIndex].transform, objectsToMove[0].transform.position);
                    objectsIndex = 0;
                }

                if (nextObjectIndex < objectsToMove.Length - 1)
                {
                    nextObjectIndex++;
                }
                else
                {
                    nextObjectIndex = 0;
                }

                aim = objectsToMove[nextObjectIndex].transform.position;
                animators[objectsIndex].SetTrigger("Walk");

                if (objectsIndex != objectsToMove.Length - 1)
                    objectsToMove[objectsIndex].transform.DOLookAt(objectsToMove[objectsIndex + 1].transform.position, 1f);
                else
                    objectsToMove[objectsIndex].transform.DOLookAt(objectsToMove[0].transform.position, 1f);
            }
        }
    }

    private void LineOfSight()
    {
        for(int j = 0; j < objectsToMove.Length; j++)
        {
            RaycastHit[] raycastHits = new RaycastHit[10];
            Ray[] rays = new Ray[10];
            float rayDirection = -0.5f;

            for (int i = 0; i < rays.Length; i++)
            {
                rays[i] = new Ray(objectsToMove[j].transform.position, objectsToMove[j].transform.forward + (objectsToMove[j].transform.right * rayDirection));
                rayDirection += 0.1f;

                if (Physics.Raycast(rays[i], out raycastHits[i], 5f))
                {
                    if (raycastHits[i].transform.CompareTag("Player"))
                    {
                        StopGame(objectsToMove[j]);
                        MoveToPlayer(objectsToMove[j].transform.position, raycastHits[i].transform.position, objectsToMove[j], angry[j]);
                    }
                }
            }
        }
    }

    public void StopGame(GameObject objectToKillDG)
    {
        PlayerController.instance.Catched();
        watchOver = false;
        continueToMove = false;
        objectToKillDG.transform.DOKill();
    }

    private void MoveToPlayer(Vector3 policeManPosition, Vector3 playerPosition, GameObject policeMan, ParticleSystem angry)
    {
        Vector3 movePoint = policeManPosition + ((playerPosition - policeManPosition) / 2);
        angry.Play();
        policeMan.transform.DOMove(movePoint, 1f).OnComplete(Arrested);
        policeMan.transform.DOLookAt(playerPosition, 1f);
    }

    public void MoveToPlayer(Vector3 policeManPosition, Vector3 playerPosition, GameObject policeMan)
    {
        Vector3 movePoint = policeManPosition + ((playerPosition - policeManPosition) / 2);

        for(int i = 0; i < objectsToMove.Length; i++)
        {
            if (objectsToMove[i] == policeMan)
                angry[i].Play();
        }

        policeMan.transform.DOMove(movePoint, 1f).OnComplete(Arrested);
        policeMan.transform.DOLookAt(playerPosition, 1f);
    }

    private void Arrested()
    {
        for(int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("Arrested");
        }
        UIWinLose.instance.Lose();
    }

    private void MoveToPolicemanPos(Transform policeman, Vector3 position)
    {
        policeman.DOMove(position, 1f);
    }
}
