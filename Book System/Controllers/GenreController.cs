using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookSys.BLL.Contacts;
using BookSys.BLL.Services;
using BookSys.VeiwModel.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly IGenericService<GenreVM, long> genreService;
        public GenreController(GenreService _genreService)
        {
            genreService = _genreService;
        }

        //api/Genre/Create
        [HttpPost("[action]")]
        public ActionResult<ResponseVM> Create([FromBody]GenreVM genreVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something went wrong");
            }
            return genreService.Create(genreVM);
        }

        //api/Genre/Delete/id
        [HttpDelete("[action]/{id}")]
        public ActionResult<ResponseVM> Delete (long id)
        {
            return genreService.Delete(id);
        }

        //api/Genre/GetAll
        [HttpGet("[action]")]
        public IEnumerable<GenreVM> GetAll()
        {
            return genreService.GetAll();
        }
        
        //api/Genre/getSingleBy/id
        [HttpGet("[action]/{id}")]
        public GenreVM GetSingleBy(long id)
        {
            return genreService.GetSingleBy(id);

        }

        //api/Genre/Update
        [HttpPut("[action]")]
        public ActionResult<ResponseVM> Update ([FromBody]GenreVM genreVM)
        {
            if (!ModelState.IsValid)
                return BadRequest("Somehing went wrong");
            return genreService.Update(genreVM);
        }
    }
} 