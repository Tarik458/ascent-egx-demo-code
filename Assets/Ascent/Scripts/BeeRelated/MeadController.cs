using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeadController : MonoBehaviour
{
    [SerializeField]
    private List<HiveData> Hives;

    [SerializeField]
    private List<MeshRenderer> SteppyStones;

    [SerializeField]
    private MeshRenderer IllBoi;

    [SerializeField]
    private Material HealthySteppyStoneMat;

    [SerializeField]
    private GameObject SteppyStoneBlocker;

    [SerializeField]
    private Material HealedBoiMat;


    private bool mead = false;

    private void Update()
    {
        if(!mead)
        {
            int completedCount = 0;
            foreach (HiveData hive in Hives)
            {
                if (hive.GetCompletedState())
                {
                    completedCount++;
                }
            }
            if (completedCount == Hives.Count)
            {
                Heal();
            }
        }
    }

    private void Heal()
    {
        foreach (MeshRenderer step in SteppyStones)
        {
            step.material = HealthySteppyStoneMat;
        }
        SteppyStoneBlocker.SetActive(false);
        IllBoi.material = HealedBoiMat;
    }

}
