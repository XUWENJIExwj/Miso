using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LeaderBoardManager : MonoBehaviour
{

	private LeaderBoard lBoard;
	public GameObject[] top = new GameObject[5];



	void Start()
	{
		lBoard = new LeaderBoard();

		// �e�L�X�g��\������Q�[���I�u�W�F�N�g���擾
		for (int i = 0; i < 5; ++i)
		{
			top[i] = GameObject.Find("Top" + i);
		}
		lBoard.fetchTopRankers(() => {
			if (lBoard.topRankers != null)
			{

				// �擾�����g�b�v5�����L���O��\��
				for (int i = 0; i < lBoard.topRankers.Count; ++i)
				{
					top[i].GetComponent<UnityEngine.UI.Text>().text = i + 1 + ". " + lBoard.topRankers[i].print();
				}

			}
		});

	}



	public void OnBack()
	{

		SceneManager.LoadScene("LogIn");
	}

}
