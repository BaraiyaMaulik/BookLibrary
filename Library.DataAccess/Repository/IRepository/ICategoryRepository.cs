using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
     void Update(Category obj);
    
    //This class is move to IUnitOfWork beacause its a global method
     //void Save();   
    }
}
