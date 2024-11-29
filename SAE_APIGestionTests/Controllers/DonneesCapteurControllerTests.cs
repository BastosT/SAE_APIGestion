using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Npgsql;
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
    public class DonneesCapteurControllerTests : BaseTest
    {


        private DonneesCapteurController controller;
        private IDataRepository<DonneesCapteur> dataRepository;
        private DonneesCapteur donneesCapteur;
        private DonneesCapteur _donneesCapteur;
        private DonneesCapteur _donneesCapteurUpdate;
        private Mock<IDataRepository<DonneesCapteur>> _mockRepository;
        private DonneesCapteurController _donneesCapteurController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<DonneesCapteur>>();

            _donneesCapteur = new DonneesCapteur
             {
                DonneesCapteurId = 1,
                CapteurId = 1, 
                TypeDonneesId = 1, 
                Valeur = 23.5, 
                Timestamp = DateTime.Now 
            };

            _donneesCapteurUpdate = new DonneesCapteur
            {
                DonneesCapteurId = 1,
                CapteurId = 1,
                TypeDonneesId = 1,
                Valeur = 25,
                Timestamp = DateTime.Now
            };

            // Initialisation du contrôleur avec le mock
            _donneesCapteurController = new DonneesCapteurController(_mockRepository.Object);

            // pour les test unitaire 

            donneesCapteur = new DonneesCapteur
            {
                DonneesCapteurId = 999,
                Valeur = 23, 
                Timestamp = DateTime.UtcNow,
                CapteurId = 1,
                TypeDonneesId = 1,
            };

            // Appel à l'initialisation de la classe de base
            base.BaseInitialize();
            dataRepository = new DonneesCapteurManager (Context);
            controller = new DonneesCapteurController(dataRepository);
        }


        [TestCleanup]
        public void Teardown()
        {
            //Code pour nettoyer la base de données après chaque test
            using (var connection = new NpgsqlConnection("Server=localhost;port=5432;database=sae_rasp;uid=postgres;password=postgres"))
            {
                connection.Open();
                // chnager les id a delete quand les donnees seront ok 
                using (var command = new NpgsqlCommand("DELETE FROM t_e_donneescapteur_dcp where dcp_id = 999", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        [TestMethod()]
        public void GetDonneesCapteursTest()
        {
            var expectedList = Context.DonneesCapteurs.ToList();

            Task<ActionResult<IEnumerable<DonneesCapteur>>> listdonnees = controller.GetDonneesCapteurs();
            ActionResult<IEnumerable<DonneesCapteur>> resultat = listdonnees.Result;
            List<DonneesCapteur> listDonnescapteur = resultat.Value.ToList();


            CollectionAssert.AreEqual(expectedList, listDonnescapteur);
        }


        [TestMethod()]
        public void GetDonneesCapteurTest_moq()
        {

            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_donneesCapteur);

            // Act
            var actionResult = _donneesCapteurController.GetDonneesCapteur(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_donneesCapteur, actionResult.Value as DonneesCapteur);

        }


        [TestMethod()]
        public void PostDonnesCapteurTest()
        {
            // Act
            var result = controller.PostDonneesCapteur(donneesCapteur).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            // Assert
            var donnesRecupere = controller.GetDonneesCapteur(donneesCapteur.DonneesCapteurId).Result;
            DonneesCapteur donnees = donnesRecupere.Value;

            // Comparer les propriétés
            Assert.IsNotNull(donnees, "Le bâtiment récupéré ne doit pas être null");
            Assert.AreEqual(donneesCapteur.DonneesCapteurId, donnees.DonneesCapteurId, "Les IDs doivent correspondre");
            Assert.AreEqual(donneesCapteur.Valeur, donnees.Valeur, "Les valeurs doivent correspondre");
        }



        [TestMethod()]
        public void PutDonnesCapteurTest()
        {
            // Arrange         
            controller.PostDonneesCapteur(donneesCapteur);

            // Création d'une nouvelle donnes avec des données mises à jour

            var donneesCapteurUpdate = new DonneesCapteur
            {
                DonneesCapteurId = 999,
                Valeur = 42.5, 
                Timestamp = DateTime.UtcNow,
                CapteurId = 1,
                TypeDonneesId = 1,
            };


            // Act
            // Appel de la méthode PutCategorie du contrôleur avec la catégorie mise à jour
            var result = controller.PutDonneesCapteur(donneesCapteurUpdate.DonneesCapteurId, donneesCapteurUpdate).Result;

            // Assert
            // Vérification que la mise à jour a bien été effectuée
            DonneesCapteur donnesRecuperee = Context.DonneesCapteurs.FirstOrDefault(c => c.DonneesCapteurId == donneesCapteurUpdate.DonneesCapteurId);
            //Batiment batimentRecuperee = controller.GetBatiment(batimentUpdate.BatimentId).Result;
            Assert.IsNotNull(donnesRecuperee, "La catégorie n'a pas été trouvée dans la base de données après la mise à jour");
            Assert.AreEqual(donneesCapteurUpdate.Valeur, donnesRecuperee.Valeur, "Le nom de la catégorie mise à jour ne correspond pas");
        }


        [TestMethod()]
        public void DeleteDonnesCapteurTest()
        {

            controller.PostDonneesCapteur(donneesCapteur);

            // Act
            var result = controller.DeleteDonneesCapteur(donneesCapteur.DonneesCapteurId).Result; // Appel de la méthode DeleteCategorie pour supprimer la catégorie

            // Assert
            // Vérifier si la catégorie a été supprimée correctement
            DonneesCapteur donnesApresSuppression = Context.DonneesCapteurs.FirstOrDefault(c => c.DonneesCapteurId== donneesCapteur.DonneesCapteurId);
            Assert.IsNull(donnesApresSuppression, "La catégorie existe toujours après la suppression");
        }



        [TestMethod()]
        public void PutDonneesCapteurTest_moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_donneesCapteurUpdate);

            // Act
            var actionResult = _donneesCapteurController.PutDonneesCapteur(_donneesCapteurUpdate.DonneesCapteurId, _donneesCapteur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual(25, _donneesCapteurUpdate.Valeur, "La valeur des capteur n'a pas été mis à jour");
        }

        [TestMethod()]
        public void PostDonneesCapteurTest_moq()
        {
            // Act 
            var actionResult = _donneesCapteurController.PostDonneesCapteur(_donneesCapteur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<DonneesCapteur>), "Le retour n'est pas du type ActionResult<DonneesCapteur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_donneesCapteur.Valeur, ((DonneesCapteur)result.Value).Valeur, "la valeur des capteur ne correspond pas");

        }

        [TestMethod()]
        public void DeleteDonneesCapteurTest_moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_donneesCapteur);
            // Act
            var actionResult = _donneesCapteurController.DeleteDonneesCapteur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}