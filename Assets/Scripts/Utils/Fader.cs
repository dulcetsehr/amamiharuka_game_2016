using UnityEngine;

namespace AmamiHaruka.Utils {

	public class Fader : MonoBehaviour {
		private UnityEngine.UI.Image image;
		private SpriteRenderer sprite;

		private float duration;
		private float from, to, start_time;

		private System.Action completedListener;

		private bool inited = false;
		private void Init() {
			if(inited) return;
			inited = true;

			gameObject.SetActive(true);
			image = gameObject.GetComponent<UnityEngine.UI.Image>();
			sprite = gameObject.GetComponent<SpriteRenderer>();
			gameObject.SetActive(false);

			from = to = duration = start_time = 0;
		}

		public void Show() {
			Init();

			this.completedListener = null;
			this.from = 1f;
			this.to = 1f;
			this.start_time = this.duration = 0;

			gameObject.SetActive(true);
			setAlpha(1.0f);
		}

		public void Hide() {
			Init();

			this.completedListener = null;
			this.from = 1f;
			this.to = 1f;
			this.start_time = this.duration = 0;

			setAlpha(1.0f);
			gameObject.SetActive(false);
		}


		public void FadeIn(float duration, System.Action completeAction) {
			Init();

			gameObject.SetActive(true);
			setAlpha(0f);

			this.completedListener = completeAction;

			this.from = 0f;
			this.to = 1f;
			this.start_time = Time.time;
			this.duration = duration;
		}

		public void FadeOut(float duration, System.Action completeAction) {
			Init();
			
			gameObject.SetActive(true);
			setAlpha(1f);

			this.completedListener = completeAction;

			this.from = 1f;
			this.to = 0f;
			this.start_time = Time.time;
			this.duration = duration;
		}

		private void Update() {
			if(duration == 0 || (image == null && sprite == null)) return;

			float alpha = Mathf.SmoothStep(from, to, (Time.time - start_time) / duration);
			setAlpha(alpha);
			
			if(alpha == to) {
				from = to = duration = start_time = 0;
				if(completedListener != null) completedListener.Invoke();
				if(alpha == 0) Hide();
			}
		}

		private void setAlpha(float alpha) {
			Color c = image != null ? image.color : sprite.color;
			c = new Color(c.r, c.b, c.g, alpha);

			if(image != null) image.color = c;
			if(sprite != null) sprite.color = c;
		}

	}

}