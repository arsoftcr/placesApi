using noef.controllers;
using noef.models;
using System;
using System.Collections.Generic;
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


        private const string imagenesXPlace = "BEGIN;lock table imagen,place_imagen,place in share mode;" +
            "select listaimagenes(@id);COMMIT;ROLLBACK;";

        public const string select = "BEGIN;lock table place,place_imagen in share mode;" +
            "SELECT  p.id, p.titulo,p.subtitulo,p.descripcion,p.telefono,p.link, CASE " +
            "WHEN pi.idimg is null THEN 'null' " +
            "else pi.idimg " +
            " END " +
            "AS idImagen " +
            "FROM place p " +
            " left join place_imagen pi on p.id= pi.id;COMMIT;ROLLBACK;";


        public const string selectLogin = "BEGIN;lock table login in share mode;" +
            "SELECT correo,encode(decrypt(contrasena,'contrasena','3des'::text) ,'escape'::text)" +
            " as contrasena " +
            "FROM public.login where correo=@correo " +
            "and encode(decrypt(contrasena,'contrasena','3des'::text) ,'escape'::text)=@contra;COMMIT;ROLLBACK;";


        public const string selectImg = "BEGIN;lock table imagen in share mode;" +
            "SELECT id, data FROM public.imagen;COMMIT;ROLLBACK;";

        public const string selectOneImg = "BEGIN;lock table imagen in share mode;" +
            "SELECT id, data FROM public.imagen where id=@id;COMMIT;ROLLBACK;";
        public PlaceManager()
        {
            Payloads = new Payloads();
        }
        public async Task<List<Dictionary<string,object>>> consultarPlaces()
        {
            try
            {
               return await Payloads.SelectFromDatabase(Startup.Conexion,select);

            }
            catch (Exception)
            {
                return new List < Dictionary<string, object>>();
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

            
                List<Dictionary<string, object>> result = await Payloads.SelectFromDatabase(Startup.Conexion,
                    insert,keys);

                return result != null ? result[0].ContainsKey("insertaplace") ? 
                    $"{result[0]["insertaplace"]}" : "error":"error";
               
               
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

                List<Dictionary<string, object>> result = await Payloads.SelectFromDatabase(Startup.Conexion,
                     imagenInsert, keys);

                return result != null ? result[0].ContainsKey("insertaimagen") ?
                    $"{result[0]["insertaimagen"]}" : "error" : "error";
            }
            catch (Exception k)
            { return "error"; }
        }

        public async Task<List<Dictionary<string, object>>> consultarImagenesXPlace(string id)
        {
            try
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("@id", id);

                return await Payloads.SelectFromDatabase(Startup.Conexion, imagenesXPlace,keys);

            }
            catch (Exception)
            {
                return new List<Dictionary<string, object>>();
            }
        }


        public async Task<List<Dictionary<string, object>>> consultarImagenesXIdVisual(string id)
        {
            try
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("@id", id);

                return await Payloads.SelectFromDatabase(Startup.Conexion, selectOneImg, keys);

            }
            catch (Exception)
            {
                return new List<Dictionary<string, object>>();
            }
        }
        public async Task<List<Dictionary<string, object>>> consultarImagenes()
        {
            try
            {
                return await Payloads.SelectFromDatabase(Startup.Conexion, selectImg);

            }
            catch (Exception)
            {
                return new List<Dictionary<string, object>>();
            }
        }



        public async Task<List<Dictionary<string,object>>> validarUsuario(Login login)
        {

            try
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("@correo", login.Correo);
                keys.Add("@contra", login.Contrasena);

                return await Payloads.SelectFromDatabase(Startup.Conexion, selectLogin, keys);
            }
            catch (Exception)
            {

                return new List<Dictionary<string, object>>();
            }
           


        }
        public void Dispose()
        {
            Payloads = null;
        }
    }
}
