using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Kontur.GameStats.Data;

namespace Kontur.GameStats.Application.Controllers
{
    class BaseController
    {
        //Плохой подход
        protected ConturDataModel dbcontext = new ConturDataModel();
   
    }
}