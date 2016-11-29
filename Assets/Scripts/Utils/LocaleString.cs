using UnityEngine;
using System.Collections;

namespace AmamiHaruka.Utils {

	public class LocaleString : MonoBehaviour {

		[SerializeField]
		private string key;

		void Start() {
			UnityEngine.UI.Text text = gameObject.GetComponent<UnityEngine.UI.Text>();
			text.text = LocaleManager.Instance.GetString(key);
		}

	}
}