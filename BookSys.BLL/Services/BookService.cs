using BookSys.BLL.Contacts;
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
                        context.Books.Add(toModel.Book(bookVM));
                        context.SaveChanges();

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
                    var books = context.Books
                        .Include(x => x.Genre)
                        .ToList().OrderByDescending(x => x.ID);
                    var booksVm = books.Select(x => toViewModel.Book(x));
                        return booksVm;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public BookVM GetSingleBy(long id)
        {
            using (context)
            {
                try
                {
                    // SELECT * FROM books WHERE ID = 'id'
                    var book = context.Books
                        .Include(x => x.Genre)
                        .Where(x => x.ID == id)
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
    }
}
