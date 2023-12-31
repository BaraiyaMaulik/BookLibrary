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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //This method is moved to UnitOfWork Because its will be used global
        //void ICategoryRepository.Save()
        //{
        //    _db.SaveChanges();
        //}

        void IProductRepository.Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Author = obj.Author;
                objFromDb.Price = obj.Price;
                objFromDb.Price50= obj.Price50;
                objFromDb.Price100 = obj.Price100;
                objFromDb.ListPrice= obj.ListPrice;
                objFromDb.CategoryId=obj.CategoryId;
                objFromDb.ProductImages = obj.ProductImages;
                //if(obj.ImageUrl != null)
                //{
                //    objFromDb.ImageUrl = obj.ImageUrl;  
                //}
            }              
            //_db.Products.Update(obj);
        }
    }
}
