using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using AmamiHaruka.Utils;
using AmamiHaruka.Data;
using AmamiHaruka.Intro;


namespace AmamiHaruka.Scenes {
	
	public class IntroScene : MonoBehaviour {

		[SerializeField]
		private Fader fader;
		[SerializeField]
		private Fader logoFader;
		[SerializeField]
		private GameObject txtLoading;

		[SerializeField]
		private Alert alert;

		[SerializeField]
		private FirstScene firstScene;

		[SerializeField]
		private GameObject imgTitle;

		[SerializeField]
		private UnityEngine.UI.Button btnBgmOn;
		[SerializeField]
		private UnityEngine.UI.Button btnBgmOff;
		[SerializeField]
		private UnityEngine.UI.Button btnEffOn;
		[SerializeField]
		private UnityEngine.UI.Button btnEffOff;


		private int m = 0, d = 0;

		void Start() {
			DataStorage.isBgmPlaying = !PlayerPrefs.HasKey("BGM_PLAY_OFF");
			DataStorage.isEffPlaying = !PlayerPrefs.HasKey("EFF_PLAY_OFF");

			initSoundBtn();

			fader.Show();
			txtLoading.SetActive(false);

			if(DataStorage.isLogoShown) {
				fader.FadeOut(0.5f, () => {
					ShowTitle();
				});
			} else {
				StartCoroutine("LoadAssetBundle");
			}
		}


		private void initSoundBtn() {
			btnBgmOn.gameObject.SetActive(DataStorage.isBgmPlaying);
			btnBgmOff.gameObject.SetActive(!DataStorage.isBgmPlaying);
			btnEffOn.gameObject.SetActive(DataStorage.isEffPlaying);
			btnEffOff.gameObject.SetActive(!DataStorage.isEffPlaying);
		}

		private IEnumerator LoadAssetBundle() {
			txtLoading.SetActive(true);

			WWWForm form = new WWWForm();
			form.AddField("platform", Application.platform.ToString());
			form.AddField("deviceid", SystemInfo.deviceUniqueIdentifier);
			form.AddField("phonetype", SystemInfo.deviceModel);
			form.AddField("osversion", SystemInfo.operatingSystem);
			form.AddField("appversion", "1.0.0");

			WWW www = new WWW(DataStorage.URL + "/3141592/initialize", form);
			yield return www;
			if(!string.IsNullOrEmpty(www.error)) {
				ShowError();
				yield break;
			}
			InitializeData data = LitJson.JsonMapper.ToObject<InitializeData>(www.text);
			if(data.assetbundles != null) {
				foreach(BundleInfo bundle in data.assetbundles) {
					yield return StartCoroutine(ResourceManager.Instance.Download(bundle.f, System.IO.Path.Combine(data.path, bundle.f), bundle.v));
					if(ResourceManager.Instance.IsError) {
						ShowError();
						yield break;
					}
				}
			}
			m = data.m;
			d = data.d;

			txtLoading.SetActive(false);
			DataStorage.isLogoShown = true;

			logoFader.FadeIn(0.5f, () => {
				StartCoroutine("LogoHideCoroutine");
			});
		}

		private void ShowError() {
			alert.ShowConfirm(LocaleManager.Instance.GetString("msg_retry"), () => {
				StartCoroutine("LoadAssetBundle");
			}, () => {
				alert.ShowBlock(LocaleManager.Instance.GetString("msg_init_failed"));
			});
		}



		private IEnumerator LogoHideCoroutine() {
			yield return new WaitForSeconds(1.0f);

			if(m == 4 && d == 3 && PlayerPrefs.GetInt("birthday_scene", 0) == 0) {
				PlayerPrefs.SetInt("birthday_scene", 1);
				PlayerPrefs.Save();
				logoFader.FadeOut(0.5f, () => {
					StartCoroutine("ShowFirst");
				});
			} else {
				fader.FadeOut(0.5f, null);
				logoFader.FadeOut(0.5f, () => {
					ShowTitle();
				});
			}
		}


		private IEnumerator ShowFirst() {
			yield return StartCoroutine(firstScene.Show());
			ShowTitle();
		}

		private void ShowTitle() {
			fader.Hide();
			logoFader.Hide();

			imgTitle.GetComponent<Fader>().FadeIn(1f, null);
			imgTitle.GetComponent<Mover>().MoveFrom(new Vector2(0f, 80f), 1f, null);

			SoundManager.Instance.PlayBGM1();
		}




		private bool clicked = false;
		public void StartGame() {
			if(clicked) return;
			clicked = true;

			fader.FadeIn(0.5f, () => {
				SoundManager.Instance.StopBGM1();
				SceneManager.LoadScene("Game");
			});
		}

		public void GotoAbout() {
			if(clicked) return;
			clicked = true;

			fader.FadeIn(0.5f, () => {
				SceneManager.LoadScene("About");
			});
		}

		public void SetBgmOn() {
			PlayerPrefs.DeleteKey("BGM_PLAY_OFF");
			PlayerPrefs.Save();
			DataStorage.isBgmPlaying = true;
			SoundManager.Instance.PlayBGM1();
			initSoundBtn();
		}
		public void SetBgmOff() {
			PlayerPrefs.SetInt("BGM_PLAY_OFF", 1);
			PlayerPrefs.Save();
			DataStorage.isBgmPlaying = false;
			SoundManager.Instance.StopBGM1();
			initSoundBtn();
		}

		public void SetEffOn() {
			PlayerPrefs.DeleteKey("EFF_PLAY_OFF");
			PlayerPrefs.Save();
			DataStorage.isEffPlaying = true;
			initSoundBtn();
		}
		public void SetEffOff() {
			PlayerPrefs.SetInt("EFF_PLAY_OFF", 1);
			PlayerPrefs.Save();
			DataStorage.isEffPlaying = false;
			initSoundBtn();
		}


		public void GoRank() {
			Application.OpenURL(DataStorage.URL+"/ranking");
		}


	}

}