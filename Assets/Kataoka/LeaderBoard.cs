using NCMB;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class LeaderBoard
{

	public int currentRank = 0;
	public List<NCMB.Rankers> topRankers = null;

	// �T�[�o�[����g�b�v5���擾 ---------------    
	public void fetchTopRankers(Action completed)
	{
		// �f�[�^�X�g�A�́uHighScore�v�N���X���猟��

		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
		query.OrderByDescending("Score");
		query.Limit = 5;

		query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

			if (e != null)
			{
				//�������s���̏���
				Debug.Log("�����Ɏ��s");
				
			}
			else
			{
				//�����������̏���
				Debug.Log("�����ɐ���");
				List<NCMB.Rankers> list = new List<NCMB.Rankers>();
				// �擾�������R�[�h��score�N���X�Ƃ��ĕۑ�
				foreach (NCMBObject obj in objList)
				{
					int s = System.Convert.ToInt32(obj["Score"]);
					string n = System.Convert.ToString(obj["Name"]);
					list.Add(new Rankers(s, n));
				}
				topRankers = list;
				completed.Invoke();
			}
		});
	}

}

