using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveData : MonoBehaviour
{
    [SerializeField]
    private int SwarmNumber;
    [SerializeField]
    private float SwarmEjectWaitTime = 20f;
    [SerializeField]
    private List<GameObject> PipeFromHive;
    [SerializeField]
    private Material FilledPipeMat;

    [SerializeField]
    private GameObject BlackRibbon;

    private bool hasBees = false;

    private bool hasRibbon = false;

    private bool completed = false;

    private Beeeeez beeeeez;

    public bool GetCompletedState()
    {
        return completed;
    }

    public bool GetRibbonState()
    {
        return hasRibbon;
    }
    public void ApplyRibbon()
    {
        hasRibbon = true;
        BlackRibbon.SetActive(true);
    }

    /// <summary>
    /// Returns true if swarm number matches for the bees and the hive.
    /// </summary>
    /// <param name="_swarmNumber"></param>
    public bool BeesEnter(Beeeeez _beeeeez)
    {
        if (SwarmNumber == _beeeeez.GetSwarmNumber())
        {
            hasBees = true;
            beeeeez = _beeeeez;
            beeeeez.EnterHive(transform.position);
            CheckCompleted();
            return true;
        }
        else return false;
    }


    private void CheckCompleted()
    {
        if (hasBees && hasRibbon)
        {
            completed = true;
            foreach (GameObject pipe in PipeFromHive)
            {
                pipe.GetComponent<MeshRenderer>().material = FilledPipeMat;
            }
        }
        else StartCoroutine(EjectBeesTimer());

    }

    private IEnumerator EjectBeesTimer()
    {
        yield return new WaitForSeconds(SwarmEjectWaitTime);
        EjectBees();
    }

    private void EjectBees()
    {
        beeeeez.ScaredStopFollowing();
    }


}
