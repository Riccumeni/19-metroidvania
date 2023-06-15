using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextController : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int hp = player.GetComponent<PlayerController>().getHp();

        float dim = ((float) hp / 50) * 900;

        RectTransform rect = GetComponent<RectTransform>();

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dim);

        // resize(dim, new Vector3(1, 0, 0));
    }

    public void resize(float amount, Vector3 direction)
    {
        transform.position += direction * amount / 2; // Move the object in the direction of scaling, so that the corner on ther side stays in place
        transform.localScale += direction * amount; // Scale object in the specified direction
    }
}
