using System;
using System.Linq;

namespace AmamiHaruka.Utils {

	public class Helper {

		static public int GetTimestamp() {
			return GetTimestamp(DateTime.Now);
		}
		static public int GetTimestamp(DateTime dt) {
			return (int)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
		}

		static public byte[] HexToByteArray(string hex) {
			return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
		}

		static public string TwoDigitString(int number) {
			return number > 9 ? number.ToString() : "0" + number.ToString();
		}
		static public string ThreeDigitString(int number) {
			return number > 99 ? number.ToString() : (number > 9 ? "0" + number.ToString() : "00" + number.ToString());
		}

		static public string TimeFormat(float timestamp1000) {
			return ((int)Math.Floor(timestamp1000 / 60000.0f)) + ":" + TwoDigitString((int)Math.Floor(timestamp1000 / 1000.0f) % 60) + "." + ThreeDigitString((int)timestamp1000 % 1000);
		}

	}

}