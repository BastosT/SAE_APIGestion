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
    public class TypeDonneesCapteurControllerTests
    {

        private GlobalDBContext ctx;
        private IDataRepository<TypeDonneesCapteur> dataRepository;
        private TypeDonneesCapteur _typeDonneesCapteur;
        private TypeDonneesCapteur _typeDonneesCapteurUpdate;
        private Mock<IDataRepository<TypeDonneesCapteur>> _mockRepository;
        private TypeDonneesCapteurController _typeDonneesCapteurController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<TypeDonneesCapteur>>();

            _typeDonneesCapteur = new TypeDonneesCapteur
            {
                TypeDonneesCapteurId = 1,
                Nom = "Température",
                Unite = "°C",
                DonneesCapteurs = new HashSet<DonneesCapteur>
                {
                    new DonneesCapteur
                    {
                        DonneesCapteurId = 1,
                        CapteurId = 1,
                        TypeDonneesId = 1,
                        Valeur = 22.5,
                        Timestamp = DateTime.Now
                    }
                }
            };

            _typeDonneesCapteurUpdate = new TypeDonneesCapteur
            {
                TypeDonneesCapteurId = 1,
                Nom = "Température update",
                Unite = "°C",
                DonneesCapteurs = new HashSet<DonneesCapteur>
                {
                    new DonneesCapteur
                    {
                        DonneesCapteurId = 1,
                        CapteurId = 1,
                        TypeDonneesId = 1,
                        Valeur = 22.5,
                        Timestamp = DateTime.Now
                    }
                }
            };


            // Initialisation du contrôleur avec le mock
            _typeDonneesCapteurController = new TypeDonneesCapteurController(_mockRepository.Object);

        }

        [TestMethod()]
        public void GetTypeDonneesCapteurTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeDonneesCapteur);

            // Act
            var actionResult = _typeDonneesCapteurController.GetTypeDonneesCapteur(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_typeDonneesCapteur, actionResult.Value as TypeDonneesCapteur);
        }

        [TestMethod()]
        public void PutTypeDonneesCapteurTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeDonneesCapteurUpdate);

            // Act
            var actionResult = _typeDonneesCapteurController.PutTypeDonneesCapteur(_typeDonneesCapteurUpdate.TypeDonneesCapteurId, _typeDonneesCapteur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Température update", _typeDonneesCapteurUpdate.Nom, "Le nom n'a pas été mis à jour");
        }

        [TestMethod()]
        public void PostTypeDonneesCapteurTest()
        {
            // Act 
            var actionResult = _typeDonneesCapteurController.PostTypeDonneesCapteur(_typeDonneesCapteur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<TypeDonneesCapteur>), "Le retour n'est pas du type ActionResult<TypeDonneesCapteur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_typeDonneesCapteur.Nom, ((TypeDonneesCapteur)result.Value).Nom, "Le nom du TypeDonneesCapteur ne correspond pas");
        }

        [TestMethod()]
        public void DeleteTypeDonneesCapteurTest()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeDonneesCapteur);
            // Act
            var actionResult = _typeDonneesCapteurController.DeleteTypeDonneesCapteur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}