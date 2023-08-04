using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject referenceToPlayer;
    [SerializeField] private GameObject SkeletonPrefab;
    [SerializeField] private GameObject WerewolfPrefab;
    [SerializeField] private GameObject teleportVFXPrefab;
    [SerializeField] public Animator doorAnim;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private int spawnCount = 3;

    public void SpawnSkeleton()
    {
        Shuffle(spawnPoints);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject skeleObj = Instantiate(SkeletonPrefab, spawnPoints[i].position, Quaternion.identity);
            Instantiate(teleportVFXPrefab, spawnPoints[i].position, Quaternion.identity);

            skeleObj.GetComponent<EnemyCombatAI>().target = referenceToPlayer.transform;
            skeleObj.GetComponent<EnemyMovementAI>().player = referenceToPlayer.transform;
        }

        spawnCount += 2;
    }

    public void SpawnWerewolf()
    {
        int randomInt = Random.Range(0, spawnPoints.Length);
        GameObject werewolfObj = Instantiate(WerewolfPrefab, spawnPoints[randomInt].position, Quaternion.identity);
        Instantiate(teleportVFXPrefab, spawnPoints[randomInt].position, Quaternion.identity);

        werewolfObj.GetComponent<EnemyCombatAI>().target = referenceToPlayer.transform;
        werewolfObj.GetComponent<EnemyMovementAI>().player = referenceToPlayer.transform;
        werewolfObj.GetComponent<Health>().OnDeath.AddListener(OpenTheDamnDoor);
    }

    private void Shuffle(Transform[] elements)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            Transform tmp = spawnPoints[i];
            int r = Random.Range(i, elements.Length);
            elements[i] = elements[r];
            elements[r] = tmp;
        }
    }

    private void OpenTheDamnDoor()
    {
        doorAnim.SetTrigger("IsInteracted");
    }
}
