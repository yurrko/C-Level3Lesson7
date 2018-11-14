using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MHser.DAL.Contexts;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using MHser.Logger;

namespace MHser.Services
{
    public class CategoriesData : ICategoriesData
    {
        private readonly MyLogger _logger = MyLogger.GetLogger();
        public IEnumerable<Category> GetCategories()
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Categories.ToList();
                }
            }
            catch ( Exception ex)
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public Category GetCategoryById( int id )
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Categories.FirstOrDefault( c => c.CategoryId == id );
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog("Error", ex);
                throw;
            }
        }

        public Category AddCategory( Category category )
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    context.Categories.Add( category );
                    context.SaveChanges();
                    return category;
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public void ModifyCategory( Category category )
        {
            try
            {
                using (var context = new MessoyahaContext())
                {
                    context.Categories.Attach(category);
                    context.Entry(category).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog("Error", ex);
                throw;
            }
        }

        public void DeleteCategory( int id )
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    var categoryToDelete = GetCategoryById( id );

                    if ( categoryToDelete is null ) return;

                    context.Categories.Attach( categoryToDelete );
                    context.Entry( categoryToDelete ).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog("Error", ex);
                throw;
            }
        }
    }
}