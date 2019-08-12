using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appglobal
{
  public class AppResponseMessage
  {
    public bool status { set; get; }
    public string remark { set; get; }
    public object data { set; get; }
    public string message { get; set; }

    /// <summary>
    /// Set success response
    /// </summary>
    public void success()
    {
      status = true;
      remark = "success";
    }

    /// <summary>
    /// Set failed response
    /// </summary>
    public void fail()
    {
      status = false;
      remark = "failed";
    }
  }
}
