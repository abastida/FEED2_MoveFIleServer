using System;

namespace Bibl_F33D2
{
    public class CArchivo
    {
        public string Token { get; set; }
        public int id_Sitio { get; set; }
        public string vchClaveSitio { get; set; }      
        public byte[] bytArchivo { get; set; }
        public String vchfilename { get; set; }
        public string vchpath { get; set; }
       
        
        public CArchivo()
        {
            Token = string.Empty;
            id_Sitio = int.MinValue;
            vchClaveSitio = string.Empty;
            bytArchivo = null;
            vchfilename = string.Empty;
            vchpath = string.Empty;
        }
    }
}
