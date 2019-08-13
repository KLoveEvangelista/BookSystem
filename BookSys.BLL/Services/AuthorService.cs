

using BookSys.BLL.Contracts;
using BookSys.BLL.Helpers;
using BookSys.DAL.Models;
using BookSys.VeiwModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookSys.BLL.Services
{
    public class AuthorService : IGenericService<AuthorVM, long>
    {
        private ToViewModel toViewModel = new ToViewModel();
        private ToModel toModel = new ToModel();
        private readonly BookSysContext context;


        public AuthorService(BookSysContext _context)
        {
            context = _context;
        }

        public ResponseVM Create(AuthorVM authorVM)
        {
            using (context)
            {
                using (var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        authorVM.MyGuid = Guid.NewGuid();
                        context.Authors.Add(toModel.Author(authorVM));
                        context.SaveChanges();

                        //commit changes to db
                        dbTransaction.Commit();
                        return new ResponseVM
                            ("create", true, "Author");

                    }
                    catch (Exception ex)
                    {
                        //rollback
                        dbTransaction.Rollback();
                        return new ResponseVM("create", false, "Author", ResponseVM.SOMTHING_WENT_WRONG, "", ex);
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
                        Author authorTobeDeleted = context.Authors.Find(id);
                        if (authorTobeDeleted == null)
                            return new ResponseVM("deleted", false, "Authors", ResponseVM.DOES_NOT_EXIST);
                        //delete

                        context.Authors.Remove(authorTobeDeleted);
                        context.SaveChanges();

                        dbTransaction.Commit();
                        return new ResponseVM
                            ("deleted", true, "Author");
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ResponseVM
                            ("deleted", false, "Author", ResponseVM.SOMTHING_WENT_WRONG, "", ex);
                    }
                }
            }
        }

        public IEnumerable<AuthorVM> GetAll()
        {
            using (context)
            {
                try
                {
                    //SELECT * FROM BOOKS ORDER BY ID DESC 
                    var authors = context.Authors.ToList().OrderByDescending(x => x.ID);
                    var authorsVm = authors.Select(x => toViewModel.Author(x));
                    return authorsVm;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public AuthorVM GetSingleBy(string guid)
        {
            using (context)
            {
                try
                {
                    // SELECT * FROM books WHERE ID = 'id'
                    var author = context.Authors.Where(x => x.MyGuid.ToString() == guid).FirstOrDefault();
                    AuthorVM authorVM = null;
                    if (author != null)
                        authorVM = toViewModel.Author(author);
                    return authorVM;

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public ResponseVM Update(AuthorVM authorVM)
        {
            using (context)
            {
                using (var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //find book from database
                        Author authorTobeUpdated = context.Authors.Find(authorVM.ID);
                        if (authorTobeUpdated == null)
                            return new ResponseVM("update", false, "Book", ResponseVM.DOES_NOT_EXIST);
                        //update changes
                        authorTobeUpdated.FirstName = authorVM.FirstName;
                        authorTobeUpdated.MiddleName = authorVM.MiddleName;
                        authorTobeUpdated.LastName = authorVM.LastName;

                        context.SaveChanges();
                        dbTransaction.Commit();
                        return new ResponseVM("updated", true, "Author");
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ResponseVM
                            ("updated", false, "Author", ResponseVM.SOMTHING_WENT_WRONG, "", ex);

                        throw;
                    }
                }
            }
        }
        public PagingResponse<AuthorVM> GetDataServerSide(PagingRequest paging)
        {
            using (context)
            {

                var pagingResponse = new PagingResponse<AuthorVM>()
                {
                    // counts how many times the user draws data
                    Draw = paging.Draw
                };
                // initialized query
                IEnumerable<Author> query = null;
                // search if user provided a search value, i.e. search value is not empty
                if (!string.IsNullOrEmpty(paging.Search.Value))
                {
                    // search based from the search value
                    query = context.Authors.Where(v => v.FirstName.ToString().ToLower().Contains(paging.Search.Value.ToString().ToLower()));
                }
                else
                {
                    // selects all from table
                    query = context.Authors;
                }
                // total records from query
                var recordsTotal = query.Count();
                // orders the data by the sorting selected by the user
                // used ternary operator to determine if ascending or descending
                var colOrder = paging.Order[0];
                switch (colOrder.Column)
                {
                    case 0:
                        query = colOrder.Dir == "asc" ? query.OrderBy(v => v.FirstName) : query.OrderByDescending(v => v.FirstName);
                        break;
                }

                var taken = query.Skip(paging.Start).Take(paging.Length).ToArray();
                // converts model(query) into viewmodel then assigns it to response which is displayed as "data"
                pagingResponse.Reponse = taken.Select(x => toViewModel.Author(x));
                pagingResponse.RecordsTotal = recordsTotal;
                pagingResponse.RecordsFiltered = recordsTotal;

                return pagingResponse;
            }
        }
    }
}
