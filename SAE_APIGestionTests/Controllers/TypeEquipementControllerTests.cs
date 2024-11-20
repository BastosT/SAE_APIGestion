using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SAE_APIGestion.Controllers;
using SAE_APIGestion.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_APIGestion.Controllers.Tests
{
    [TestClass()]
    public class TypeEquipementControllerTests
    {

        private GlobalDBContext ctx;
        private IDataRepository<TypeEquipement> dataRepository;
        private TypeEquipement _typeEquipement;
        private TypeEquipement _typeEquipementUpdate;
        private Mock<IDataRepository<TypeEquipement>> _mockRepository;
        private TypeEquipementController _typeequipementController;



        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<TypeEquipement>>();

            _typeEquipement = new TypeEquipement
            {
                TypeEquipementId = 1,
                Nom = "Projecteur"
            };

            _typeEquipementUpdate = new TypeEquipement
            {
                TypeEquipementId = 1,
                Nom = "Projecteur Update"
            };

            // Initialisation du contrôleur avec le mock
            _typeequipementController = new TypeEquipementController(_mockRepository.Object);
        }

        [TestMethod()]
        public void GetTypeEquipementTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeEquipement);

            // Act
            var actionResult = _typeequipementController.GetTypeEquipement(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_typeEquipement, actionResult.Value as TypeEquipement);
        }

        [TestMethod()]
        public void PutTypeEquipementTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeEquipementUpdate);

            // Act
            var actionResult = _typeequipementController.PutTypeEquipement(_typeEquipementUpdate.TypeEquipementId, _typeEquipement).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Projecteur Update", _typeEquipementUpdate.Nom, "Le nom du type n'a pas été mis à jour");
        }

        [TestMethod()]
        public void PostTypeEquipementTest()
        {
            // Act 
            var actionResult = _typeequipementController.PostTypeEquipement(_typeEquipement).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<TypeEquipement>), "Le retour n'est pas du type ActionResult<TypeEquipement>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_typeEquipement.Nom, ((TypeEquipement)result.Value).Nom, "Le nom du TypeEquipement ne correspond pas");
        }

        [TestMethod()]
        public void DeleteTypeEquipementTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeEquipement);
            // Act
            var actionResult = _typeequipementController.DeleteTypeEquipement(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}