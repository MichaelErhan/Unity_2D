using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetSunDirection : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Клавиша E - для смены дня и ночи (динамичкское освещение)");
    }
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }
}
