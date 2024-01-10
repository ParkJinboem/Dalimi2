using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleareParticle : MonoBehaviour
{
    public ParticleSystem[] clearParticle;

    public void ShowClearParticle()
    {
        for (int i = 0; i < clearParticle.Length; i++)
        {
            clearParticle[i].Play();
        }
        //StartCoroutine(ShowParticle());
    }

    IEnumerator ShowParticle()
    {
        yield return new WaitForSeconds(3.0f);

        for (int i = 0; i < clearParticle.Length; i++)
        {
            clearParticle[i].Play();
        }
    }

    public void StopClearParticle()
    {
        StartCoroutine(StopParticle());
    }

    IEnumerator StopParticle()
    {

        for (int i = 0; i < clearParticle.Length; i++)
        {
            clearParticle[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(3.0f);

        for (int i = 0; i < clearParticle.Length; i++)
        {
            clearParticle[i].gameObject.SetActive(true);
        }
    }

}
