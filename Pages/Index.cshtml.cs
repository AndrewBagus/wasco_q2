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
    public class IndexModel : PageModel
    {
        public class history_user{
            public string full_name { get; set; }
            public string login_time { get; set; }
            public string logout_time { get; set; }
        }
        public void OnGet()
        {

        }
        public static List<m_user> get_user_list()
        {
            login_db _context = new login_db (AppGlobal.get_db_option ());
            var data = _context.m_user.ToList();
            return data;
        }

        public static List<history_user> get_history(int m_user_id)
        {
            login_db _context = new login_db (AppGlobal.get_db_option ());
            var data = (from h in _context.t_login_history
                        join s in _context.m_user on h.m_user_id equals s.m_user_id
                        where h.m_user_id == m_user_id
                        select new history_user{
                            full_name = s.full_name,
                            login_time = Convert.ToDateTime(h.login_time).ToString("dd/mm/yyyy HH:mm"),
                            logout_time = Convert.ToDateTime(h.logout_time).ToString("dd/mm/yyyy HH:mm")
                        }).ToList();
          
            return data;
        }

        public static List<history_user> get_history_all()
        {
            login_db _context = new login_db (AppGlobal.get_db_option ());
            var data = (from h in _context.t_login_history
                        join s in _context.m_user on h.m_user_id equals s.m_user_id
                        select new history_user{
                            full_name = s.full_name,
                            login_time = Convert.ToDateTime(h.login_time).ToString("dd/mm/yyyy HH:mm"),
                            logout_time = Convert.ToDateTime(h.logout_time).ToString("dd/mm/yyyy HH:mm")
                        }).ToList();
          
            return data;
        }
    }
}
