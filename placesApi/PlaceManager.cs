using noef.controllers;
using noef.models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace placesApi
{
    public class PlaceManager : IDisposable
    {
        private Payloads Payloads;

        private const string insert = "BEGIN;lock table place in share mode;" +
            "select insertaplace(" +
            "@titulo, @subtitulo, @descripcion,@telefono , @link);COMMIT;ROLLBACK;";

        private const string registrar = "BEGIN;lock table login in share mode;" +
            "INSERT INTO public.login(correo, contrasena) " +
            "VALUES(@correo, encrypt('@contra', 'contrasena', '3des'));COMMIT;ROLLBACK;";

        private const string imagenInsert = "BEGIN;lock table public.imagen,public.place_imagen in share mode;" +
            "select insertaimagen(@idplace,@datos);COMMIT;ROLLBACK;";


        private const string imagenesXPlace = "" +
            "select listaimagenes(@id);";

        public const string select = "" +
            "SELECT  p.id, p.titulo,p.subtitulo,p.descripcion,p.telefono,p.link, CASE " +
            "WHEN pi.idimg is null THEN 'null' " +
            "else pi.idimg " +
            " END " +
            "AS idImagen " +
            "FROM place p " +
            " left join place_imagen pi on p.id= pi.id;";


        public const string selectLogin = "" +
            "SELECT correo,encode(decrypt(contrasena,'contrasena','3des'::text) ,'escape'::text)" +
            " as contrasena " +
            "FROM public.login where correo=@correo " +
            "and encode(decrypt(contrasena,'contrasena','3des'::text) ,'escape'::text)=@contra;";


        public const string selectImg = "" +
            "SELECT id, data FROM public.imagen;";

        public const string selectOneImg = "" +
            "SELECT id, data FROM public.imagen where id=@id;COMMIT;";
        public PlaceManager()
        {
            Payloads = new Payloads();
        }
        public async Task<List<dynamic>> consultarPlaces()
        {
            try
            {
               return await Payloads.SelectFromDatabaseGenericObject(Startup.Conexion,select);

            }
            catch (Exception)
            {
                return new List<dynamic>();
            }
        }
        public async Task<string> crearPlace(Place place)
        {
            try
            {

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("@titulo", place.Titulo);
                keys.Add("@subtitulo", place.Subtitulo);
                keys.Add("@descripcion", place.Descripcion);
                keys.Add("@telefono", place.Telefono);
                keys.Add("@link", place.linkMaps);

            
                List<dynamic> result = await Payloads.SelectFromDatabaseGenericObject(Startup.Conexion,
                    insert,keys);

                return "ok";
               
               
            }
            catch (Exception k)
            { return "error"; }
        }


        public async Task<string> registro(Login login)
        {
            try
            {

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("@correo", login.Correo);
                keys.Add("@contra",login.Contrasena);


               int result = await Payloads.InsertOrUpdateOrDeleteDatabase(Startup.Conexion,
                    registrar, keys);

                return result==1?"realizado":"error";


            }
            catch (Exception k)
            { return "error"; }
        }

        public async Task<string> guardarImagen(Imagen img)
        {
            try
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("@idplace", img.Id);
                keys.Add("@datos", img.Data);

                List<dynamic> result = await Payloads.SelectFromDatabaseGenericObject(Startup.Conexion,
                     imagenInsert, keys);

                return "";
            }
            catch (Exception k)
            { return "error"; }
        }

        public async Task<List<dynamic>> consultarImagenesXPlace(string id)
        {
            try
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("@id", id);

                return await Payloads.SelectFromDatabaseGenericObject(Startup.Conexion, imagenesXPlace,keys);

            }
            catch (Exception)
            {
                return new List<dynamic>();
            }
        }


        public async Task<List<dynamic>> consultarImagenesXIdVisual(string id)
        {
            try
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("@id", id);

                return await Payloads.SelectFromDatabaseGenericObject(Startup.Conexion, selectOneImg, keys);

            }
            catch (Exception)
            {
                return new List<dynamic>();
            }
        }
        public async Task<List<dynamic>> consultarImagenes()
        {
            try
            {
                return await Payloads.SelectFromDatabaseGenericObject(Startup.Conexion, selectImg);

            }
            catch (Exception)
            {
                return new List<dynamic>();
            }
        }



        public async Task<List<dynamic>> validarUsuario(Login login)
        {

            try
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("@correo", login.Correo);
                keys.Add("@contra", login.Contrasena);

                return await Payloads.SelectFromDatabaseGenericObject(Startup.Conexion, selectLogin, keys);
            }
            catch (Exception)
            {

                return new List<dynamic>();
            }
           


        }
        public void Dispose()
        {
            Payloads = null;
        }
    }
}
