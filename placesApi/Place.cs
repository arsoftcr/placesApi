using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace placesApi
{
    public class Place
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public string linkMaps { get; set; }

    }

    public class PlaceLogin
    {
        public string Login { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public string linkMaps { get; set; }
    }
}
