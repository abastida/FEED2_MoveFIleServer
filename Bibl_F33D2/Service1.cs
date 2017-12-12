using System; 
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using Bibl_F33D2.Extensions;

namespace Bibl_F33D2
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        
        public string Carga_Archivo_F33D2(CArchivo request)
        {
            string bandera_mover = "";            

            if (Security.ValidateTokenSitio(request.Token, request.id_Sitio.ToString(), request.vchClaveSitio))
            {

                try
                {
                    string path = ConfigurationManager.AppSettings["path_nuevo"].ToString() + @"\";
                    if (!Directory.Exists(path))
                    {
                        try
                        {
                            // Crear directorio
                            DirectoryInfo di = Directory.CreateDirectory(path);
                            Log.EscribeLog("Se creo directorio");
                        }

                        catch (Exception ex)
                        {
                            return ex.Message.ToString();
                            Log.EscribeLog("Error creando directorio " + ex.Message.ToString());
                        }
                    }

                    // Copiar y mover archivo
                    MemoryStream ms = new MemoryStream(request.bytArchivo);
                    FileStream fs = new FileStream(path + request.vchpath + request.vchfilename, FileMode.Create);

                    ms.WriteTo(fs);
                    ms.Close();
                    fs.Close();
                    fs.Dispose();
                    bandera_mover = "OK";
                    Log.EscribeLog("Se movio archivo " + path + request.vchpath + request.vchfilename);
                }


                catch (Exception ex)
                {
                    bandera_mover = ex.Message;
                    return ex.Message.ToString();
                    Log.EscribeLog("Error moviendo archivo " + ex.Message.ToString());

                }
            }


            return bandera_mover;
        }
    }
}
