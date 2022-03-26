using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskLoader : MonoBehaviour
{
    public int SiblingIndex;

    public GameObject Template;

    private void Start()
    {
        if (Template)
        {
            var instance = Instantiate(Template);
            instance.transform.SetParent(transform, false);
            instance.transform.SetSiblingIndex(SiblingIndex);
        }
    }
}
