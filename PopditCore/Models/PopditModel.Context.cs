﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PopditCore.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PopditDBEntities : DbContext
    {
        public PopditDBEntities()
            : base("name=PopditDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Bubble> Bubbles { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Filter> Filters { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Radius> Radii { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
