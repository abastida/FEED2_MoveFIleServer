using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Test_F33dt2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            string filename = @"C:\Anonymized20171205.dcm";

            String strFile = System.IO.Path.GetFileName(filename);
            FileInfo fInfo = new FileInfo(filename);
            long numBytes = fInfo.Length;
            double dLen = Convert.ToDouble(fInfo.Length / 1000000);

            FileStream fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);

            // convert the file to a byte array 
            byte[] data = br.ReadBytes((int)numBytes);
            br.Close();



            // pass the byte array (file) and file name to the web service 
            //string sTmp = srv.UploadFile(data, strFile);

            FEEDT2.Service1Client servicio_mover = new FEEDT2.Service1Client();
            string sTmp = servicio_mover.Carga_Archivo_F33D2(data, strFile, "Carpeta8");
            fStream.Close();
            fStream.Dispose();

            //Servicio_Feedt2.Service1Client prueba_servicio = new Servicio_Feedt2.Service1Client();           
            //servicio_mover.Carga_Archivo_F33D2();          
        }
    }
}
