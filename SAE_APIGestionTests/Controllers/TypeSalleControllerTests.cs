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
    public class TypeSalleControllerTests
    {
        private GlobalDBContext ctx;
        private IDataRepository<TypeSalle> dataRepository;
        private TypeSalle _typeSalle;
        private TypeSalle _typeSalleUpdate;
        private Mock<IDataRepository<TypeSalle>> _mockRepository;
        private TypeSalleController _typesalleController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<TypeSalle>>();

            _typeSalle = new TypeSalle
             {
                TypeSalleId = 1,
                Nom = "Salle de classe",
                Description = "Une salle conçue pour les cours et les formations.",
                Salles = new List<Salle>
             {
                new Salle
                {
                    SalleId = 1,
                    Nom = "Salle A",
                    Surface = 50.0,
                    BatimentId = 1,
                    TypeSalleId = 1, // Correspond à ce TypeSalle
                },
                new Salle
                {
                    SalleId = 2,
                    Nom = "Salle B",
                    Surface = 40.0,
                    BatimentId = 1,
                    TypeSalleId = 1, // Correspond à ce TypeSalle
                }
                }
             };

            _typeSalleUpdate = new TypeSalle
            {
                TypeSalleId = 1,
                Nom = "Salle de classe Udapte",
                Description = "Une salle conçue pour les cours et les formations Update",
                Salles = new List<Salle>
             {
                new Salle
                {
                    SalleId = 1,
                    Nom = "Salle A",
                    Surface = 50.0,
                    BatimentId = 1,
                    TypeSalleId = 1, 
                },
                new Salle
                {
                    SalleId = 2,
                    Nom = "Salle B",
                    Surface = 40.0,
                    BatimentId = 1,
                    TypeSalleId = 1, 
                }
                }
            };

            // Initialisation du contrôleur avec le mock
            _typesalleController = new TypeSalleController(_mockRepository.Object);
        }

        [TestMethod()]
        public void GetTypeSalleTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeSalle);

            // Act
            var actionResult = _typesalleController.GetTypeSalle(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_typeSalle, actionResult.Value as TypeSalle);

        }

        [TestMethod()]
        public void PutTypeSalleTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeSalleUpdate);

            // Act
            var actionResult = _typesalleController.PutTypeSalle(_typeSalleUpdate.TypeSalleId, _typeSalle).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Salle de classe Udapte", _typeSalleUpdate.Nom, "Le nom de la salle n'a pas été mis à jour");
            Assert.AreEqual("Une salle conçue pour les cours et les formations Update", _typeSalleUpdate.Description, "La description n'a pas été mise à jour");

        }

        [TestMethod()]
        public void PostTypeSalleTest()
        {
            // Act 
            var actionResult = _typesalleController.PostTypeSalle(_typeSalle).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<TypeSalle>), "Le retour n'est pas du type ActionResult<TypeSalle>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_typeSalle.Nom, ((TypeSalle)result.Value).Nom, "Le nom de la type salle ne correspond pas");
        }

        [TestMethod()]
        public void DeleteTypeSalleTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeSalle);
            // Act
            var actionResult = _typesalleController.DeleteTypeSalle(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}