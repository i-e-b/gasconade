using System;
using System.Collections.Generic;
using System.Web.Http;
using Gasconade;
using SampleWebApi.LogMessages;

namespace SampleWebApi.Controllers
{
    [RoutePrefix("values")]
    public class ValuesController : ApiController
    {
        [Route("")]
        public IEnumerable<string> Get()
        {
            Log.Info(new SampleMessage{ Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
            return new[] { "value1", "value2" };
        }
    }
}
