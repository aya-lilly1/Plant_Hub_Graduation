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
        Task<ResponseApi> CreateCategoty(CreateCategoryMV category);
        ResponseApi GetAllCategories(int langId);
        ResponseApi GetCategoryById(int categortId, int langId);
        ResponseApi SearchForCategory(String categoryName, int langId);
        ResponseApi UpdateCategoryById( CategoryMV category);
        ResponseApi DeleteCategoryById(int categortId);


    }
}
