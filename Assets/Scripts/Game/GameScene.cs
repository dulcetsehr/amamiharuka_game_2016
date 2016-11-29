using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using AmamiHaruka.Utils;
using AmamiHaruka.Games;


namespace AmamiHaruka.Scenes {

	public class GameScene : MonoBehaviour {

		[SerializeField]
		private Fader fader;

		[SerializeField]
		private Alert alert;

		[SerializeField]
		private Sprite[] sprites;

		[SerializeField]
		private UnityEngine.UI.Image imgReady;
		[SerializeField]
		private UnityEngine.UI.Image imgGo;
		[SerializeField]
		private Fader imgGameOver;

		[SerializeField]
		private HarukaWalker haruka;

		[SerializeField]
		private UnityEngine.UI.Image[] blockLefts;
		[SerializeField]
		private UnityEngine.UI.Image[] blockCenters;
		[SerializeField]
		private UnityEngine.UI.Image[] blockRights;

		[SerializeField]
		private ScoreBoard scoreBoard;

		[SerializeField]
		private ResultBoard resultBoard;

		private List<byte[]> blockData;

		private long starttime;
		private long StartTime {
			get { return starttime ^ 1048576; }
			set { starttime = value ^ 1048576; }
		}
		private long gamecount;
		private long GameCount {
			get { return gamecount ^ starttime; }
			set { gamecount = value ^ starttime; }
		}
		private long gametime;
		private long GameTime {
			get { return gametime ^ starttime; }
			set { gametime = value ^ starttime; }
		}

		private bool GameEnabled { get; set; }


		void Start() {
			fader.Show();

			starttime = 0;
			gamecount = 0;
			GameEnabled = false;

			for(int i = 0; i < blockCenters.Length; ++i) {
				blockLefts[i].gameObject.SetActive(false);
				blockCenters[i].gameObject.SetActive(false);
				blockRights[i].gameObject.SetActive(false);
			}

			// initialize
			blockData = new List<byte[]>();
			initBlock();

			fader.FadeOut(0.5f, () => {
				SoundManager.Instance.PlayBGM2();
				StartCoroutine("GameStartCoroutine");
			});
		}


		private void initBlock() {
			while(blockData.Count < blockCenters.Length) {
				byte[] dt = new byte[3] { 0, 0, 0 };

				int i = Random.Range(0, 3), j = 0;
				do {
					j = Random.Range(0, 5) + 1;
				} while(blockData.Count >= 1 && blockData[blockData.Count-1][i] == j);

				dt[i] = (byte)(j);
				blockData.Add(dt);
			}
			initUI();
		}
		private void initUI() {
			for(int i = 0; i < blockCenters.Length; ++i) {
				blockLefts[i].gameObject.SetActive(blockData[i][0] > 0);
				blockCenters[i].gameObject.SetActive(blockData[i][1] > 0);
				blockRights[i].gameObject.SetActive(blockData[i][2] > 0);
				if(blockData[i][0] > 0) blockLefts[i].sprite = sprites[blockData[i][0] - 1];
				if(blockData[i][1] > 0) blockCenters[i].sprite = sprites[blockData[i][1] - 1];
				if(blockData[i][2] > 0) blockRights[i].sprite = sprites[blockData[i][2] - 1];
			}
		}



		private IEnumerator GameStartCoroutine() {
			yield return new WaitForSeconds(1.0f);
			imgReady.gameObject.SetActive(false);
			imgGo.gameObject.SetActive(true);
			imgGo.gameObject.GetComponent<Scaler>().MoveScale(1.0f, 2.0f, 0.5f, null);
			imgGo.gameObject.GetComponent<Fader>().FadeOut(0.55f, null);

			StartTime = (long)System.Math.Floor(Time.time * 1000.0f);
			GameCount = 0;

			scoreBoard.Print(0, 0);
			GameEnabled = true;
		}




