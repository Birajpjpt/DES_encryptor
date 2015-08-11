using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data;
using System.IO;

namespace DESEncryptionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the string to encrypt");
            string x = Console.ReadLine();
            Console.WriteLine("Enter the pass");
            string y = Console.ReadLine();
            string z = Encryptor(y, x);
            Console.WriteLine(z);

            string a = Decryptor(z, y);
            Console.WriteLine(a);
            Console.ReadLine();

        }
        public static string Encryptor(string key_des, string x)
        {
            string strIV = "01234567";
            //Turn the plaintext into a byte array.
            byte[] PlainTextBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(x.ToString());

            //Setup the AES providor for our purposes.
            TripleDESCryptoServiceProvider desProvider = new TripleDESCryptoServiceProvider();

            desProvider.BlockSize = 64;
            desProvider.KeySize = 128;
            //My key and iv that i have used in openssl
            desProvider.Key = stringToByte(key_des, 16);
            desProvider.IV = string2byte(strIV);
            desProvider.Padding = PaddingMode.PKCS7;
            desProvider.Mode = CipherMode.CBC;

            ICryptoTransform cryptoTransform = desProvider.CreateEncryptor(desProvider.Key, desProvider.IV);
            byte[] EncryptedBytes = cryptoTransform.TransformFinalBlock(PlainTextBytes, 0, PlainTextBytes.Length);
            return Convert.ToBase64String(EncryptedBytes);
        }

        private static string Decryptor(string TextToDecrypt, string strKey)
        {
            byte[] EncryptedBytes = Convert.FromBase64String(TextToDecrypt);
            string strIV = "01234567";
            //Setup the AES provider for decrypting.            
            TripleDESCryptoServiceProvider desProvider = new TripleDESCryptoServiceProvider();
            //aesProvider.Key = System.Text.Encoding.ASCII.GetBytes(strKey);
            //aesProvider.IV = System.Text.Encoding.ASCII.GetBytes(strIV);
            desProvider.BlockSize = 64;
            desProvider.KeySize = 128;
            //My key and iv that i have used in openssl
            desProvider.Key = stringToByte(strKey, 16);
            desProvider.IV = string2byte(strIV);
            desProvider.Padding = PaddingMode.PKCS7;
            desProvider.Mode = CipherMode.CBC;


            ICryptoTransform cryptoTransform = desProvider.CreateDecryptor(desProvider.Key, desProvider.IV);
            byte[] DecryptedBytes = cryptoTransform.TransformFinalBlock(EncryptedBytes, 0, EncryptedBytes.Length);
            return System.Text.Encoding.ASCII.GetString(DecryptedBytes);
        }

        public static byte[] string2byte(string newString)
        {
            char[] CharArray = newString.ToCharArray();
            byte[] ByteArray = new byte[CharArray.Length];

            for (int i = 0; i < CharArray.Length; i++)
            {
                ByteArray[i] = Convert.ToByte(CharArray[i]);
            }
            return ByteArray;
        }

        public static byte[] stringToByte(string newString, int charLength)
        {
            char[] CharArray = newString.ToCharArray();
            byte[] ByteArray = new byte[charLength];
            for (int i = 0; i < CharArray.Length; i++)
            {
                ByteArray[i] = Convert.ToByte(CharArray[i]);
            }
            return ByteArray;
        }

        //public static string Byte2String(CryptoStream stream)
        //{
        //    string x = "";
        //    int i = 0;
        //    do
        //    {
        //        i = stream.ReadByte();
        //        if (i != -1) x += ((char)i);
        //    } while (i != -1);
        //    return (x);
        //}

        //public static string ByteToString(byte[] stream)
        //{
        //    string x = "";
        //    for (int i = 0; i < stream.Length; i++)
        //    {
        //        x += stream[i].ToString("X2");
        //    }
        //    return (x);
        //}

    }
}
