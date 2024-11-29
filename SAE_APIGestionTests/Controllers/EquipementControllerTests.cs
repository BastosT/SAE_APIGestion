using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SAE_APIGestion.Controllers;
using SAE_APIGestion.Models.DataManger;
using SAE_APIGestion.Models.EntityFramework;
using SAE_APIGestionTests.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_APIGestion.Controllers.Tests
{
    [TestClass()]
    public class EquipementControllerTests : BaseTest
    {
        private EquipementController controller;
        private IDataRepository<Equipement> dataRepository;
        private Equipement _equipement;
        private Equipement _equipementUpdate;
        private Equipement equipement;
        private Mock<IDataRepository<Equipement>> _mockRepository;
        private EquipementController _equipementController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<Equipement>>();


            _equipement= new Equipement
            {
                EquipementId = 1,
                Nom = "Projecteur 1",
                TypeEquipementId = 1,
                Hauteur = 2.0,
                Longueur = 1.0,
                PositionX = 5.0,
                PositionY = 10.0,
                MurId = 1,
                SalleId = 1,
            };

            _equipementUpdate = new Equipement
            {
                EquipementId = 1,
                Nom = "Projecteur Update",
                TypeEquipementId = 1,
                Hauteur = 2.0,
                Longueur = 1.0,
                PositionX = 5.0,
                PositionY = 10.0,
                MurId = 1,
                SalleId = 1,
            };

            // Initialisation du contrôleur avec le mock
            _equipementController = new EquipementController(_mockRepository.Object);

            // pour les test unitaire 

            equipement = new Equipement
            {
                EquipementId = 1,
                Nom = "Projecteur 1",
                TypeEquipementId = 1,
                Hauteur = 2.0,
                Longueur = 1.0,
                PositionX = 5.0,
                PositionY = 10.0,
                MurId = 1,
                SalleId = 1,
            };

            // Appel à l'initialisation de la classe de base
            base.BaseInitialize();
            dataRepository = new EquipementManager(Context);
            controller = new EquipementController(dataRepository);

        }


        [TestMethod()]
        public void GetEquipementTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_equipement);

            // Act
            var actionResult = _equipementController.GetEquipement(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_equipement, actionResult.Value as Equipement);
        }

        [TestMethod()]
        public void PutEquipementTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_equipementUpdate);

            // Act
            var actionResult = _equipementController.PutEquipement(_equipementUpdate.EquipementId, _equipement).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Projecteur Update", _equipementUpdate.Nom, "Le nom de l'quipement n'a pas été mis à jour");
        }
    

        [TestMethod()]
        public void PostEquipementTest()
        {
            // Act 
            var actionResult = _equipementController.PostEquipement(_equipement).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Equipement>), "Le retour n'est pas du type ActionResult<Equipement>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_equipement.Nom, ((Equipement)result.Value).Nom, "Le nom de l'Equipement ne correspond pas");
        }

        [TestMethod()]
        public void DeleteEquipementTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_equipement);
            // Act
            var actionResult = _equipementController.DeleteEquipement(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour


        }
    }
}