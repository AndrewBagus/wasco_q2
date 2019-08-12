using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace appglobal
{	
	public class m_user
	{
		public int m_user_id { get; set; }
		public string full_name { get; set; }
		public string email { get; set; }
		public string password { get; set; }
        public List<t_login_history> t_login_history { get; set; }
	}
    public class t_login_history
    {
        public int t_login_history_id { get; set; }
		public int m_user_id { get; set; }
        public System.DateTime login_time { get; set; }
        public System.DateTime logout_time { get; set; }
        public m_user m_user { get; set; }
    }
    
    public class login_db : DbContext
    {
        public DbSet<m_user> m_user {get; set;}
        public DbSet<t_login_history> t_login_history {get; set;}

        public login_db(DbContextOptions<login_db> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder opt)
        {
            #region m_user
            opt.Entity<m_user>()
               .HasKey(e => e.m_user_id);

            opt.Entity<m_user>()
               .HasMany(e => e.t_login_history)
               .WithOne(e => e.m_user)
               .HasForeignKey(e => e.m_user_id)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);
            
			#endregion

			#region t_login_history
            opt.Entity<t_login_history>()
               .HasKey(e => e.t_login_history_id);
			#endregion
        }
        
    }
}