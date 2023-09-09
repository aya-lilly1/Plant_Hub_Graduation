using Microsoft.AspNetCore.Mvc;
using Plant_Hub.ModelRepo;
using Plant_Hub.ModelServices;
using Plant_Hub_Core.Helper;

namespace Plant_Hub.Controllers
{
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IModelRepos _modelRepos;

        public ModelController(IHostEnvironment hostEnvironment, IModelRepos modelRepos)
        {
           _hostEnvironment = hostEnvironment;
            _modelRepos = modelRepos;
        }
        
       
    }
}
