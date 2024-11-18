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
    public class SalleControllerTests
    {

        private BatimentController controller;
        private GlobalDBContext ctx;
        private IDataRepository<Batiment> dataRepository;

        [TestMethod()]
        public void SalleControllerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSalleTest_Moq()
        {
            // Arrange
            var salleTest = new Salle
            {
                SalleId = 1,
                Nom = "Salle de Conférence",
                Surface = 45.0,
                TypeSalle = new TypeSalle
                {
                    TypeSalleId = 1,
                    Nom = "Conférence",
                    Description = "Salle équipée pour des conférences et des réunions"
                },
                Murs = new List<Mur>
                {
            new Mur { MurId = 1, Nom = "Mur Est", Longueur = 12.0, Hauteur = 3.0 },
            new Mur { MurId = 2, Nom = "Mur Ouest", Longueur = 12.0, Hauteur = 3.0 }
                },
                Equipements = new List<Equipement>
            {
            new Equipement
            {
                EquipementId = 1,
                Nom = "Projecteur",
                TypeEquipement = new TypeEquipement { TypeEquipementId = 1, Nom = "Électronique" },
                Largeur = 0.5,
                Hauteur = 0.5,
                PositionX = 2.0,
                PositionY = 1.0
            }
            }
            };

            var mockRepository = new Mock<IDataRepository<Salle>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(salleTest);

            var salleController = new SalleController(mockRepository.Object);

            // Act
            var actionResult = salleController.GetSalle(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(salleTest, actionResult.Value as Salle);
        }


        [TestMethod()]
        public void GetSalleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutSalleTest()
        {
            // Arrange
            var salleTest = new Salle
            {
                SalleId = 1,
                Nom = "Salle de Conférence",
                Surface = 45.0,
                TypeSalle = new TypeSalle
                {
                    TypeSalleId = 1,
                    Nom = "Conférence",
                    Description = "Salle équipée pour des conférences et des réunions"
                },
                Murs = new List<Mur>
                {
            new Mur { MurId = 1, Nom = "Mur Est", Longueur = 12.0, Hauteur = 3.0 },
            new Mur { MurId = 2, Nom = "Mur Ouest", Longueur = 12.0, Hauteur = 3.0 }
                },
                Equipements = new List<Equipement>
            {
            new Equipement
            {
                EquipementId = 1,
                Nom = "Projecteur",
                TypeEquipement = new TypeEquipement { TypeEquipementId = 1, Nom = "Électronique" },
                Largeur = 0.5,
                Hauteur = 0.5,
                PositionX = 2.0,
                PositionY = 1.0
            }
            }
            };

            
            var salleTestUpdate = new Salle
            {
                SalleId = 1,
                Nom = "Salle de Conférence update",
                Surface = 46.0,
                TypeSalle = new TypeSalle
                {
                    TypeSalleId = 1,
                    Nom = "Conférence",
                    Description = "Salle équipée pour des conférences et des réunions"
                },
                Murs = new List<Mur>
                {
            new Mur { MurId = 1, Nom = "Mur Est", Longueur = 12.0, Hauteur = 3.0 },
            new Mur { MurId = 2, Nom = "Mur Ouest", Longueur = 12.0, Hauteur = 3.0 }
                },
                Equipements = new List<Equipement>
            {
            new Equipement
            {
                EquipementId = 1,
                Nom = "Projecteur",
                TypeEquipement = new TypeEquipement { TypeEquipementId = 1, Nom = "Électronique" },
                Largeur = 0.5,
                Hauteur = 0.5,
                PositionX = 2.0,
                PositionY = 1.0
            }
            }
            };


            var mockRepository = new Mock<IDataRepository<Salle>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(salleTestUpdate);
            var salleController = new SalleController(mockRepository.Object);

            // Act
            var actionResult = salleController.PutSalle(salleTestUpdate.SalleId, salleTest).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Salle de Conférence update", salleTestUpdate.Nom, "Le nom de la salle n'a pas été mis à jour");
            Assert.AreEqual(46, salleTestUpdate.Surface, "La surface n'a pas été mis à jour");

        }

        [TestMethod()]
        public void PostSalleTest()
        {
            // Arrange
            var salleTest = new Salle
            {
                SalleId = 1,
                Nom = "Salle de Conférence",
                Surface = 45.0,
                TypeSalle = new TypeSalle
                {
                    TypeSalleId = 1,
                    Nom = "Conférence",
                    Description = "Salle équipée pour des conférences et des réunions"
                },
                Murs = new List<Mur>
                {
            new Mur { MurId = 1, Nom = "Mur Est", Longueur = 12.0, Hauteur = 3.0 },
            new Mur { MurId = 2, Nom = "Mur Ouest", Longueur = 12.0, Hauteur = 3.0 }
                },
                Equipements = new List<Equipement>
            {
            new Equipement
            {
                EquipementId = 1,
                Nom = "Projecteur",
                TypeEquipement = new TypeEquipement { TypeEquipementId = 1, Nom = "Électronique" },
                Largeur = 0.5,
                Hauteur = 0.5,
                PositionX = 2.0,
                PositionY = 1.0
            }
            }
            };


            var mockRepository = new Mock<IDataRepository<Salle>>();
            var salleController = new SalleController(mockRepository.Object);


            // Act 
            var actionResult = salleController.PostSalle(salleTest).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Salle>), "Le retour n'est pas du type ActionResult<Salle>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(salleTest.Nom, ((Salle)result.Value).Nom, "Le nom de la salle ne correspond pas");


        }

        [TestMethod()]
        public void DeleteSalleTest()
        {
            
        }
    }
}