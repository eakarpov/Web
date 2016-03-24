﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GraphLabs.Dal.Ef
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using GraphLabs.DomainModel;
    
    public partial class GraphLabsContext : DbContext
    {
        public GraphLabsContext()
            : base("name=GraphLabsContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<StudentAction> StudentActions { get; set; }
        public DbSet<LabWork> LabWorks { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<LabVariant> LabVariants { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<AnswerVariant> AnswerVariants { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<TaskVariant> TaskVariants { get; set; }
        public DbSet<LabEntry> LabEntries { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}