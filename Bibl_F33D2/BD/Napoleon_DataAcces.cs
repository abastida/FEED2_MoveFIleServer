using Bibl_F33D2.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibl_F33D2.BD
{
    

     public class Napoleon_DataAcces
    {
        private NAPOLEONEntities Nap_DB = new NAPOLEONEntities();

        public bool set_Archivo(tbl_MST_Archivos archivo)
        {
            bool bandera = false;

            try
            {
                using (Nap_DB = new NAPOLEONEntities())
                {
                    tbl_MST_Archivos tbl_ = new tbl_MST_Archivos();
                    if (Nap_DB.tbl_MST_Archivos.Any(x => x.vchPathFile == archivo.vchPathFile && x.id_Sitio == archivo.id_Sitio))
                    {                       
                        tbl_ = Nap_DB.tbl_MST_Archivos.First(x => x.vchPathFile == archivo.vchPathFile && x.id_Sitio == archivo.id_Sitio);
                        tbl_.bitUnZip = false;                        
                        Nap_DB.SaveChanges();
                       
                    }

                    else
                    {
                        tbl_.bitUnZip = false;
                        tbl_.vchPathFile = archivo.vchPathFile;
                        tbl_.id_Sitio = archivo.id_Sitio;
                        tbl_.datFecha = DateTime.Now;
                        Nap_DB.tbl_MST_Archivos.Add(tbl_);
                        Nap_DB.SaveChanges();
                    }
                }
                Log.EscribeLog("Se inserto informacion archivo " + archivo.vchPathFile);
                bandera = true; 
            }
            catch(Exception e)
            {
                Log.EscribeLog("Error al insertar informacion archivo. " + archivo.vchPathFile + " El ID de sitio es:" + archivo.id_Sitio.ToString() + " " + e.Message);
                bandera = false;
            }

            return bandera;
        }

    }
}
