using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmersHearth : MonoBehaviour
{
    [SerializeField]
    private FireFlicker FarmersFire;

    [SerializeField]
    private DialogueModule FarmersDialogue;

    private void Start()
    {
        StartCoroutine(CheckFireLit());
    }

    private IEnumerator CheckFireLit()
    {
        while(FarmersFire.GetLitState() != false)
        {
            yield return new WaitForSeconds(2f);
        }
        ChangeFarmerDialogue();
    }

    private void ChangeFarmerDialogue()
    {
        FarmersDialogue.SetDialogueIterationToUse(4);
    }

}
