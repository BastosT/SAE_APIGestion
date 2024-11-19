using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAE_APIGestion.Controllers;
using SAE_APIGestion.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace SAE_APIGestion.Controllers.Tests
{
    [TestClass()]
    public class BatimentControllerTests
    {

        private BatimentController controller;
        private GlobalDBContext ctx;
        private IDataRepository<Batiment> dataRepository;
        private Batiment _batiment;
        private Batiment _batimentUpdate;
        private Batiment _batimentSansNom;
        private Mock<IDataRepository<Batiment>> _mockRepository;
        private BatimentController _batimentController;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<Batiment>>();

            // batiment
            _batiment = new Batiment
            {
                BatimentId = 1,
                Nom = "Batiment Test",
                Adresse = "123 Rue Test",
                Salles = new List<Salle>
            {
                new Salle
                {
                    SalleId = 1,
                    Nom = "Salle 1",
                    Surface = 30.5,
                    TypeSalle = new TypeSalle
                    {
                        TypeSalleId = 1,
                        Nom = "Type Salle 1",
                        Description = "Description Type Salle 1"
                    },
                    Murs = new List<Mur>
                    {
                        new Mur
                        {
                            MurId = 1,
                            Nom = "Mur Nord",
                            Longueur = 10,
                            Hauteur = 3
                        }
                    },
                    Equipements = new List<Equipement>
                    {
                        new Equipement
                        {
                            EquipementId = 1,
                            Nom = "Equipement 1",
                            TypeEquipement = new TypeEquipement
                            {
                                TypeEquipementId = 1,
                                Nom = "Type Equipement 1"
                            },
                            Largeur = 1.5,
                            Hauteur = 2,
                            PositionX = 0.5,
                            PositionY = 0.5
                        }
                    }
                }
            }
            };

            // Mise à jour du bâtiment
            _batimentUpdate = new Batiment
            {
                BatimentId = 1,
                Nom = "Batiment Test update",
                Adresse = "123 Rue Test update",
                Salles = new List<Salle>(_batiment.Salles) 
            };


            _batimentSansNom = new Batiment
             {
                 BatimentId = 0,
                 Nom = null, // Pas de nom
                 Adresse = "123 Rue Test",
                 Salles = new List<Salle>
                 {
                    new Salle
                    {
                        SalleId = 1,
                        Nom = "Salle 1",
                        Surface = 30.5,
                        TypeSalle = new TypeSalle
                        {
                            TypeSalleId = 1,
                            Nom = "Type Salle 1",
                            Description = "Description Type Salle 1"
                        }
                    }
                  }
             };


            // Initialisation du contrôleur avec le mock
            _batimentController = new BatimentController(_mockRepository.Object);
        }


        [TestMethod()]
        public void GetBatimentsTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_batiment);

            // Act
            var actionResult = _batimentController.GetBatiment(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_batiment, actionResult.Value as Batiment);
        }

        [TestMethod()]
        public void GetBatimentTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutBatimentTest_moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_batimentUpdate);

            // Act
            var actionResult = _batimentController.PutBatiment(_batimentUpdate.BatimentId, _batiment).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Batiment Test update", _batimentUpdate.Nom, "Le nom du batiment n'a pas été mis à jour");
            Assert.AreEqual("123 Rue Test update", _batimentUpdate.Adresse, "L'adresse du batiment n'a pas été mise à jour");
        }

        [TestMethod()]
        public void PostBatimentTest_Moq()
        {
            // Act 
            var actionResult = _batimentController.PostBatiment(_batiment).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Batiment>), "Le retour n'est pas du type ActionResult<Batiment>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_batiment.Nom, ((Batiment)result.Value).Nom, "Le nom du batiment ne correspond pas");
        }

        [TestMethod()]
        public void PostBatimentTestNoName_Moq()
        {
            // Act 
            var actionResult = _batimentController.PostBatiment(_batimentSansNom).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult), "Le résultat n'est pas un BadRequestObjectResult");
            var badRequest = actionResult.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest, "Le résultat BadRequest est null");
            Assert.AreEqual("Le nom du bâtiment est obligatoire.", badRequest.Value.ToString(), "Le message d'erreur ne correspond pas.");
        }


        [TestMethod()]
        public void DeleteBatimentTest_Moq()
        {     
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_batiment);
            // Act
            var actionResult = _batimentController.DeleteBatiment(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

        }

    }
}