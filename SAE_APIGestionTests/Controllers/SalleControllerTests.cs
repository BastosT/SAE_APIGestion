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

        private GlobalDBContext ctx;
        private IDataRepository<Salle> dataRepository;
        private Salle _salle;
        private Salle _salleUpdate;
        private Mock<IDataRepository<Salle>> _mockRepository;
        private SalleController _salleController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<Salle>>();

            // salle 
            _salle = new Salle
            {
                SalleId = 1,
                Nom = "Salle de réunion",
                Surface = 45.5,
                TypeSalleId = 1,
                TypeSalle = new TypeSalle
                {
                    TypeSalleId = 1,
                    Nom = "Réunion",
                    Description = "Salle équipée pour des réunions d'équipe"
                },
                BatimentId = 1,
                Batiment = new Batiment
                {
                    BatimentId = 1,
                    Nom = "Bâtiment Principal",
                    Adresse = "123 Rue Test"
                },
                MurFaceId = 1,
                MurFace = new Mur
                {
                    MurId = 1,
                    Nom = "Mur Nord",
                    Longueur = 10,
                    Hauteur = 3
                },
                MurEntreeId = 2,
                MurEntree = new Mur
                {
                    MurId = 2,
                    Nom = "Mur Sud",
                    Longueur = 10,
                    Hauteur = 3
                },
                MurGaucheId = 3,
                MurGauche = new Mur
                {
                    MurId = 3,
                    Nom = "Mur Ouest",
                    Longueur = 10,
                    Hauteur = 3
                },
                MurDroiteId = 4,
                MurDroite = new Mur
                {
                    MurId = 4,
                    Nom = "Mur Est",
                    Longueur = 10,
                    Hauteur = 3
                }
            };


            // update salle 
            _salleUpdate = new Salle
            {
                SalleId = 1,
                Nom = "Salle de réunion update",
                Surface = 46,
                TypeSalleId = 1,
                TypeSalle = new TypeSalle
                {
                    TypeSalleId = 1,
                    Nom = "Réunion",
                    Description = "Salle équipée pour des réunions d'équipe"
                },
                BatimentId = 1,
                Batiment = new Batiment
                {
                    BatimentId = 1,
                    Nom = "Bâtiment Principal",
                    Adresse = "123 Rue Test"
                },
                MurFaceId = 1,
                MurFace = new Mur
                {
                    MurId = 1,
                    Nom = "Mur Nord",
                    Longueur = 10,
                    Hauteur = 3
                },
                MurEntreeId = 2,
                MurEntree = new Mur
                {
                    MurId = 2,
                    Nom = "Mur Sud",
                    Longueur = 10,
                    Hauteur = 3
                },
                MurGaucheId = 3,
                MurGauche = new Mur
                {
                    MurId = 3,
                    Nom = "Mur Ouest",
                    Longueur = 10,
                    Hauteur = 3
                },
                MurDroiteId = 4,
                MurDroite = new Mur
                {
                    MurId = 4,
                    Nom = "Mur Est",
                    Longueur = 10,
                    Hauteur = 3
                }
            };




            // Initialisation du contrôleur avec le mock
            _salleController = new SalleController(_mockRepository.Object);


        }

        [TestMethod()]
        public void GetSalleTest_Moq()
        {
           
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_salle);

            // Act
            var actionResult = _salleController.GetSalle(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_salle, actionResult.Value as Salle);
        }

        [TestMethod()]
        public void PutSalleTest()
        {
            
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_salleUpdate);
           
            // Act
            var actionResult = _salleController.PutSalle(_salleUpdate.SalleId, _salle).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Salle de réunion update", _salleUpdate.Nom, "Le nom de la salle n'a pas été mis à jour");
            Assert.AreEqual(46, _salleUpdate.Surface, "La surface n'a pas été mis à jour");

        }

        [TestMethod()]
        public void PostSalleTest()
        {
            
            // Act 
            var actionResult = _salleController.PostSalle(_salle).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Salle>), "Le retour n'est pas du type ActionResult<Salle>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_salle.Nom, ((Salle)result.Value).Nom, "Le nom de la salle ne correspond pas");


        }

        [TestMethod()]
        public void DeleteSalleTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_salle);
            // Act
            var actionResult = _salleController.DeleteSalle(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

        }
    }
}