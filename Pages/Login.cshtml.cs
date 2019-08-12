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

namespace wasco_q2.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnGet(){
            Claim m_user_id = User.FindFirst("m_user_id");
            if(m_user_id != null)
            {
                return LocalRedirect("/");
            }
            return null;
        }
        public JsonResult OnPost () {
            login_db _context = new login_db (AppGlobal.get_db_option ()); //simplifying context initializer by override
            AppResponseMessage arm = new AppResponseMessage (); //inisialisasi ARM sebagai standarisasi respon balik

            try {
                using (var transaction = _context.Database.BeginTransaction ()) { 
                    if (Request.Query["f"] == "login_handler") {
                        string email = Request.Form["email"];
                        string password = Request.Form["password"];
                        var m_user = _context.m_user.FirstOrDefault(e => e.email == email && e.password == password);
                        if (m_user == null) { 
                            arm.fail();
                            arm.message = "Data Not Exist!!";
                        } else {

                            t_login_history login_history = new t_login_history{
                                m_user_id = m_user.m_user_id,
                                login_time = DateTime.Now
                            };

                            _context.t_login_history.Add(login_history);
                            _context.SaveChanges();

                            //jika user valid & aktif
                            var claims = new[] {
                                new Claim(ClaimTypes.Email, email),

                                new Claim("m_user_id", m_user.m_user_id.ToString()),
                                new Claim("full_name", m_user.full_name),
                                new Claim("history_id", login_history.t_login_history_id.ToString()),
                            };
                            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                            AuthenticationHttpContextExtensions.SignInAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            arm.success();
                            arm.message = "Success";
                        }
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
