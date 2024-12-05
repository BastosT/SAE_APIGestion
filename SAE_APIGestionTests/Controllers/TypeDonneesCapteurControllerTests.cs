using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class TypeDonneesCapteurControllerTests : BaseTest
    {

        private TypeDonneesCapteurController controller;
        private IDataRepository<TypeDonneesCapteur> dataRepository;
        private TypeDonneesCapteur _typeDonneesCapteur;
        private TypeDonneesCapteur _typeDonneesCapteurUpdate;
        private TypeDonneesCapteur typeDonneesCapteur;
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

            // pour les test unitaire 

            typeDonneesCapteur = new TypeDonneesCapteur
            {
                TypeDonneesCapteurId = 999,
                Nom = "Température",
                Unite = "°C",
                DonneesCapteurs = new HashSet<DonneesCapteur>
                {
                    new DonneesCapteur
                    {
                        DonneesCapteurId = 999,
                        CapteurId = 1,
                        TypeDonneesId = 1,
                        Valeur = 22.5,
                        Timestamp = DateTime.Now
                    }
                }
            };


            // Appel à l'initialisation de la classe de base
            base.BaseInitialize();
            dataRepository = new TypeDonneesCapteurManager(Context);
            controller = new TypeDonneesCapteurController(dataRepository);

        }


        [TestCleanup]
        public void Teardown()
        {
            //Code pour nettoyer la base de données après chaque test
            using (var connection = new NpgsqlConnection("Server=localhost;port=5432;database=sae_rasp;uid=postgres;password=postgres"))
            {
                connection.Open();
                // chnager les id a delete quand les donnees seront ok 
                using (var command = new NpgsqlCommand("DELETE FROM t_e_typedonneescapteur_tdc where tdc_id = 999; DELETE FROM t_e_donneescapteur_dcp where dcp_id = 999", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod()]
        public void GetTest()
        {
            var expectedList = Context.Murs.ToList();

            Task<ActionResult<IEnumerable<Mur>>> listmur = controller.GetMurs();
            ActionResult<IEnumerable<Mur>> resultat = listmur.Result;
            List<Mur> listMurs = resultat.Value.ToList();


            CollectionAssert.AreEqual(expectedList, listMurs);
        }


        [TestMethod()]
        public void PostMurTest()
        {
            // Act
            var result = controller.PostMur(mur).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            // Assert
            var murRecupere = controller.GetMur(mur.MurId).Result;
            Mur murss = murRecupere.Value;

            // Comparer les propriétés
            Assert.IsNotNull(murss, "Le bâtiment récupéré ne doit pas être null");
            Assert.AreEqual(mur.MurId, murss.MurId, "Les IDs doivent correspondre");
            Assert.AreEqual(mur.Nom, murss.Nom, "Les noms doivent correspondre");
            Assert.AreEqual(mur.Longueur, murss.Longueur, "La longeurs doivent correspondre");
        }



        [TestMethod()]
        public async Task PutMurTest()
        {
            // Arrange         
            await controller.PostMur(mur);

            // Création d'une nouvelle catégorie avec des données mises à jour
            var murUpdate = new Mur
            {
                MurId = 999,
                Nom = "Mur Update",
                Longueur = 11.0,
                Hauteur = 3.0,
            };

            // Act
            // Appel de la méthode PutCategorie du contrôleur avec la catégorie mise à jour
            var result = await controller.PutMur(murUpdate.MurId, murUpdate);

            // Assert
            // Vérification que la mise à jour a bien été effectuée
            Mur murRecuperee = await Context.Murs.FirstOrDefaultAsync(c => c.MurId == murUpdate.MurId);
            Assert.IsNotNull(murRecuperee, "Le mur  n'a pas été trouvée dans la base de données après la mise à jour");
            Assert.AreEqual(murUpdate.Nom, murRecuperee.Nom, "Le nom du mur mise à jour ne correspond pas");
            Assert.AreEqual(murUpdate.Longueur, murRecuperee.Longueur, "La longeur du mur mise à jour ne correspond pas");
        }


        [TestMethod()]
        public void DeleteMurTest()
        {

            controller.PostMur(mur);

            // Act
            var result = controller.DeleteMur(mur.MurId).Result; // Appel de la méthode DeleteCategorie pour supprimer la catégorie

            // Assert
            // Vérifier si la catégorie a été supprimée correctement
            Mur murApresSuppression = Context.Murs.FirstOrDefault(c => c.MurId == mur.MurId);
            Assert.IsNull(murApresSuppression, "La catégorie existe toujours après la suppression");
        }


        [TestMethod()]
        public void GetTypeDonneesCapteurTest_Moq()
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
        public void PutTypeDonneesCapteurTest_Moq()
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
        public void PostTypeDonneesCapteurTest_Moq()
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