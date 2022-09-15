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
    private GameObject IllBoi;

    [SerializeField]
    private List<MeshRenderer> IllBoisMeshes;

    [SerializeField]
    private Material HealthySteppyStoneMat;

    [SerializeField]
    private GameObject SteppyStoneBlocker;

    [SerializeField]
    private Material HealedBoiMat;

    [SerializeField]
    private MeshRenderer Water;
    [SerializeField]
    private Material CleanWater;

    [SerializeField]
    private TriggerZoneInfo triggerZoneInfo;


    private bool meadCompleted = false;
    private DialogueModule brothersDialogue;

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
            Debug.Log("ribbons placing " + value);
        }
    }

    private void Start()
    {
        brothersDialogue = IllBoi.GetComponent<DialogueModule>();
    }


    private void FixedUpdate()
    {
        if(!meadCompleted && ribbonsEnabled)
        {
            int completedCount = 0;
            int ribbonCount = 0;
            foreach (HiveData hive in Hives)
            {
                if (hive.GetCompletedState())
                {
                    completedCount++;
                }
                if(hive.GetRibbonState())
                {
                    ribbonCount++;
                }
            }


            // Check for stages of completion

            if (completedCount == Hives.Count)
            {
                meadCompleted = true;
                Heal();
            }
            else if (beePowersEnabled && brothersDialogue.GetCurrentDialogueIteration() < 3)
            { 
                brothersDialogue.SetDialogueIterationToUse(3);
            }
            else if (ribbonCount == Hives.Count && beePowersEnabled && brothersDialogue.GetCurrentDialogueIteration() < 5)
            {
                brothersDialogue.SetDialogueIterationToUse(5);
            }
        }
    }

    private void Heal()
    {
        foreach (MeshRenderer step in SteppyStones)
        {
            step.material = HealthySteppyStoneMat;
        }
        foreach (MeshRenderer boi in IllBoisMeshes)
        {
            boi.material = HealedBoiMat;
        }

        SteppyStoneBlocker.SetActive(false);
        Water.material = CleanWater;
        brothersDialogue.SetDialogueIterationToUse(7);
    }

}
