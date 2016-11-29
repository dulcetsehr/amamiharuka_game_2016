using UnityEngine;
using System.Collections;

namespace AmamiHaruka.Utils {

	public class BtnSound : MonoBehaviour {

		public void PlayBtnEffect() {
			SoundManager.Instance.PlayEffectBtn();
		}

	}
}