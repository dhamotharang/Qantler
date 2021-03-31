using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.API.Services;
using Identity.Model;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PersonController : ControllerBase
  {
    readonly IPersonService _personservice;
    public PersonController(IPersonService personService)
    {
      _personservice = personService;
    }

    [HttpGet]
    [Route("find")]
    public async Task<Person> Get(string altID)
    {
      return await _personservice.GetPersonByAltID(altID);
    }

    [HttpPost]
    [Route("")]
    public async Task<Person> Post([FromBody] Person person)
    {
      return await _personservice.InsertPerson(person);
    }

    [HttpPut]
    [Route("")]
    public async Task<Person> Person([FromBody] Person person)
    {
      return await _personservice.UpdatePerson(person);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Person> Get(Guid id)
    {
      return await _personservice.GetPersonByID(id);
    }
  }
}
