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
    public class CapteurControllerTests
    {

        private GlobalDBContext ctx;
        private IDataRepository<Capteur> dataRepository;
        private Capteur _capteur;
        private Capteur _capteurUpdate;
        private Mock<IDataRepository<Capteur>> _mockRepository;
        private CapteurController _capteurController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<Capteur>>();

            _capteur= new Capteur
             {
                CapteurId = 1,
                EstActif = true,
                DistanceFenetre = 3.5,
                DistancePorte = 1.0,
                DistanceChauffage = 2.0,
                SalleId = 1,
                MurId = 1,
             };

            _capteurUpdate = new Capteur
            {
                CapteurId = 1,
                EstActif = false,
                DistanceFenetre = 3.5,
                DistancePorte = 1.0,
                DistanceChauffage = 2.0,
                SalleId = 1,
                MurId = 1,
            };

            // Initialisation du contrôleur avec le mock
            _capteurController = new CapteurController(_mockRepository.Object);
        }

        [TestMethod()]
        public void GetCapteurTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_capteur);

            // Act
            var actionResult = _capteurController.GetCapteur(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_capteur, actionResult.Value as Capteur);
        }

        [TestMethod()]
        public void PutCapteurTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_capteurUpdate);

            // Act
            var actionResult = _capteurController.PutCapteur(_capteurUpdate.CapteurId, _capteur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual(false, _capteurUpdate.EstActif, "est actif n'a pas été mis à jour");
        }

        [TestMethod()]
        public void PostCapteurTest()
        {
            // Act 
            var actionResult = _capteurController.PostCapteur(_capteur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Capteur>), "Le retour n'est pas du type ActionResult<Capteur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_capteur.EstActif, ((Capteur)result.Value).EstActif, "Le est actif du Capteur ne correspond pas");
        }

        [TestMethod()]
        public void DeleteCapteurTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_capteur);
            // Act
            var actionResult = _capteurController.DeleteCapteur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}