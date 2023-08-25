using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Managers.Categories
{
    public interface ICatregory
    {
        ResponseApi CreateCategoty(CategoryMV category);
        ResponseApi GetAllCategories();
        ResponseApi GetCategoryById(int categortId);
        ResponseApi UpdateCategoryById(int categortId, CategoryMV category);
        ResponseApi DeleteCategoryById(int categortId);


    }
}
