using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;


namespace Kontur.GameStats.Application.Controllers
{
    class BaseController
    {
        protected App_Data.ConturDataModel ConturDbContext = new App_Data.ConturDataModel();
   
    }
}