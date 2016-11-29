using UnityEngine;
using System.Collections;

namespace AmamiHaruka.Utils {
	
	public class SoundManager : MonoBehaviour {

		static private SoundManager instance;
		static public SoundManager Instance {
			get {
				if(instance == null) {
					GameObject obj = GameObject.Find("SoundManager");
					if(obj) instance = obj.GetComponent<SoundManager>();
					else {
						obj = new GameObject("SoundManager");
						instance = obj.AddComponent<SoundManager>();
					}
					instance.Initialize();
				}
				return instance;
			}
		}


		private AudioSource bgmSource1;
		private AudioSource bgmSource2;

		private AudioSource effSourceBtn;
		private AudioSource effSourceGameOver;

		void Start() {
			Initialize();
		}

		private bool initialized = false;
		private void Initialize() {
			if(initialized) return;
			initialized = true;

			bgmSource1 = gameObject.AddComponent<AudioSource>();
			bgmSource1.playOnAwake = false;
			bgmSource1.loop = true;

			bgmSource1.clip = ResourceManager.Instance.Load<AudioClip>("appends", "intro");

			bgmSource2 = gameObject.AddComponent<AudioSource>();
			bgmSource2.playOnAwake = false;
			bgmSource2.loop = true;
			bgmSource2.clip = Resources.Load("BGM/town") as AudioClip;

			effSourceBtn = gameObject.AddComponent<AudioSource>();
			effSourceBtn.playOnAwake = false;
			effSourceBtn.clip = Resources.Load("Effects/btn_press") as AudioClip;

			effSourceGameOver = gameObject.AddComponent<AudioSource>();
			effSourceGameOver.playOnAwake = false;
			effSourceGameOver.clip = Resources.Load("Effects/gameover") as AudioClip;
		}

		public void PlayBGM1() {
			if(DataStorage.isBgmPlaying && !bgmSource1.isPlaying) bgmSource1.Play();
		}
		public void StopBGM1() {
			if(bgmSource1.isPlaying) StartCoroutine(StopBgmCoroutine(bgmSource1));
		}

		public void PlayBGM2() {
			if(DataStorage.isBgmPlaying && !bgmSource2.isPlaying) bgmSource2.Play();
		}
		public void StopBGM2() {
			if(bgmSource2.isPlaying) StartCoroutine(StopBgmCoroutine(bgmSource2));
		}

		IEnumerator StopBgmCoroutine(AudioSource source) {
			while(true) {
				float vol = source.volume;
				vol -= 0.04f;
				if(vol <= 0) {
					source.Stop();
					source.volume = 1.0f;
					break;
				}
				source.volume = vol;
				yield return 1;
			}
		}


		public void PlayEffectBtn() {
			if(!DataStorage.isEffPlaying) return;
			if(effSourceBtn.isPlaying) effSourceBtn.Stop();
			effSourceBtn.Play();
		}

		public void PlayEffectGameOver() {
			if(!DataStorage.isEffPlaying) return;
			if(effSourceGameOver.isPlaying) effSourceGameOver.Stop();
			effSourceGameOver.Play();
		}


		void Awake() {
			DontDestroyOnLoad(gameObject);
		}

	}

}