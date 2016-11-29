using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Security.Cryptography;


namespace AmamiHaruka.Utils {
	
	public class SecurityManager {
		private const int BlockSize = 256;
		private const int KeySize = 256;

		#region Base64
		public static byte[] Base64Encode(string src) {
			return Base64Encode(UTF8Encoding.UTF8.GetBytes(src));
		}
		public static byte[] Base64Encode(byte[] bytes) {
			return System.Text.UTF8Encoding.UTF8.GetBytes(Base64EncodeString(bytes));
		}
		public static string Base64EncodeString(string src) {
			return Base64EncodeString(UTF8Encoding.UTF8.GetBytes(src));
		}
		public static string Base64EncodeString(byte[] bytes) {
			return System.Convert.ToBase64String(bytes);
		}
		public static string Base64DecodeString(string src) {
			return UTF8Encoding.UTF8.GetString(Base64Decode(src));
		}
		public static byte[] Base64Decode(string src) {
			return System.Convert.FromBase64String(src);
		}
		#endregion

		static public string RSAEncrypt(string data, byte[] module, byte[] exponent) {
			RSACryptoServiceProvider csp = new RSACryptoServiceProvider(1536);
			csp.ImportParameters(new RSAParameters() { Modulus = module, Exponent = exponent });

			byte[] bytes = Encoding.UTF8.GetBytes(data);
			return Base64EncodeString(csp.Encrypt(bytes, false));
		}

	}

}