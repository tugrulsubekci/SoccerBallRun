using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    public bool isDestroyed;
    private Animator panelAnim;
    private void Start()
    {
        panelAnim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isDestroyed)
        {
            panelAnim.SetBool("isDestroyed", true);
        }
    }
}
