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
using Npgsql;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.DataManger;
using SAE_APIGestionTests.Controllers;
using Microsoft.Extensions.Options;

namespace SAE_APIGestion.Controllers.Tests
{
    [TestClass()]
    public class CapteurControllerTests : BaseTest
    {

        private CapteurController controller;
        private IDataRepository<Capteur> dataRepository;
        private Capteur _capteur;
        private Capteur _capteurUpdate;
        private Capteur capteur;
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


            // pour les test unitaire 
            capteur = new Capteur
            {
                CapteurId = 999,
                Nom = "Capteur Test",
                EstActif = true,
                DistanceFenetre = 3.5,
                DistancePorte = 1.0,
                DistanceChauffage = 2.0,
                SalleId = 1,
                MurId = 1,
            };

            // Appel à l'initialisation de la classe de base
            base.BaseInitialize();


            dataRepository = new CapteurManager(Context);
            controller = new CapteurController(dataRepository);

        }


        [TestCleanup]
        public void Teardown()
        {
            //Code pour nettoyer la base de données après chaque test
            using (var connection = new NpgsqlConnection("Server=localhost;port=5432;database=sae_rasp;uid=postgres;password=postgres"))
            {
                connection.Open();
                // chnager les id a delete quand les donnees seront ok 
                using (var command = new NpgsqlCommand("DELETE FROM t_e_capteur_cap where cap_id = 999", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        [TestMethod()]
        public void GetCapteurTest_moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_capteur);

            // Act
            var actionResult = _capteurController.GetCapteur(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_capteur, actionResult.Value as Capteur);
        }


        //[TestMethod()]
        //public void GetCapteursTest()
        //{
        //    var expectedList = Context.Capteurs.ToList();

        //    Task<ActionResult<IEnumerable<Capteur>>> listCapt = controller.GetCapteurs();
        //    ActionResult<IEnumerable<Capteur>> resultat = listCapt.Result;
        //    List<Capteur> listCapteur = resultat.Value.ToList();


        //    CollectionAssert.AreEqual(expectedList, listCapteur);
        //}


        [TestMethod()]
        public void PostCapteurTest()
        {
            // Act
            var result = controller.PostCapteur(capteur).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            // Assert
            var capteurRecupere = controller.GetCapteur(capteur.CapteurId).Result;
            Capteur capt = capteurRecupere.Value;

            // Comparer les propriétés
            Assert.IsNotNull(capt, "Le bâtiment récupéré ne doit pas être null");
            Assert.AreEqual(capteur.CapteurId, capt.CapteurId, "Les IDs doivent correspondre");
            Assert.AreEqual(capteur.Nom, capt.Nom, "Les noms doivent correspondre");
            Assert.AreEqual(capteur.EstActif, capt.EstActif, "Les est actif doivent correspondre");
        }



        [TestMethod()]
        public async Task PutCapteurTest()
        {
            // Arrange         
            await controller.PostCapteur(capteur);
            var capteurUpdate = new Capteur
            {
                CapteurId = 999,
                Nom = "Capteur Test Update",
                EstActif = false,
                DistanceFenetre = 3.5,
                DistancePorte = 1.0,
                DistanceChauffage = 2.0,
                SalleId = 1,
                MurId = 1,
            };

            // Act
            var result = await controller.PutCapteur(capteurUpdate.CapteurId, capteurUpdate);

            // Assert
            var capteureRecuperee = await Context.Capteurs.FirstOrDefaultAsync(c => c.CapteurId == capteurUpdate.CapteurId);
            Assert.IsNotNull(capteureRecuperee);
            Assert.AreEqual(capteurUpdate.EstActif, capteureRecuperee.EstActif);
        }


        [TestMethod()]
        public void DeleteCapteurTest()
        {

            controller.PostCapteur(capteur);

            // Act
            var result = controller.DeleteCapteur(capteur.CapteurId).Result; // Appel de la méthode DeleteCategorie pour supprimer la catégorie

            // Assert
            // Vérifier si la catégorie a été supprimée correctement
            Capteur capteurApresSuppression = Context.Capteurs.FirstOrDefault(c => c.CapteurId == capteur.CapteurId);
            Assert.IsNull(capteurApresSuppression, "Le capteur existe toujours après la suppression");
        }



        [TestMethod()]
        public void PutCapteurTest_moq()
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
        public void PostCapteurTest_moq()
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
        public void DeleteCapteurTest_moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_capteur);
            // Act
            var actionResult = _capteurController.DeleteCapteur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}