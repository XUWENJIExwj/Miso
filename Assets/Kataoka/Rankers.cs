using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NCMB
{
	public class Rankers
	{
		public int score { get; set; }
		public string name { get; private set; }

		// �R���X�g���N�^ -----------------------------------
		public Rankers(int _score, string _name)
		{
			score = _score;
			name = _name;
		}

		// �����L���O�ŕ\�����邽�߂ɕ�����𐮌` -----------
		public string print()
		{
			return name + " : "  + score;
		}
	}

}