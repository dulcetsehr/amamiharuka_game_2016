using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using AmamiHaruka.Utils;


namespace AmamiHaruka.Scenes {

	public class AboutScene : MonoBehaviour {

		[SerializeField]
		private Fader fader;

		[SerializeField]
		private UnityEngine.UI.RawImage image;


		void Start() {
			string lang = PlayerPrefs.GetString("AboutLanguage");
			if(string.IsNullOrEmpty(lang))
				lang = Application.systemLanguage.ToString();

			change(lang);

			fader.FadeOut(0.5f, null);
		}

		private void change(string language) {
			Texture2D tx = ResourceManager.Instance.Load<Texture2D>("appends", "about_" + language);
			if(tx == null)
				tx = ResourceManager.Instance.Load<Texture2D>("appends", "about");

			PlayerPrefs.SetString("AboutLanguage", language);
			PlayerPrefs.Save();

			image.texture = tx;
		}

		public void changeKorean() {
			change("Korean");
		}
		public void changeJapanese() {
			change("Japanese");
		}

		public void GotoTitle() {
			fader.FadeIn(0.5f, () => {
				SceneManager.LoadScene("Intro");
			});
		}

	}

}