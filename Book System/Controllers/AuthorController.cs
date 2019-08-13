using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookSys.BLL.Contracts;
using BookSys.BLL.Services;
using BookSys.VeiwModel.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IGenericService<AuthorVM, long> authorService;
        public AuthorController(AuthorService _authorService)
        {
            authorService = _authorService;
        }

        //api/Author/Create
        [HttpPost("[action]")]
        public ActionResult<ResponseVM> Create([FromBody]AuthorVM authorVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something went wrong");
            }
            return authorService.Create(authorVM);
        }

        //api/Author/Delete/id
        [HttpDelete("[action]/{id}")]
        public ActionResult<ResponseVM> Delete (long id)
        {
            return authorService.Delete(id);
        }

        //api/Author/GetAll
        [HttpGet("[action]")]
        public IEnumerable<AuthorVM> GetAll()
        {
            return authorService.GetAll();
        }
        
        //api/Author/getSingleBy/id
        [HttpGet("[action]/{guid}")]
        public AuthorVM GetSingleBy(string guid)
        {
            return authorService.GetSingleBy(guid);

        }

        //api/Author/Update
        [HttpPut("[action]")]
        public ActionResult<ResponseVM> Update ([FromBody]AuthorVM authorVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Somehing went wrong");
            return authorService.Update(authorVM);
        }
    }
} 