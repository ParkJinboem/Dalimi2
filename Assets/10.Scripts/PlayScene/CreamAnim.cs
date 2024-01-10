using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamAnim : MonoBehaviour
{
    [SerializeField] private Animator creamAnim;
    public void OnEnable()
    {
        creamAnim.SetTrigger("AnimOn");
    }
}
