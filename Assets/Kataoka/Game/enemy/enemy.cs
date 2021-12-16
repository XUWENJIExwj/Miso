using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public bool gameOver;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

        if(gameOver)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        Destroy(this.gameObject);

        if(other.gameObject.tag =="Stage")
        {
            FindObjectOfType<enemypr>().DestroyObj();
            gameOver = true;
            //FindObjectOfType<Score>().Save();

        }
        
    }
}
