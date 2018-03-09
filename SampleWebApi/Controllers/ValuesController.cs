using System.Collections.Generic;
using System.Web.Http;

namespace SampleWebApi.Controllers
{
    [RoutePrefix("values")]
    public class ValuesController : ApiController
    {
        [Route("")]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        [Route("{id}")]
        public string Get(int id)
        {
            return "What, what?";
        }

        public void Post([FromBody]string value)
        {
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
