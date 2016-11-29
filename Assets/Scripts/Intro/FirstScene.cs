using UnityEngine;
using System.Collections;
using AmamiHaruka.Utils;

namespace AmamiHaruka.Intro {

	public class FirstScene : MonoBehaviour {

		[SerializeField]
		private AudioSource bgaudio;

		[SerializeField]
		private GameObject date;
		[SerializeField]
		private GameObject title1;
		[SerializeField]
		private GameObject title2;
		[SerializeField]
		private Fader first;

		void Start() {
			Init();
		}

		private bool inited = false;
		private void Init() {
			if(inited) return;
			inited = true;

			date.SetActive(false);
			title1.SetActive(false);
			title2.SetActive(false);
			first.Hide();
			gameObject.SetActive(false);
		}

		public IEnumerator Show() {
			Init();
			gameObject.SetActive(true);
			if(DataStorage.isBgmPlaying) bgaudio.Play();

			yield return new WaitForSeconds(1.5f);
			 
			date.GetComponent<Fader>().FadeIn(1.5f, null);
			date.GetComponent<Mover>().MoveFrom(new Vector2(-100f, 0f), 1f, null);
			yield return new WaitForSeconds(3f);

			title1.GetComponent<Fader>().FadeIn(1f, null);
			title1.GetComponent<Mover>().MoveFrom(new Vector2(-50f, 0f), 1f, null);
			yield return new WaitForSeconds(2.5f);

			title2.GetComponent<Fader>().FadeIn(1f, null);
			title2.GetComponent<Mover>().MoveFrom(new Vector2(-50f, 0f), 1f, null);
			yield return new WaitForSeconds(2.5f);

			first.Show();
			if(DataStorage.isBgmPlaying) first.GetComponent<AudioSource>().Play();
			date.SetActive(false);
			title1.SetActive(false);
			title2.SetActive(false);
			first.FadeOut(1.0f, () => {
				gameObject.SetActive(false);
			});
		}

	}

}