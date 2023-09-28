using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class UrlShortenerDatabaseSettings
    {
        public string? CONNECTION_STRING { get; set; }
        public string? DATABASE_NAME { get; set; }
        public string? COLLECTION_NAME { get; set; }
    }
}
