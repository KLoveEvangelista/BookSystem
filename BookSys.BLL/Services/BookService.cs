using BookSys.BLL.Contracts;
using BookSys.BLL.Helpers;
using BookSys.DAL.Models;
using BookSys.VeiwModel.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookSys.BLL.Services
{
    public class BookService : IGenericService<BookVM, long>
    {
        private ToViewModel toViewModel = new ToViewModel();
        private ToModel toModel = new ToModel();
        private readonly BookSysContext context;


        public BookService(BookSysContext _context)
        {
            context = _context;
        }

        public ResponseVM Create(BookVM bookVM)
        {
            using (context)
            {
                using (var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        bookVM.MyGuid = Guid.NewGuid();
                        var bookSaved = context.Books.Add(toModel.Book(bookVM)).Entity;
                        context.SaveChanges();

                        foreach(var authID in bookVM.AuthorIdList)
                        {
                            //validate existence of author
                            var author = context.Authors.Find(authID);
                            if (author == null)
                                return new ResponseVM("create", false, "Book", "Author does not exists");
                            var bookAuthor = new BookAuthor
                            {
                                AuthorID = authID,
                                BookID = bookSaved.ID,
                                AuthorFullName = toViewModel.ToFullName(author.FirstName, author.MiddleName, author.LastName)
                            };

                            context.BookAuthors.Add(bookAuthor);
                            context.SaveChanges();
                        }

                        //commit changes to db
                        dbTransaction.Commit();
                        return new ResponseVM
                            ("create", true, "Book");

                    }
                    catch (Exception ex)
                    {
                        //rollback
                        dbTransaction.Rollback();
                        return new ResponseVM("create", false, "Book", ResponseVM.SOMTHING_WENT_WRONG, "", ex);
                    }
                }
            }
        }
        public ResponseVM Delete (long id)
        {
            using(context)
            {
                using(var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Book bookTobeDeleted = context.Books.Find(id);
                        if (bookTobeDeleted == null)
                            return new ResponseVM ("deleted", false, "Book", ResponseVM.DOES_NOT_EXIST);
                        //delete

                        var removeFromBookAuthors = context.BookAuthors.Where(x => x.BookID == bookTobeDeleted.ID);
                        context.BookAuthors.RemoveRange(removeFromBookAuthors);
                        context.SaveChanges();

                        context.Books.Remove(bookTobeDeleted);
                        context.SaveChanges();

                        dbTransaction.Commit();
                        return new ResponseVM
                            ("deleted", true, "Book");
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ResponseVM
                            ("deleted", false, "Book", ResponseVM.SOMTHING_WENT_WRONG, "", ex);
                    }
                }
            }
        }

        public IEnumerable<BookVM> GetAll()
        {
            using (context)
            {
                try
                {
                    //SELECT * FROM BOOKS ORDER BY ID DESC 
                    //LinQ EagerLoading
                    var books = context.Books
                        .Include(x => x.Genre)
                        .Include(x => x.BookAuthors)
                            .ThenInclude(x => x.Author)
                        .ToList()
                        .OrderByDescending(x => x.ID);
                    var booksVm = books.Select(x => toViewModel.Book(x));
                        return booksVm;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public BookVM GetSingleBy(string guid)
        {
            using (context)
            {
                try
                {
                    // SELECT * FROM books WHERE ID = 'id'
                    var book = context.Books
                        .Include(x => x.Genre)
                        .Where(x => x.MyGuid.ToString() == guid)
                        .FirstOrDefault();
                    BookVM bookVM = null;
                    if (book != null)
                        bookVM = toViewModel.Book(book);
                    return bookVM;

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public ResponseVM Update(BookVM bookVM)
        {
            using (context)
            {
                using (var dbTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //find book from database
                        Book bookTobeUpdated = context.Books.Find(bookVM.ID);
                        if (bookTobeUpdated == null)
                            return new ResponseVM("update", false, "Book", ResponseVM.DOES_NOT_EXIST);
                        //update changes
                        bookTobeUpdated.Title = bookVM.Title;
                        bookTobeUpdated.Copyright = bookVM.Copyright;
                        bookTobeUpdated.GenreID = bookVM.GenreID;
                        context.SaveChanges();

                        //came from delete
                        var removeFromBookAuthors = context.BookAuthors.Where(x => x.BookID == bookTobeUpdated.ID);
                        context.BookAuthors.RemoveRange(removeFromBookAuthors);
                        context.SaveChanges();

                        //from create, saves to assoc table
                        foreach (var authID in bookVM.AuthorIdList)
                        {
                            //validate existence of author
                            var author = context.Authors.Find(authID);
                            if (author == null)
                                return new ResponseVM("create", false, "Book", "Author does not exists");
                            var bookAuthor = new BookAuthor
                            {
                                AuthorID = authID,
                                BookID = bookTobeUpdated.ID,
                                AuthorFullName = toViewModel.ToFullName(author.FirstName, author.MiddleName, author.LastName)
                            };

                            context.BookAuthors.Add(bookAuthor);
                            context.SaveChanges();
                        }

                        dbTransaction.Commit();
                        return new ResponseVM("updated", true, "Book");
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ResponseVM
                            ("updated", false, "Book", ResponseVM.SOMTHING_WENT_WRONG, "", ex);

                        throw;
                    }
                }
            }
        }
        public PagingResponse<BookVM> GetDataServerSide(PagingRequest paging)
        {
            using (context)
            {
                var pagingResponse = new PagingResponse<BookVM>()
                {
                    // counts how many times the user draws data
                    Draw = paging.Draw
                };
                // initialized query
                IEnumerable<Book> query = null;
                // search if user provided a search value, i.e. search value is not empty
                if (!string.IsNullOrEmpty(paging.Search.Value))
                {
                    // search based from the search value     
                    query = context.Books.Include(x => x.Genre)
                                         .Include(x => x.BookAuthors)
                                         .ThenInclude(x => x.Author)
                                         .Where(v => v.Title.ToString().ToLower().Contains(paging.Search.Value.ToLower()) ||
                                                     v.Copyright.ToString().ToLower().Contains(paging.Search.Value.ToLower()) ||
                                                     v.Genre.Name.ToString().ToLower().Contains(paging.Search.Value.ToLower()) ||
                                                     v.BookAuthors.Any(x => x.AuthorFullName.ToLower().Contains(paging.Search.Value.ToLower())));
                }
                else
                {
                    // selects all from table
                    query = context.Books
                                        .Include(x => x.Genre)
                                        .Include(x => x.BookAuthors)
                                        .ThenInclude(x => x.Author);
                }
                // total records from query
                var recordsTotal = query.Count();
                // orders the data by the sorting selected by the user
                // used ternary operator to determine if ascending or descending
                var colOrder = paging.Order[0];
                switch (colOrder.Column)
                {
                    case 0:
                        query = colOrder.Dir == "asc" ? query.OrderBy(v => v.Title) : query.OrderByDescending(v => v.Title);
                        break;
                    case 1:
                        query = colOrder.Dir == "asc" ? query.OrderBy(b => b.Copyright) : query.OrderByDescending(b => b.Copyright);
                        break;
                    case 2:
                        query = colOrder.Dir == "asc" ? query.OrderBy(b => b.Genre.Name) : query.OrderByDescending(b => b.Genre.Name);
                        break;
                }
                var taken = query.Skip(paging.Start).Take(paging.Length).ToArray();
                // converts model(query) into viewmodel then assigns it to response which is displayed as "data"
                pagingResponse.Reponse = taken.Select(x => toViewModel.Book(x));
                pagingResponse.RecordsTotal = recordsTotal;
                pagingResponse.RecordsFiltered = recordsTotal;

                return pagingResponse;
            }
        }
    }
}
