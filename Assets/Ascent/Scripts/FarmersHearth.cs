using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmersHearth : MonoBehaviour
{
    [SerializeField]
    private FireFlicker FarmersFire;

    [SerializeField]
    private DialogueModule FarmersDialogue;

    [SerializeField]
    private DialogueModule FarmersFamilyDialogue;

    private void Start()
    {
        StartCoroutine(CheckFireLit());
    }

    private IEnumerator CheckFireLit()
    {
        while(!FarmersFire.GetLitState())
        {
            yield return new WaitForSeconds(2f);
        }
        ChangeFarmerDialogue();
    }

    private void ChangeFarmerDialogue()
    {
        FarmersDialogue.SetDialogueIterationToUse(3);
        FarmersFamilyDialogue.SetDialogueIterationToUse(2);
    }

}
