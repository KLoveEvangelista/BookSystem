using BookSys.DAL.Models;
using BookSys.VeiwModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookSys.BLL.Helpers
{
    public class ToModel
    {
        public Book Book (BookVM bookVM)
        {
            return new Book
            {
                ID = bookVM.ID,
                MyGuid = bookVM.MyGuid,
                Title = bookVM.Title,
                Copyright = bookVM.Copyright,
                GenreID = bookVM.GenreID
            };
        }

        public Genre Genre (GenreVM genreVM)
        {
            return new Genre
            {
                ID = genreVM.ID,
                MyGuid = genreVM.MyGuid,
                Name = genreVM.Name
        
            };
        }



    }
}
