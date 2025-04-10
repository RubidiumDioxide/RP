using Microsoft.AspNetCore.Mvc;
using RP_API.Structures.Tree;
using System.Reflection;


namespace RP_API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AdPlatformController : Controller
    {
        // GET: api/Load/filename
        [HttpGet("Load")]
        public async Task<ActionResult<IEnumerable<string>>> LoadFromFile([FromQuery] string filename)
        {
            if (filename == null || filename.Contains(' ')) 
                return BadRequest("filename can't contain spaces");

            try
            {
                var txtDisplay = Functions.ReadFrom(filename); 

                Data.tree = Functions.LoadIntoTree(filename);
                if (Data.tree.Value == null)
                    return BadRequest("Couldn't load the data from file");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(Functions.ReadFrom(filename));
        }

        // GET: api/Search
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<string>>> Search([FromQuery] string location)
        {
            if (location == null || location == "" || location.Contains(' '))
                return BadRequest("filename can't contain spaces");
            try
            {
                if (Data.tree.Value != null)
                    return Ok(Functions.getPlatforms(Data.tree, location));
                else
                    return BadRequest("no data was loaded");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
