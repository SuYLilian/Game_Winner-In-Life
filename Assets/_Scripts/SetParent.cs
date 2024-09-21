using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
    public string parentTag;
    void Awake()
    {
        gameObject.transform.parent = SettingParent();
    }

    public Transform SettingParent()
    {
        Transform parent;
        parent = GameObject.FindGameObjectWithTag(parentTag).transform;
        return parent;
    }

}
