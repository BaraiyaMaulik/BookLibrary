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
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private  ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //This method is moved to UnitOfWork Because its will be used global
        //void ICategoryRepository.Save()
        //{
        //    _db.SaveChanges();
        //}

        public void Update(ProductImage obj)
        {
            _db.ProductImages.Update(obj);
        }
    }
}
