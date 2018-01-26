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
using Bibl_F33D2.BD;

namespace Bibl_F33D2
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    //[ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
    public class Service1 : IService1
    {
        public Napoleon_DataAcces NAP = new Napoleon_DataAcces();

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
                    if (!Directory.Exists(path + request.vchpath))
                    {
                        try
                        {
                            // Crear directorio
                            DirectoryInfo di = Directory.CreateDirectory(path + request.vchpath);
                            Log.EscribeLog("Se creo directorio");
                        }

                        catch (Exception ex)
                        {
                            Log.EscribeLog("Error creando directorio " + ex.Message.ToString());
                            return ex.Message.ToString();                            
                        }
                    }

                    // Copiar y mover archivo
                    MemoryStream ms = new MemoryStream(request.bytArchivo);
                    FileStream fs = new FileStream(path + request.vchpath + request.vchfilename.Replace(".dcm", ".7z") , FileMode.Create);

                    ms.WriteTo(fs);


                    ms.Close();
                    fs.Close();
                    fs.Dispose();
                    tbl_MST_Archivos tbl_ = new tbl_MST_Archivos();

                    tbl_.vchPathFile = path + request.vchpath + request.vchfilename.Replace(".dcm", ".7z");
                    tbl_.id_Sitio = request.id_Sitio;

                    NAP.set_Archivo(tbl_);

                    bandera_mover = "OK";
                    Log.EscribeLog("Se movio archivo " + path + request.vchpath + request.vchfilename);                   
                }


                catch (Exception ex)
                {
                    bandera_mover = ex.Message;
                    Log.EscribeLog("Error moviendo archivo " + ex.Message.ToString());
                    return ex.Message.ToString();                    
                }
            }
            return bandera_mover;
        }


        //COmpresor zip
        //private static void DecompressFileLZMA(string inFile, string outFile)
        //{
        //    try
        //    {
        //        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        //        FileStream input = new FileStream(inFile, FileMode.Open);
        //        FileStream output = new FileStream(outFile, FileMode.Create);

        //        // Read the decoder properties
        //        byte[] properties = new byte[5];
        //        input.Read(properties, 0, 5);

        //        // Read in the decompress file size.
        //        byte[] fileLengthBytes = new byte[8];
        //        input.Read(fileLengthBytes, 0, 8);
        //        long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

        //        coder.SetDecoderProperties(properties);
        //        coder.Code(input, output, input.Length, fileLength, null);
        //        output.Flush();
        //        output.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.Write("Existe en un  error: " + e.Message);
        //    }
        //}

    }
}
