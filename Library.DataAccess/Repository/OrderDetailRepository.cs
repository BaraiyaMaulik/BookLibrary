using Library.DataAccess.Data;
using Library.DataAccess.Repository.IRepository;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private  ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //This method is moved to UnitOfWork Because its will be used global
        //void ICategoryRepository.Save()
        //{
        //    _db.SaveChanges();
        //}

       public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
