using System.Collections.Generic;
using ApplicationDevelopmentCourseProject.Models;

namespace ApplicationDevelopmentCourseProject.Helpers
{
    public interface IProduct
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetPreferred(int count);
        IEnumerable<Product> GetProductsByCategoryId(int categoryId);
        IEnumerable<Product> GetFilteredProducts(int id, string searchQuery);
        IEnumerable<Product> GetFilteredProducts(string searchQuery);
        Product GetById(int id);
        void NewProduct(Product Product);
        void EditProduct(Product Product);
        void DeleteProduct(int id);
    }
}
