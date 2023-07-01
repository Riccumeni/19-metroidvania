using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LightController : MonoBehaviour
{
    public GameObject player;
    private UnityEngine.Rendering.Universal.Light2D light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
        // StartCoroutine(turnOffLight());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator turnOffLight(){

        for(int i = 0; i < 20; i++){
            light.intensity = light.intensity - .03f;
            yield return new WaitForSeconds(.1f);
        }
    }
}
