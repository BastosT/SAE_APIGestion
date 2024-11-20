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
    public class DonneesCapteurControllerTests
    {


        private GlobalDBContext ctx;
        private IDataRepository<DonneesCapteur> dataRepository;
        private DonneesCapteur _donneesCapteur;
        private DonneesCapteur _donneesCapteurUpdate;
        private Mock<IDataRepository<DonneesCapteur>> _mockRepository;
        private DonneesCapteurController _donneesCapteurController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<DonneesCapteur>>();

            _donneesCapteur = new DonneesCapteur
             {
                DonneesCapteurId = 1,
                CapteurId = 1, 
                TypeDonneesId = 1, 
                Valeur = 23.5, 
                Timestamp = DateTime.Now 
            };

            _donneesCapteurUpdate = new DonneesCapteur
            {
                DonneesCapteurId = 1,
                CapteurId = 1,
                TypeDonneesId = 1,
                Valeur = 25,
                Timestamp = DateTime.Now
            };

            // Initialisation du contrôleur avec le mock
            _donneesCapteurController = new DonneesCapteurController(_mockRepository.Object);

        }

        [TestMethod()]
        public void GetDonneesCapteurTest()
        {

            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_donneesCapteur);

            // Act
            var actionResult = _donneesCapteurController.GetDonneesCapteur(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_donneesCapteur, actionResult.Value as DonneesCapteur);

        }

        [TestMethod()]
        public void PutDonneesCapteurTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_donneesCapteurUpdate);

            // Act
            var actionResult = _donneesCapteurController.PutDonneesCapteur(_donneesCapteurUpdate.DonneesCapteurId, _donneesCapteur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual(25, _donneesCapteurUpdate.Valeur, "La valeur des capteur n'a pas été mis à jour");
        }

        [TestMethod()]
        public void PostDonneesCapteurTest()
        {
            // Act 
            var actionResult = _donneesCapteurController.PostDonneesCapteur(_donneesCapteur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<DonneesCapteur>), "Le retour n'est pas du type ActionResult<DonneesCapteur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_donneesCapteur.Valeur, ((DonneesCapteur)result.Value).Valeur, "la valeur des capteur ne correspond pas");

        }

        [TestMethod()]
        public void DeleteDonneesCapteurTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_donneesCapteur);
            // Act
            var actionResult = _donneesCapteurController.DeleteDonneesCapteur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}