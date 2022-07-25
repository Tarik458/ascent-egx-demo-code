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
    private List<Material> PipeFromHiveMaterial;

    private bool hasBees = false;

    private bool hasRibbon = false;

    private bool completed = false;

    private Beeeeez beeeeez;

    public bool GetRibbonState()
    {
        return hasRibbon;
    }
    public void ApplyRibbon()
    {
        hasRibbon = true;
    }

    /// <summary>
    /// Returns true if swarm number matches for the bees and the hive.
    /// </summary>
    /// <param name="_swarmNumber"></param>
    public void BeesEnter(Beeeeez _beeeeez)
    {
        if (SwarmNumber == _beeeeez.GetSwarmNumber())
        {
            hasBees = true;
            beeeeez = _beeeeez;
            CheckCompleted();
        }
    }


    private void CheckCompleted()
    {
        if (hasBees && hasRibbon)
        {
            completed = true;
            foreach (Material mat in PipeFromHiveMaterial)
            {
                mat.SetColor("_Color", Color.yellow);
            }
            beeeeez.EnterHive(transform.position);
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
