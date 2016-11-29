using UnityEngine;

namespace AmamiHaruka.Utils {

	public class Mover : MonoBehaviour {
		private float duration, start_time;
		private Vector2 from, to;

		private RectTransform retr;

		private System.Action completedListener;


		void Start() {
			Init();
		}

		private bool inited = false;
		private void Init() {
			if(inited) return;
			inited = true;

			retr = gameObject.GetComponent<RectTransform>();

			duration = start_time = 0;
			from = to = default(Vector2);
		}



		public void MoveFrom(Vector2 from, float duration, System.Action completeAction) {
			Init();
			if(retr == null) return;

			float x = retr.localPosition.x, y = retr.localPosition.y;
			Move(new Vector2(x+from.x, y+from.y), new Vector2(x, y), duration, completeAction);
		}
		public void MoveTo(Vector2 to, float duration, System.Action completeAction) {
			Init();
			if(retr == null) return;

			float x = retr.localPosition.x, y = retr.localPosition.y;
			Move(new Vector2(x, y), new Vector2(x+to.x, y+to.y), duration, completeAction);
		}
		public void Move(Vector2 from, Vector2 to, float duration, System.Action completeAction) {
			Init();
			if(retr == null) return;

			this.completedListener = completeAction;
			this.from = from;
			this.to = to;
			this.start_time = Time.time;
			this.duration = duration;
		}





		private void Update() {
			if(duration == 0 || retr == null) return;

			float elasped = Time.time - start_time;

			float x = Mathf.SmoothStep(from.x, to.x, (Time.time - start_time) / duration);
			float y = Mathf.SmoothStep(from.y, to.y, (Time.time - start_time) / duration);
			setPosition(x, y);

			if(elasped >= duration) {
				duration = start_time = 0;
				setPosition(to.x, to.y);
				from = to = default(Vector2);

				if(completedListener != null) completedListener.Invoke();
			}
		}

		private void setPosition(float x, float y) {
			float z = retr.localPosition.z;

			retr.localPosition = new Vector3(x, y, z);
		}

	}

}