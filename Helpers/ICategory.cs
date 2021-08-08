using System.Collections.Generic;
using ApplicationDevelopmentCourseProject.Models;

namespace ApplicationDevelopmentCourseProject.Helpers
{
    public interface ICategory
    {
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        void NewCategory(Category category);
        void EditCategory(Category category);
        void DeleteCategory(int id);
    }
}