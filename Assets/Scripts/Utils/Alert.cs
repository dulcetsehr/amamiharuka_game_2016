using UnityEngine;
using System.Collections;

namespace AmamiHaruka.Utils {

	public class Alert : MonoBehaviour {

		[SerializeField]
		private UnityEngine.UI.Text txtMessage;

		[SerializeField]
		private GameObject btnOk;
		[SerializeField]
		private GameObject btnYes;
		[SerializeField]
		private GameObject btnNo;

		private System.Action okCallback;
		private System.Action cancelCallback;


		private bool inited = false;
		private void Init() {
			if(inited) return;
			inited = true;

			gameObject.SetActive(false);
		}



		public void ShowBlock(string message) {
			gameObject.SetActive(true);
			btnOk.SetActive(false);
			btnYes.SetActive(false);
			btnNo.SetActive(false);

			txtMessage.text = message;
		}



		public void ShowAlert(string message) {
			ShowAlert(message, null);
		}
		public void ShowAlert(string message, System.Action okCallback) {
			Init();

			gameObject.SetActive(true);
			btnOk.SetActive(true);
			btnYes.SetActive(false);
			btnNo.SetActive(false);

			txtMessage.text = message;
			this.okCallback = okCallback;
		}



		public void ShowConfirm(string message) {
			ShowConfirm(message, null, null);
		}
		public void ShowConfirm(string message, System.Action okCallback) {
			ShowConfirm(message, okCallback, null);
		}
		public void ShowConfirm(string message, System.Action okCallback, System.Action cancelCallback) {
			Init();

			gameObject.SetActive(true);
			btnOk.SetActive(false);
			btnYes.SetActive(true);
			btnNo.SetActive(true);

			txtMessage.text = message;
			this.okCallback = okCallback;
			this.cancelCallback = cancelCallback;
		}

		public void Hide() {
			Init();

			gameObject.SetActive(false);
		}

		public void ClickOkButton() {
			Hide();
			if(okCallback != null) okCallback.Invoke();
		}

		public void ClickCancelButton() {
			Hide();
			if(cancelCallback != null) cancelCallback.Invoke();
		}

	}

}