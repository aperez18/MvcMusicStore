using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MvcMusicStore.Models;

namespace MvcMusicStore.DataContexts
{
    public class MusicDb : DbContext
    {
        public MusicDb() : base("DefaultConnection") { }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public System.Data.Entity.DbSet<MvcMusicStore.Models.Artist> Artists { get; set; }
    }
}