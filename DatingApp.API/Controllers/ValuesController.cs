using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{   // after we crate the database we have to come here to  retrive it. 

    // using the two methods below api get and post and delete perphase.

    [Authorize] // to authenticate our controller
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
         private readonly DataContext _context; // we careate this cz we need to use it in the class later

        public ValuesController(DataContext context)

        {
            _context = context;

        }
        // GET api/values
        [HttpGet]
// we can use async code becuas the below function can block the code until the query is finiseh, 
// but with async the code is excuting and more http request are done at the same time.
        public async Task<IActionResult> GetValuesAsync () // Iactions allows to retun http request as on OK
        {
     
            var values = await _context.Values.ToListAsync();  // take the vlases and make them list. cz we need them all so 
            // as list
            return Ok (values);  // retun values from the list as OK
        }

        // GET api/values/5
        // in of this case we make it as allowanonmucs to get indvi values.
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValueAsync(int id)  // this retun only one vlaue
        {
        var  value =await _context.Values.FirstOrDefaultAsync(x => x.Id== id); // forst or defaul will retun value but of not then will not give exception
        return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
