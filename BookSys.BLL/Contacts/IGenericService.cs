﻿using BookSys.VeiwModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookSys.BLL.Contacts
{
    public interface IGenericService<TVM, TType> where TVM : class where TType : IConvertible
    {
        IEnumerable<TVM> GetAll();
        TVM GetSingleBy(TType id);
        ResponseVM Create(TVM entity);
        ResponseVM Delete(TType guid);
        ResponseVM Update(TVM entity);
    }
}
