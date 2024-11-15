using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TypeEquipementController : ControllerBase
    {
        private readonly IDataRepository<TypeEquipement> dataRepository;

        public TypeEquipementController(IDataRepository<TypeEquipement> dataRepo)
        {
            dataRepository = dataRepo;
        }


        // GET: api/TypeEquipements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeEquipement>>> GetTypeEquipements()
        {
            return await dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypeEquipement>> GetTypeEquipement(int id)
        {
            var typeEquipement = await dataRepository.GetByIdAsync(id);

            if (typeEquipement == null)
            {
                return NotFound();
            }

            return typeEquipement;
        }






    }
}
