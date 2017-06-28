using System;
using Microsoft.EntityFrameworkCore;
using GwhrSettings.Core;

namespace GwhrSettings.Providers
{
    public class GwhrDbContext : DbContext
    {
        #region Properties

        public static string ConnectionString { get; set; }
        public static DatabaseProvider DatabaseProvider { get; set; }

		#endregion

		#region DbSets

        public virtual DbSet<GwhrSetting> GwhrSettings { get; set; }

		#endregion

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=tcp:talentos.database.windows.net,1433;Initial Catalog=peopleDev;Persist Security Info=False;User ID=developer;Password=MyPeople!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
		}
    }
}
