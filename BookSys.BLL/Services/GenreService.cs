

using BookSys.BLL.Contracts;
using BookSys.BLL.Helpers;
using BookSys.DAL.Models;
using BookSys.VeiwModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookSys.BLL.Services
{
    public class GenreService : IGenericService<GenreVM, long>
    {
        private ToViewModel toViewModel = new ToViewModel();
        private ToModel toModel = new ToModel();
        private readonly BookSysContext context;


        public GenreService(BookSysContext _context)
        {
            context = _context;
        }

        public ResponseVM Create(GenreVM genreVM)
        {
            using (context)
            {
                using (var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        genreVM.MyGuid = Guid.NewGuid();
                        context.Genres.Add(toModel.Genre(genreVM));
                        context.SaveChanges();

                        //commit changes to db
                        dbTransaction.Commit();
                        return new ResponseVM
                            ("create", true, "Genre");

                    }
                    catch (Exception ex)
                    {
                        //rollback
                        dbTransaction.Rollback();
                        return new ResponseVM("create", false, "Genre", ResponseVM.SOMTHING_WENT_WRONG, "", ex);
                    }
                }
            }
        }
        public ResponseVM Delete(long id)
        {
            using (context)
            {
                using (var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Genre genreTobeDeleted = context.Genres.Find(id);
                        if (genreTobeDeleted == null)
                            return new ResponseVM("deleted", false, "Genres", ResponseVM.DOES_NOT_EXIST);
                        //delete

                        context.Genres.Remove(genreTobeDeleted);
                        context.SaveChanges();

                        dbTransaction.Commit();
                        return new ResponseVM
                            ("deleted", true, "Genre");
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ResponseVM
                            ("deleted", false, "Genre", ResponseVM.SOMTHING_WENT_WRONG, "", ex);
                    }
                }
            }
        }

        public IEnumerable<GenreVM> GetAll()
        {
            using (context)
            {
                try
                {
                    //SELECT * FROM BOOKS ORDER BY ID DESC 
                    var genres = context.Genres.ToList().OrderByDescending(x => x.ID);
                    var genresVm = genres.Select(x => toViewModel.Genre(x));
                    return genresVm;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public GenreVM GetSingleBy(string guid)
        {
            using (context)
            {
                try
                {
                    // SELECT * FROM books WHERE ID = 'id'
                    var genre = context.Genres.Where(x => x.MyGuid.ToString() == guid).FirstOrDefault();
                    GenreVM genreVM = null;
                    if (genre != null)
                        genreVM = toViewModel.Genre(genre);
                    return genreVM;

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public ResponseVM Update(GenreVM genreVM)
        {
            using (context)
            {
                using (var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //find book from database
                        Genre genreTobeUpdated = context.Genres.Find(genreVM.ID);
                        if (genreTobeUpdated == null)
                            return new ResponseVM("update", false, "Book", ResponseVM.DOES_NOT_EXIST);
                        //update changes
                        genreTobeUpdated.Name = genreVM.Name;
                        context.SaveChanges();
                        dbTransaction.Commit();
                        return new ResponseVM("updated", true, "Genre");
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ResponseVM
                            ("updated", false, "Genre", ResponseVM.SOMTHING_WENT_WRONG, "", ex);

                        throw;
                    }
                }
            }
        }
        public PagingResponse<GenreVM> GetDataServerSide(PagingRequest paging)
        {
            using (context)
            {

                var pagingResponse = new PagingResponse<GenreVM>()
                {
                    // counts how many times the user draws data
                    Draw = paging.Draw
                };
                // initialized query
                IEnumerable<Genre> query = null;
                // search if user provided a search value, i.e. search value is not empty
                if (!string.IsNullOrEmpty(paging.Search.Value))
                {
                    // search based from the search value
                    query = context.Genres.Where(v => v.Name.ToString().ToLower().Contains(paging.Search.Value.ToString().ToLower()));
                }
                else
                {
                    // selects all from table
                    query = context.Genres;
                }
                // total records from query
                var recordsTotal = query.Count();
                // orders the data by the sorting selected by the user
                // used ternary operator to determine if ascending or descending
                var colOrder = paging.Order[0];
                switch (colOrder.Column)
                {
                    case 0:
                        query = colOrder.Dir == "asc" ? query.OrderBy(v => v.Name) : query.OrderByDescending(v => v.Name);
                        break;
                }

                var taken = query.Skip(paging.Start).Take(paging.Length).ToArray();
                // converts model(query) into viewmodel then assigns it to response which is displayed as "data"
                pagingResponse.Reponse = taken.Select(x => toViewModel.Genre(x));
                pagingResponse.RecordsTotal = recordsTotal;
                pagingResponse.RecordsFiltered = recordsTotal;

                return pagingResponse;
            }
        }
    }
}