		#if UNITY_EDITOR
		private bool[] keydowned = new bool[3] { false, false, false };
		#endif
		void Update() {

			if(GameEnabled && StartTime > 0) {
				int rest = (int)((long)System.Math.Floor(Time.time * 1000.0f) - StartTime);
				scoreBoard.PrintTime(rest);
			}

			#if UNITY_EDITOR
			if(GameEnabled && Input.GetKeyUp(KeyCode.LeftArrow)) {
				keydowned[0] = false;
			}
			if(GameEnabled && Input.GetKeyUp(KeyCode.DownArrow)) {
				keydowned[1] = false;
			}
			if(GameEnabled && Input.GetKeyUp(KeyCode.RightArrow)) {
				keydowned[2] = false;
			}

			if(GameEnabled && !keydowned[0] && Input.GetKeyDown(KeyCode.LeftArrow)) {
				keydowned[0] = true;
				inputBlockLeft();
			}
			if(GameEnabled && !keydowned[1] && Input.GetKeyDown(KeyCode.DownArrow)) {
				keydowned[1] = true;
				inputBlockCenter();
			}
			if(GameEnabled && !keydowned[2] && Input.GetKeyDown(KeyCode.RightArrow)) {
				keydowned[2] = true;
				inputBlockRight();
			}
			#endif
		}

		public void inputBlockLeft() {
			if(GameEnabled) {
				SoundManager.Instance.PlayEffectBtn();
				haruka.MoveTo(-1);
				inputBlock(0);
			}
		}
		public void inputBlockCenter() {
			if(GameEnabled) {
				SoundManager.Instance.PlayEffectBtn();
				haruka.MoveTo(0);
				inputBlock(1);
			}
		}
		public void inputBlockRight() {
			if(GameEnabled) {
				SoundManager.Instance.PlayEffectBtn();
				haruka.MoveTo(1);
				inputBlock(2);
			}
		}

		private void inputBlock(int index) {
			byte[] dt = blockData[0];
			blockData.RemoveAt(0);
			if((dt[0] > 0 && index == 0) || (dt[1] > 0 && index == 1) || (dt[2] > 0 && index == 2)) {
				GameCount++;
				scoreBoard.PrintScore(GameCount);
				initBlock();
			} else {
				// game over
				GameEnabled = false;
				SoundManager.Instance.PlayEffectGameOver();

				GameTime = (long)System.Math.Floor(Time.time * 1000.0f) - StartTime;

				imgGameOver.Show();
				StartCoroutine("RenderGameOver");
			}
		}

		private IEnumerator RenderGameOver() {
			yield return new WaitForSeconds(0.5f);

			imgGameOver.FadeOut(0.5f, () => {
				resultBoard.Show(GameCount, GameTime);
			});
		}



		public void UploadData() {
			if(string.IsNullOrEmpty(resultBoard.Nickname.Trim())) {
				alert.ShowAlert(LocaleManager.Instance.GetString("msg_need_nickname"));
				return;
			}
			StartCoroutine("UploadDataCoroutine");
		}
		private IEnumerator UploadDataCoroutine() {
			resultBoard.Process();

			WWW www = new WWW(DataStorage.URL + "/3141592/prepare", System.Text.UTF8Encoding.UTF8.GetBytes("test"));
			yield return www;
			if(!string.IsNullOrEmpty(www.error)) {
				ShowError();
				yield break;
			}
			Dictionary<string, string> dict = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(www.text);

			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("s", GameCount);
			param.Add("t", GameTime);
			param.Add("n", resultBoard.Nickname.Trim());

			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Content-Type", "text/plain;charset=utf-8");
			headers.Add("X-Session-Id", dict["i"]);

			string encrypted = SecurityManager.RSAEncrypt(LitJson.JsonMapper.ToJson(param), SecurityManager.Base64Decode(dict["n"]), SecurityManager.Base64Decode(dict["e"]));
			WWW www2 = new WWW(DataStorage.URL + "/3141592/execute", System.Text.UTF8Encoding.UTF8.GetBytes(encrypted), headers);
			yield return www2;
			if(string.IsNullOrEmpty(www2.error)) {
				resultBoard.Complete();
			} else {
				resultBoard.Revert();
				ShowError();
			}

		}
		private void ShowError() {
			alert.ShowConfirm(LocaleManager.Instance.GetString("msg_retry"), () => {
				StartCoroutine("UploadDataCoroutine");
			}, () => {
				alert.ShowAlert(LocaleManager.Instance.GetString("msg_rank_failed"));
			});
		}


		public void GotoTitle() {
			fader.FadeIn(0.5f, () => {
				SoundManager.Instance.StopBGM2();

				SceneManager.LoadScene("Intro");
			});
		}

	}

}