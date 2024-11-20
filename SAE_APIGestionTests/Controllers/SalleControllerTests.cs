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

            // update salle 
            _salleUpdate = new Salle
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
            Assert.AreEqual("Salle de Conférence update", _salleUpdate.Nom, "Le nom de la salle n'a pas été mis à jour");
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