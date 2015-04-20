using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        [DisplayName("Artist")]
        public string Name { get; set; }
    }
}