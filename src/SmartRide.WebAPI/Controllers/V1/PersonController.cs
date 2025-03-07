using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRide.WebAPI.Controllers.Attributes;
using SmartRide.WebAPI.Controllers.Base;

namespace SmartRide.WebAPI.Controllers.V1;

[Area("v1")]
[Pluralize] // person -> people
public class PersonController : BaseController
{
    // GET: api/v1/person
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }
    // GET api/v1/person/<id>
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }
    // POST api/v1/person
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }
    // PUT api/v1/person/<id>
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }
    // DELETE api/v1/person/<id>
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
