using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TrackingSystemAPI.Models.Entities;

namespace TrackingSystemAPI.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Machine> Machines { get; set; }

        public DbSet<MachineTag> MachineTags { get; set; }

        public DbSet<TagData> TagDatas { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Alarm> Alarms { get; set; }

        public DbSet<AlarmTarget> AlarmTargets { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventTarget> EventTargets { get; set; }

        public DbSet<AlarmLog> AlarmLogs { get; set; }

        public DbSet<EventLog> EventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tag>().ToTable("tags");
            modelBuilder.Entity<Machine>().ToTable("machines");
            modelBuilder.Entity<MachineTag>().ToTable("machine_tags");
            modelBuilder.Entity<TagData>().ToTable("tag_datas");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Image>().ToTable("images");
            modelBuilder.Entity<Alarm>().ToTable("alarms");
            modelBuilder.Entity<AlarmLog>().ToTable("alarm_logs");
            modelBuilder.Entity<AlarmTarget>().ToTable("alarm_targets");
            modelBuilder.Entity<Event>().ToTable("events");
            modelBuilder.Entity<EventTarget>().ToTable("event_targets");
            modelBuilder.Entity<EventLog>().ToTable("event_logs");


            //many-to many ilişki 
            modelBuilder.Entity<MachineTag>()
                .HasKey(mt => new { mt.tag_id, mt.machine_id });

            modelBuilder.Entity<MachineTag>()
                .HasOne(mt => mt.Tag)
                .WithMany(t => t.MachineTags)
                .HasForeignKey(mt => mt.tag_id);

            modelBuilder.Entity<MachineTag>()
                .HasOne(mt => mt.Machine)
                .WithMany(m => m.MachineTags)
                .HasForeignKey(mt => mt.machine_id);

            modelBuilder.Entity<TagData>()
            .HasOne(td => td.Machine)
            .WithMany(m => m.TagDatas)
            .HasForeignKey(td => td.machine_id);

            modelBuilder.Entity<TagData>()
                .HasOne(td => td.Tag)
                .WithMany(t => t.TagDatas)
                .HasForeignKey(td => td.tag_id);

            //one to many

            modelBuilder.Entity<Image>()
                .HasOne(u => u.User)
                .WithMany(i => i.Images)
                .HasForeignKey(u => u.user_id);

            
            modelBuilder.Entity<Alarm>()
                .HasOne(u => u.User)
                .WithMany(a => a.Alarms)
                .HasForeignKey(u => u.user_id);

            //

            modelBuilder.Entity<Alarm>()
                .HasKey(a => a.alarm_id);

            modelBuilder.Entity<Alarm>()
                .HasMany(a => a.AlarmTargets)
                .WithOne(t => t.Alarms)
                .HasForeignKey(t => t.alarm_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AlarmTarget>()
                .HasKey(t => t.alarm_target_id);

            modelBuilder.Entity<AlarmTarget>()
                .HasOne(m => m.Machine)
                .WithMany(a => a.AlarmTargets)
                .HasForeignKey(m => m.machine_id);


            //
            modelBuilder.Entity<Event>()
                .HasOne(u => u.User)
                .WithMany(e => e.Events)
                .HasForeignKey(u => u.user_id);

            modelBuilder.Entity<Event>()
                .HasKey(e => e.event_id);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventTargets)
                .WithOne(t => t.Events)
                .HasForeignKey(t => t.event_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventTarget>()
                .HasKey(t => t.event_target_id);

            modelBuilder.Entity<EventTarget>()
                .HasOne(m => m.Machine)
                .WithMany(e => e.EventTargets)
                .HasForeignKey(m => m.machine_id);

            modelBuilder.Entity<AlarmLog>()
                .HasOne(a => a.Alarm)
                .WithMany(al => al.AlarmLogs)
                .HasForeignKey(a => a.alarm_id);

            modelBuilder.Entity<EventLog>()
                .HasOne(e => e.Event)
                .WithMany(el => el.EventLogs)
                .HasForeignKey(e => e.event_id);

        }
    }


}

