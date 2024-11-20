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
    public class MurControllerTests
    {

        private GlobalDBContext ctx;
        private IDataRepository<Mur> dataRepository;
        private Mur _mur;
        private Mur _murUpdate;
        private Mock<IDataRepository<Mur>> _mockRepository;
        private MurController _murController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<Mur>>();

            _mur = new Mur
             {
                MurId = 1,
                Nom = "Mur Nord",
                Longueur = 10.0,
                Hauteur = 3.0,
                SalleId = 1,
                Salle = new Salle
                {
                    SalleId = 1,
                    Nom = "Salle Test",
                    Surface = 50.0
                }
             };

            _murUpdate = new Mur
            {
                MurId = 1,
                Nom = "Mur Update",
                Longueur = 11.0,
                Hauteur = 3.0,
                SalleId = 1,
                Salle = new Salle
                {
                    SalleId = 1,
                    Nom = "Salle Test",
                    Surface = 50.0
                }
            };

            // Initialisation du contrôleur avec le mock
            _murController= new MurController(_mockRepository.Object);

        }


        [TestMethod()]
        public void GetMurTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_mur);

            // Act
            var actionResult = _murController.GetMur(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_mur, actionResult.Value as Mur);
        }

        [TestMethod()]
        public void PutMurTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_murUpdate);

            // Act
            var actionResult = _murController.PutMur(_murUpdate.MurId, _mur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Mur Update", _murUpdate.Nom, "Le nom du mur n'a pas été mis à jour");
            Assert.AreEqual(11, _murUpdate.Longueur, "La longeur n'a pas été mise à jour");
        }

        [TestMethod()]
        public void PostMurTest()
        {
            // Act 
            var actionResult = _murController.PostMur(_mur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Mur>), "Le retour n'est pas du type ActionResult<Mur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_mur.Nom, ((Mur)result.Value).Nom, "Le nom du mur ne correspond pas");
        }

        [TestMethod()]
        public void DeleteMurTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_mur);
            // Act
            var actionResult = _murController.DeleteMur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}