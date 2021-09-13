using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemypr : MonoBehaviour
{

    public GameObject enemyPrefab;
    private float interval;
    private float time = 0f;
    //X���W�̍ŏ��l
    public float xMinPosition = -6f;
    //X���W�̍ő�l
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

        //�o�ߎ��Ԃ��������ԂɂȂ����Ƃ�(�������Ԃ��傫���Ȃ����Ƃ�)
        if (time > interval)
        {
            //enemy���C���X�^���X������(��������)
            GameObject enemy = Instantiate(enemyPrefab);
            //���������G�̍��W�����肷��
            enemy.transform.position = GetRandomPosition();
            //�o�ߎ��Ԃ����������čēx���Ԍv�����n�߂�
            time = 0f;
        }
    }


     private Vector3 GetRandomPosition()
    {
        float x = Random.Range(xMinPosition, xMaxPosition);

        //Vector3�^��Position��Ԃ�
        return new Vector3(x, 5, -1);
    }

    public void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
