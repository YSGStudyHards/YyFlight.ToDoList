using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// MD5加密解密帮助类
    /// </summary>
    public static class MD5Helper
    {
        #region 对字符串进行MD5摘要

        /// <summary>
        /// 对字符串进行MD5摘要
        /// </summary>
        /// <param name="message">需要摘要的字符串</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString(this string message)
        {
            MD5 md5 = MD5.Create();
            byte[] buffer = Encoding.Default.GetBytes(message);
            byte[] bytes = md5.ComputeHash(buffer);
            return GetHexString(bytes);
        }

        /// <summary>
        /// 对字符串进行MD5二次摘要
        /// </summary>
        /// <param name="message">需要摘要的字符串</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString2(this string message) => MDString(MDString(message));

        /// <summary>
        /// MD5 三次摘要算法
        /// </summary>
        /// <param name="s">需要摘要的字符串</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString3(this string s)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            byte[] bytes1 = md5.ComputeHash(bytes);
            byte[] bytes2 = md5.ComputeHash(bytes1);
            byte[] bytes3 = md5.ComputeHash(bytes2);
            return GetHexString(bytes3);
        }

        /// <summary>
        /// 对字符串进行MD5加盐摘要
        /// </summary>
        /// <param name="message">需要摘要的字符串</param>
        /// <param name="salt">盐</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString(this string message, string salt) => MDString(message + salt);

        /// <summary>
        /// 对字符串进行MD5二次加盐摘要
        /// </summary>
        /// <param name="message">需要摘要的字符串</param>
        /// <param name="salt">盐</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString2(this string message, string salt) => MDString(MDString(message + salt), salt);

        /// <summary>
        /// MD5 三次摘要算法
        /// </summary>
        /// <param name="s">需要摘要的字符串</param>
        /// <param name="salt">盐</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString3(this string s, string salt)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s + salt);
            byte[] bytes1 = md5.ComputeHash(bytes);
            byte[] bytes2 = md5.ComputeHash(bytes1);
            byte[] bytes3 = md5.ComputeHash(bytes2);
            return GetHexString(bytes3);
        }

        #endregion

        #region 获取文件的MD5值

        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="fileName">需要求MD5值的文件的文件名及路径</param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDFile(this string fileName)
        {
            var fs = new BufferedStream(File.Open(fileName, FileMode.Open, FileAccess.Read), 1048576);
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(fs);
            return GetHexString(bytes);
        }

        /// <summary>
        /// 计算文件的sha256
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string SHA256(this Stream stream)
        {
            var fs = new BufferedStream(stream, 1048576);
            SHA256Managed sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(fs);
            return BitConverter.ToString(checksum).Replace("-", string.Empty);
        }

        /// <summary>
        /// 获取数据流的MD5摘要值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>MD5摘要字符串</returns>
        public static string MDString(this Stream stream)
        {
            var fs = new BufferedStream(stream, 1048576);
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(fs);
            var mdstr = GetHexString(bytes);
            stream.Position = 0;
            return mdstr;
        }

        public static string GetHexString(byte[] bytes)
        {
            var hexArray = new char[bytes.Length << 1];
            for (var i = 0; i < hexArray.Length; i += 2)
            {
                var b = bytes[i >> 1];
                hexArray[i] = GetHexValue(b >> 4);       // b / 16
                hexArray[i + 1] = GetHexValue(b & 0xF);  // b % 16
            }
            return new string(hexArray, 0, hexArray.Length);

            char GetHexValue(int i)
            {
                if (i < 10)
                {
                    return (char)(i + '0');
                }
                return (char)(i - 10 + 'a');
            }
        }

        #endregion

        #region MD5解密解密

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey">密钥(8位字符)</param>
        /// <returns></returns>
        public static string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// MD5解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey">密钥(8位字符)</param>
        /// <returns></returns>
        public static string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }
}
