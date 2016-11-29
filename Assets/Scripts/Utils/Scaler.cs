using UnityEngine;

namespace AmamiHaruka.Utils {

	public class Scaler : MonoBehaviour {
		[SerializeField]
		private RectTransform target;

		private float duration;
		private float from, to, start_time;

		private System.Action completedListener;

		private bool inited = false;
		private void Init() {
			if(inited) return;
			inited = true;

			from = to = duration = start_time = 0;
		}

		public void MoveScale(float from, float to, float duration, System.Action completeAction) {
			Init();

			gameObject.SetActive(true);
			setScale(from);

			this.completedListener = completeAction;

			this.from = from;
			this.to = to;
			this.start_time = Time.time;
			this.duration = duration;
		}

		private void Update() {
			if(duration == 0) return;

			float scale = Mathf.SmoothStep(from, to, (Time.time - start_time) / duration);
			setScale(scale);

			if(scale == to) {
				from = to = duration = start_time = 0;
				if(completedListener != null) completedListener.Invoke();
			}
		}

		private void setScale(float scale) {
			target.localScale = new Vector3(scale, scale, 1);
		}

	}

}