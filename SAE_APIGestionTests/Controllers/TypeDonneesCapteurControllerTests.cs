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
                        Timestamp = DateTime.UtcNow
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
        public void GetTypeDonneesTest()
        {
            var expectedList = Context.TypesDonneesCapteurs.ToList();

            Task<ActionResult<IEnumerable<TypeDonneesCapteur>>> listdonnees = controller.GetTypeDonneesCapteurs();
            ActionResult<IEnumerable<TypeDonneesCapteur>> resultat = listdonnees.Result;
            List<TypeDonneesCapteur> listMurs = resultat.Value.ToList();


            CollectionAssert.AreEqual(expectedList, listMurs);
        }


        [TestMethod()]
        public void PostTypeDonneesTest()
        {
            // Act
            var result = controller.PostTypeDonneesCapteur(typeDonneesCapteur).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            // Assert
            var typedonnesRecupere = controller.GetTypeDonneesCapteur(typeDonneesCapteur.TypeDonneesCapteurId).Result;
            TypeDonneesCapteur type = typedonnesRecupere.Value;

            // Comparer les propriétés
            Assert.IsNotNull(type, "Le bâtiment récupéré ne doit pas être null");
            Assert.AreEqual(typeDonneesCapteur.TypeDonneesCapteurId, type.TypeDonneesCapteurId, "Les IDs doivent correspondre");
            Assert.AreEqual(typeDonneesCapteur.Nom, type.Nom, "Les noms doivent correspondre");
            Assert.AreEqual(typeDonneesCapteur.Unite, type.Unite, "La longeurs doivent correspondre");
        }



        [TestMethod()]
        public async Task PutTypeDonneesTest()
        {
            // Arrange         
            await controller.PostTypeDonneesCapteur(typeDonneesCapteur);

            // Création d'une nouvelle catégorie avec des données mises à jour

            var typeDonneesCapteurUpdate = new TypeDonneesCapteur
            {
                TypeDonneesCapteurId = 999,
                Nom = "Température update",
                Unite = "°C",
                DonneesCapteurs = new HashSet<DonneesCapteur>
                {
                    new DonneesCapteur
                    {
                        DonneesCapteurId = 999,
                        CapteurId = 1,
                        TypeDonneesId = 1,
                        Valeur = 22.5,
                        Timestamp = DateTime.UtcNow
                    }
                }
            };

            // Act
            // Appel de la méthode PutCategorie du contrôleur avec la catégorie mise à jour
            var result = await controller.PutTypeDonneesCapteur(typeDonneesCapteurUpdate.TypeDonneesCapteurId, typeDonneesCapteurUpdate);

            // Assert
            // Vérification que la mise à jour a bien été effectuée
            TypeDonneesCapteur typeRecuperee = await Context.TypesDonneesCapteurs.FirstOrDefaultAsync(c => c.TypeDonneesCapteurId== typeDonneesCapteurUpdate.TypeDonneesCapteurId);
            Assert.IsNotNull(typeRecuperee, "Le mur  n'a pas été trouvée dans la base de données après la mise à jour");
            Assert.AreEqual(typeDonneesCapteurUpdate.Nom, typeRecuperee.Nom, "Le nom du mur mise à jour ne correspond pas");
        }


        [TestMethod()]
        public void DeleteTypeDonneesTest()
        {

            controller.PostTypeDonneesCapteur(typeDonneesCapteur);

            // Act
            var result = controller.DeleteTypeDonneesCapteur(typeDonneesCapteur.TypeDonneesCapteurId).Result; // Appel de la méthode DeleteCategorie pour supprimer la catégorie

            // Assert
            // Vérifier si la catégorie a été supprimée correctement
            TypeDonneesCapteur typeApresSuppression = Context.TypesDonneesCapteurs.FirstOrDefault(c => c.TypeDonneesCapteurId== typeDonneesCapteur.TypeDonneesCapteurId);
            Assert.IsNull(typeApresSuppression, "La catégorie existe toujours après la suppression");

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