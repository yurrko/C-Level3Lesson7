using MHser.Domain.Entities;
using System.Collections.Generic;

namespace MHser.Domain.Interfaces
{
    public interface ICategoriesData
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryById( int id );
        Category AddCategory( Category category );
        void ModifyCategory( Category category );
        void DeleteCategory( int id );
    }
}
