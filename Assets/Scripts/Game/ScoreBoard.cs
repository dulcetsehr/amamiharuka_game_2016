using UnityEngine;
using System.Collections;
using AmamiHaruka.Utils;

namespace AmamiHaruka.Games {

	public class ScoreBoard : MonoBehaviour {
		[SerializeField]
		private UnityEngine.UI.Text txtScore;
		[SerializeField]
		private UnityEngine.UI.Text txtTime;
		[SerializeField]
		private Mover mover;

		public void Print(long score, long time) {
			PrintScore(score);
			PrintTime(time);
		}
		public void PrintTime(long time) {
			txtTime.text = Helper.TimeFormat(time);
		}
		public void PrintScore(long score) {
			txtScore.text = score.ToString();
		}

	}

}