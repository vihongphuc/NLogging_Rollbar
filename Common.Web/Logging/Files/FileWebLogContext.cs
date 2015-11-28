using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Logging.Files
{
    public class FileWebLogContext : WebLogContext
    {
        public FileWebLogContext(HttpRequest request, HttpResponse response)
            : base(request, response)
        {
        }

        public string GetPath()
        {
            if (!String.IsNullOrWhiteSpace(Controller))
            {
                if (!String.IsNullOrWhiteSpace(Action))
                {
                    return Path.Combine(Controller, Action);
                }
            }

            return null;
        }
    }
}
