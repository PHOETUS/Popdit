﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PopditDB.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bubble> Bubbles { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Radius> Radii { get; set; }
        public virtual DbSet<LogEvent> LogEvents { get; set; }
    }
}
