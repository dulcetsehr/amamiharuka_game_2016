using UnityEngine;
using System.Collections;

namespace AmamiHaruka.Games {

	public class HarukaWalker : MonoBehaviour {
		[SerializeField]
		private RectTransform[] harukaLeft;
		[SerializeField]
		private RectTransform[] harukaRight;
		[SerializeField]
		private RectTransform haruka;

		private RectTransform[] animateObject;


		public void MoveTo(int position) {
			int x = position * 180;
			int delta = x - (int)haruka.localPosition.x;
			if(delta == 0) return;

			StopCoroutine("Animate");

			int step = delta / 5;

			for(int i = 0; i < harukaLeft.Length; ++i) harukaLeft[i].gameObject.SetActive(false);
			for(int i = 0; i < harukaRight.Length; ++i) harukaRight[i].gameObject.SetActive(false);
			haruka.gameObject.SetActive(false);

			if(delta > 0) animateObject = harukaRight;
			else animateObject = harukaLeft;

			animateObject[0].localPosition = new Vector3(haruka.localPosition.x + step, 0, 0);
			animateObject[1].localPosition = new Vector3(haruka.localPosition.x + step*2, 0, 0);
			animateObject[2].localPosition = new Vector3(haruka.localPosition.x + step*3, 0, 0);
			animateObject[3].localPosition = new Vector3(haruka.localPosition.x + step*4, 0, 0);

			haruka.localPosition = new Vector3(x, 0, 0);

			StartCoroutine("Animate");

		}
		private IEnumerator Animate() {
			animateObject[0].gameObject.SetActive(true);
			yield return 2;
			animateObject[0].gameObject.SetActive(false);
			animateObject[1].gameObject.SetActive(true);
			yield return 2;
			animateObject[1].gameObject.SetActive(false);
			animateObject[2].gameObject.SetActive(true);
			yield return 2;
			animateObject[2].gameObject.SetActive(false);
			animateObject[3].gameObject.SetActive(true);
			yield return 2;
			animateObject[3].gameObject.SetActive(false);
			haruka.gameObject.SetActive(true);
		}

	}

}