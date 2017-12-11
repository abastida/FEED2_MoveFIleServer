using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;




namespace Bibl_F33D2
{
    public class Security
    {
                /// <summary> 
         /// Permite validar si el token es válido 
         /// </summary> 
         /// <param name="Token"></param> 
         /// <returns></returns> 
         public static bool ValidateTokenSitio(String _Token, String id_Sitio, String vchClaveSitio)
         { 
             bool Valid = false; 
             try 
             { 
                 String validacion = Security.Decrypt(_Token); 
                 if (validacion != "") 
                 { 
                     //Log.EscribeLog("Validar: " + validacion); 
                     var _val = validacion.Split('|'); 
                     if (_val[0].ToString() == id_Sitio && _val[1].ToString() == vchClaveSitio) 
                     { 
                         Valid = true; 
                     } 
                 } 
             } 
             catch (Exception exc) {
            } 
             return Valid; 
         } 


          /// <summary> 
         /// Permite desencriptar la cadena con el algoritmo de rijndael 
         /// </summary> 
         /// <param name="Token"></param> 
         /// /// <returns></returns> 
         public static string Decrypt(string Token)
         { 
             string response = ""; 
             try 
             {

                
                 string CadenaEncriptada = HexToString(Token); 
                 byte[] Results; 
                 UTF8Encoding UTF8 = new UTF8Encoding(); 
                 MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider(); 
                 byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(ConfigurationManager.AppSettings["clave"].ToString())); 
                 TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider(); 
                 TDESAlgorithm.Key = TDESKey; 
                 TDESAlgorithm.Mode = CipherMode.ECB; 
                 TDESAlgorithm.Padding = PaddingMode.PKCS7; 
                 byte[] DataToDecrypt = Convert.FromBase64String(CadenaEncriptada); 
                 try 
                 { 
                     ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor(); 
                     Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length); 
                     response = UTF8.GetString(Results); 
                 } 
                 finally 
                 { 
                     TDESAlgorithm.Clear(); 
                     HashProvider.Clear(); 
                 } 
             } 
             catch (Exception e) {
            } 
             return response;
        } 

         /// <summary> 
         /// Converte una cadena de hexadecimal a un string, método inverso de ToHex 
         /// </summary> 
         /// <param name="hex"></param> 
         /// <returns></returns> 
         public static string HexToString(string hex)
         { 
             string cadena = ""; 
             try 
             { 
                 if (hex.Length % 2 == 0) 
                 { 
                     for (int i = 0; i<hex.Length; i = i + 2) 
                     { 
                         int value = Convert.ToInt32(hex.Substring(i, 2), 16); 
                         cadena += (char) value; 
                     } 
                 } 
                 else 
                 { 
                     throw new Exception("Cadena inválida"); 
                 } 
             } 
             catch (Exception e) {
            } 
             return cadena; 
         } 
     } 
}
