using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appglobal
{
  public class AppGlobal
  {
    internal static string BASE_URL = "";
    internal static string MYSQL_CS = "Server=127.0.0.1;Port=50000;Database=login_db;Uid=root;Pwd=GL-System123";

    /// <summary>
    /// Used in defining which connection to be used in a db_context
    /// </summary>
    /// <returns></returns>
    public static dynamic get_db_option()
    {
      DbContextOptionsBuilder ob = new DbContextOptionsBuilder<login_db>();
      ob.UseMySql(MYSQL_CS);
      return ob.Options;
    }

    /// <summary>
    /// Get primary working directory for application path searching
    /// </summary>
    /// <returns></returns>
    public static string get_working_directory()
    {
      return BASE_URL; //Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }
  }
}
