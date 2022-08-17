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

    [SerializeField]
    private TriggerZoneInfo triggerZoneInfo;

    private bool mead = false;

    // Bee Puzzle Vars
    private bool beePowersEnabled = false;
    public bool BeePowersEnabled
    {
        get { return beePowersEnabled; }
        set 
        {
            beePowersEnabled = value;
            triggerZoneInfo.canControlBees = value;
        }
    }

    private bool ribbonsEnabled = false;
    public bool RibbonsEnabled
    {
        get { return ribbonsEnabled; }
        set
        {
            ribbonsEnabled = value;
            triggerZoneInfo.canPlaceRibbons = value;
        }
    }



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
