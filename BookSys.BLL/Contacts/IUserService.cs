using BookSys.VeiwModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookSys.BLL.Contacts
{
    public interface IUserService<TVM, TType> where TVM : class where TType:IConvertible
    {
        Task<IEnumerable<TVM>> GetAll();
        Task<TVM> GetSingleBy(TType id);
        Task<ResponseVM> Register(TVM entity);
        Task<ResponseVM> Delete(TType guid);
        Task<ResponseVM> Update(TVM entity);
        Task<ResponseVM> Deactivate(TVM entity);
        Task<ResponseVM> Login(TVM entity);
        Task<ResponseVM> Validate(TVM entity);
    }
}
