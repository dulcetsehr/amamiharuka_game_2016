using UnityEngine;
using System.Collections;
using AmamiHaruka.Utils;


namespace AmamiHaruka.Games {

	public class ResultBoard : MonoBehaviour {
		[SerializeField]
		private UnityEngine.UI.Text txtScore;
		[SerializeField]
		private UnityEngine.UI.Text txtTime;
		[SerializeField]
		private UnityEngine.UI.Text txtSpeed;
		[SerializeField]
		private UnityEngine.UI.InputField input;
		[SerializeField]
		private GameObject btn;
		[SerializeField]
		private GameObject done;

		public string Nickname { get { return input.text; } }

		public void Show(long score, long time) {
			gameObject.SetActive(true);

			input.text = PlayerPrefs.GetString("ranking_nickname", "");

			txtScore.text = score.ToString();
			txtTime.text = Helper.TimeFormat(time);
			txtSpeed.text = ((System.Math.Floor((float)score / time * 100000.0f)) / 100.0f) + "/s";
		}

		public void Process() {
			input.enabled = false;
			btn.SetActive(false);
			done.SetActive(false);
		}
		public void Revert() {
			input.enabled = true;
			btn.SetActive(true);
			done.SetActive(false);
		}
		public void Complete() {
			PlayerPrefs.SetString("ranking_nickname", input.text);
			PlayerPrefs.Save();

			input.enabled = false;
			btn.SetActive(false);
			done.SetActive(true);
		}

	}

}