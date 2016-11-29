using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AmamiHaruka.Utils {

	public class LocaleManager : MonoBehaviour {

		static private LocaleManager instance;
		static public LocaleManager Instance {
			get {
				if(instance == null) {
					GameObject obj = GameObject.Find("LocaleManager");
					if(obj) instance = obj.GetComponent<LocaleManager>();
					else {
						obj = new GameObject("LocaleManager");
						instance = obj.AddComponent<LocaleManager>();
					}
					instance.Initialize();
				}
				return instance;
			}
		}



		private Dictionary<string, string> keywords;


		void Start() {
			Initialize();
		}

		private bool initialized = false;
		private void Initialize() {
			if(initialized) return;
			initialized = true;

			string data;
			if(Application.systemLanguage == SystemLanguage.Japanese) {
				data = (Resources.Load("locale.ja") as TextAsset).text;
			} else {
				data = (Resources.Load("locale.ko") as TextAsset).text;
			}

			keywords = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(data);
		}

		void Awake() {
			DontDestroyOnLoad(gameObject);
		}


		public string GetString(string key) {
			return keywords.ContainsKey(key) ? keywords[key] : key;
		}

	}

}