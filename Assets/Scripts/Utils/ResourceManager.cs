using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AmamiHaruka.Utils {
	
	public class ResourceManager : MonoBehaviour {
		private Dictionary<string, AssetBundle> assetBundles = null;

		#region singleton instance
		static private ResourceManager instance;
		static public ResourceManager Instance {
			get {
				if(instance == null) {
					GameObject gameObject = GameObject.Find("_ResourceManager");
					if(gameObject == null) gameObject = new GameObject("_ResourceManager");
					instance = gameObject.GetComponent<ResourceManager>();
					if(instance == null) instance = gameObject.AddComponent<ResourceManager>();
				}
				return instance;
			}
		}
		#endregion

		#region initialize gameobject
		void Awake() { Init(); }
		void Start() { Init(); }
		private bool inited = false;
		private void Init() {
			if(inited) return;
			inited = true;

			if(assetBundles == null) assetBundles = new Dictionary<string, AssetBundle>();

			DontDestroyOnLoad(gameObject);
		}
		void OnDestroy() {

		}
		#endregion

		public bool IsError { get; private set; }
		public IEnumerator Download(string name, string url, int version) {
			IsError = false;
			//Debug.Log("Assetbundle Download: " + name + ", " + version);
			if(version != PlayerPrefs.GetInt("__bundle__" + name, 0)) {
				Caching.CleanCache();
			}

			WWW www = WWW.LoadFromCacheOrDownload(url, version);
			yield return www;
			if(string.IsNullOrEmpty(www.error) && www.assetBundle != null) {
				assetBundles.Add(name, www.assetBundle);
			}
			else {
				//Debug.LogError("Error on downloading assetbundle: " + www.error);
				IsError = true;
				yield break;
			}
		}

		public T Load<T>(string bundle, string key) where T : UnityEngine.Object {
			Init();

			if(assetBundles.ContainsKey(bundle)) {
				return assetBundles[bundle].LoadAsset(key) as T;
			} else {
				return Resources.Load(bundle + "/" + key) as T;
			}
		}
		public T Load<T>(string key) where T : UnityEngine.Object {
			Init();
			return Resources.Load(key) as T;
		}

	}

}