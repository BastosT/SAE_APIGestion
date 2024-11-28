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

namespace SAE_APIGestion.Controllers.Tests
{
    [TestClass()]
    public class BatimentControllerTests : BaseTest
    {

        private GlobalDBContext ctx;
        private BatimentController controller;
        private IDataRepository<Batiment> dataRepository;
        private Batiment _batiment;
        private Batiment _batimentUpdate;
        private Batiment batiment;
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

            // Initialisation du contrôleur avec le mock
            _batimentController = new BatimentController(_mockRepository.Object);

            // pour les test unitaire 

            batiment = new Batiment
            {
                BatimentId = 999,
                Nom = "Batiment Test",
                Adresse = "123 Rue Test"
            };

            // Appel à l'initialisation de la classe de base
            base.BaseInitialize();
            dataRepository = new BatimentManager(Context);
            controller = new BatimentController(dataRepository);
        }

        [TestCleanup]
        public void Teardown()
        {
            //Code pour nettoyer la base de données après chaque test
            using (var connection = new NpgsqlConnection("Server=localhost;port=5432;database=sae_rasp;uid=postgres;password=postgres"))
            {
                connection.Open();
                // chnager les id a delete quand les donnees seront ok 
                using (var command = new NpgsqlCommand("DELETE FROM t_e_batiment_bat where bat_id = 999; DELETE FROM t_e_batiment_bat where bat_id = 3", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod()]
        public void GetBatimentsTest()
        {
            var expectedList = Context.Batiments.ToList();

            Task<ActionResult<IEnumerable<Batiment>>> listBat = controller.GetBatiments();
            ActionResult<IEnumerable<Batiment>> resultat = listBat.Result;
            List<Batiment> listBatiment = resultat.Value.ToList();


            CollectionAssert.AreEqual(expectedList, listBatiment);
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
        public void PostBatimentTest()
        {
            // Act
            var result = controller.PostBatiment(batiment).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            // Assert
            var batRecupere = controller.GetBatiment(batiment.BatimentId).Result;
            Batiment bat = batRecupere.Value;

            // Comparer les propriétés
            Assert.IsNotNull(bat, "Le bâtiment récupéré ne doit pas être null");
            Assert.AreEqual(batiment.BatimentId, bat.BatimentId, "Les IDs doivent correspondre");
            Assert.AreEqual(batiment.Nom, bat.Nom, "Les noms doivent correspondre");
            Assert.AreEqual(batiment.Adresse, bat.Adresse, "Les adresses doivent correspondre");
        }



        [TestMethod()]
        public void PutBatimentTest()
        {
            // Arrange         
            ctx.Batiments.Add(batiment);

            // Création d'une nouvelle catégorie avec des données mises à jour
            var batimentUpdate = new Batiment
            {
                BatimentId = 999,
                Nom = "Batiment Test",
                Adresse = "123 Rue Test"
            };

            // Act
            // Appel de la méthode PutCategorie du contrôleur avec la catégorie mise à jour
            var result = controller.PutBatiment(batimentUpdate.BatimentId, batimentUpdate).Result; 

            // Assert
            // Vérification que la mise à jour a bien été effectuée
            Batiment batimentRecuperee = ctx.Batiments.FirstOrDefault(c => c.BatimentId== batimentUpdate.BatimentId); // Récupération de la catégorie mise à jour depuis la base de données
            Assert.IsNotNull(batimentRecuperee, "La catégorie n'a pas été trouvée dans la base de données après la mise à jour");
            Assert.AreEqual(batimentUpdate.Nom, batimentRecuperee.Nom, "Le nom de la catégorie mise à jour ne correspond pas");
        }


        [TestMethod()]
        public void DeleteBatimentTest()
        {

            ctx.Batiments.Add(batiment);
            ctx.SaveChanges();

            // Act
            var result = controller.DeleteBatiment(batiment.BatimentId).Result; // Appel de la méthode DeleteCategorie pour supprimer la catégorie

            // Assert
            // Vérifier si la catégorie a été supprimée correctement
            Batiment batimentApresSuppression = ctx.Batiments.FirstOrDefault(c => c.BatimentId== batiment.BatimentId);
            Assert.IsNull(batimentApresSuppression, "La catégorie existe toujours après la suppression");
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