using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using appglobal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wasco_q2.Pages {
    public class LogoutModel : PageModel {
        public JsonResult OnPost () {
            login_db _context = new login_db (AppGlobal.get_db_option ()); //simplifying context initializer by override
            AppResponseMessage arm = new AppResponseMessage (); //inisialisasi ARM sebagai standarisasi respon balik

            try {
                using (var transaction = _context.Database.BeginTransaction ()) { 
                    if (Request.Query["f"] == "logout_handler") {
                        var history_id = User.FindFirst("history_id").Value;

                        var history = _context.t_login_history.FirstOrDefault(e => e.t_login_history_id == Convert.ToInt32(history_id));

                        history.logout_time = DateTime.Now;
                        _context.t_login_history.Update(history);
                        _context.SaveChanges();

                        AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme);          

                        arm.success();
                        arm.message = "Success";
                    }
                    transaction.Commit();
                    _context.SaveChanges();
                }
            } catch (Exception ex) {
                arm.fail ();
                arm.message = ex.Message;
            }
            return new JsonResult (arm); //return ARM dg method JsonResult untuk auto serialize ke format JSON
        }
    }
}