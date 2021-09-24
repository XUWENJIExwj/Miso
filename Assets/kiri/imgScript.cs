using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imgScript : MonoBehaviour
{

    Image image_f;

    // Start is called before the first frame update
    void Start() => image_f = GetComponent<Image>();

    // Update is called once per frame
    void Update()
    {

        if (image_f.fillAmount > 0.5f)
        {
            image_f.color = Color.green;
        }
        else if (image_f.fillAmount > 0.2f)
        {
            image_f.color = new Color(1f, 127f / 255f, 39f / 255f);
        }
        else
        {
            image_f.color = Color.red;
        }
    }
}