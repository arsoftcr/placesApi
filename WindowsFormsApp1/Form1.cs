using Newtonsoft.Json;
using noef.controllers;
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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        noef.models.Conexion Conexion = new noef.models.Conexion
        {
            Servidor="201.204.169.171",
            BaseDatos="places",
            Password="admin",
            Usuario="postgres",
            Port="5432",
            Tipo=noef.models.tipo.Postgres
        };
        string co = "BEGIN;lock table public.imagen,public.place_imagen in share mode;" +
            "select insertaimagen(@idplace,@datos);COMMIT;ROLLBACK;";
        public Form1()
        {
            InitializeComponent();
        }

        async void Form1_Load(object sender, EventArgs e)
        {
            string ruta = @"C:\Users\Administrator\Pictures\about.jpg";

          
            string data = "";
            byte[] imageArray = System.IO.File.ReadAllBytes(ruta);
            data = Convert.ToBase64String(imageArray);

            Dictionary<string, object> keys = new Dictionary<string, object>();

            keys.Add("@idplace", "7d4e77aa-a422-4eb1-a48a-e8f17f860c53");
            keys.Add("@datos", imageArray);

            Payloads payloads = new Payloads();

           int re=await payloads.InsertOrUpdateOrDeleteDatabase(Conexion,co,keys);


        }
    }
}
