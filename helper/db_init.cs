using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using hkti_api.models.hkti;

namespace appglobal
{
  /// <summary>
  /// Main class for initiating database content on .netcore
  /// </summary>
  public static class DbInitializer
  {

    /// <summary>
    /// Main method to populate database content
    /// </summary>
    public static void Initialize()
    {
      using (var _context = new login_db(AppGlobal.get_db_option()))
      {
        //run primary migration method
        _context.Database.Migrate();
      }

    }
  }
}