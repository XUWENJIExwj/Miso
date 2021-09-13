using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemypr : MonoBehaviour
{

    public GameObject enemyPrefab;
    private float interval;
    private float time = 0f;
    //X座標の最小値
    public float xMinPosition = -6f;
    //X座標の最大値
    public float xMaxPosition = 6f;
   

    // Start is called before the first frame update
    void Start()
    {
        interval = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        //経過時間が生成時間になったとき(生成時間より大きくなったとき)
        if (time > interval)
        {
            //enemyをインスタンス化する(生成する)
            GameObject enemy = Instantiate(enemyPrefab);
            //生成した敵の座標を決定する
            enemy.transform.position = GetRandomPosition();
            //経過時間を初期化して再度時間計測を始める
            time = 0f;
        }
    }


     private Vector3 GetRandomPosition()
    {
        float x = Random.Range(xMinPosition, xMaxPosition);

        //Vector3型のPositionを返す
        return new Vector3(x, 5, -1);
    }

    public void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
