//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using Npgsql;
//using SAE_APIGestion.Controllers;
//using SAE_APIGestion.Models.DataManger;
//using SAE_APIGestion.Models.EntityFramework;
//using SAE_APIGestionTests.Controllers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SAE_APIGestion.Controllers.Tests
//{
//    [TestClass()]
//    public class SalleControllerTests : BaseTest
//    {

//        private SalleController controller;
//        private IDataRepository<Salle> dataRepository;
//        private Salle _salle;
//        private Salle _salleUpdate;
//        private Salle salle;
//        private Mock<IDataRepository<Salle>> _mockRepository;
//        private SalleController _salleController;


//        [TestInitialize]
//        public void Initialize()
//        {
//            _mockRepository = new Mock<IDataRepository<Salle>>();

//            // salle 
//            _salle = new Salle
//            {
//                SalleId = 1,
//                Nom = "Salle de réunion",
//                Surface = 45.5,
//                TypeSalleId = 1,
//                TypeSalle = new TypeSalle
//                {
//                    TypeSalleId = 1,
//                    Nom = "Réunion",
//                    Description = "Salle équipée pour des réunions d'équipe"
//                },
//                BatimentId = 1,
//                Batiment = new Batiment
//                {
//                    BatimentId = 1,
//                    Nom = "Bâtiment Principal",
//                    Adresse = "123 Rue Test"
//                },
//                MurFaceId = 1,
//                MurFace = new Mur
//                {
//                    MurId = 1,
//                    Nom = "Mur Nord",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurEntreeId = 2,
//                MurEntree = new Mur
//                {
//                    MurId = 2,
//                    Nom = "Mur Sud",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurGaucheId = 3,
//                MurGauche = new Mur
//                {
//                    MurId = 3,
//                    Nom = "Mur Ouest",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurDroiteId = 4,
//                MurDroite = new Mur
//                {
//                    MurId = 4,
//                    Nom = "Mur Est",
//                    Longueur = 10,
//                    Hauteur = 3
//                }
//            };


//            // update salle 
//            _salleUpdate = new Salle
//            {
//                SalleId = 1,
//                Nom = "Salle de réunion update",
//                Surface = 46,
//                TypeSalleId = 1,
//                TypeSalle = new TypeSalle
//                {
//                    TypeSalleId = 1,
//                    Nom = "Réunion",
//                    Description = "Salle équipée pour des réunions d'équipe"
//                },
//                BatimentId = 1,
//                Batiment = new Batiment
//                {
//                    BatimentId = 1,
//                    Nom = "Bâtiment Principal",
//                    Adresse = "123 Rue Test"
//                },
//                MurFaceId = 1,
//                MurFace = new Mur
//                {
//                    MurId = 1,
//                    Nom = "Mur Nord",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurEntreeId = 2,
//                MurEntree = new Mur
//                {
//                    MurId = 2,
//                    Nom = "Mur Sud",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurGaucheId = 3,
//                MurGauche = new Mur
//                {
//                    MurId = 3,
//                    Nom = "Mur Ouest",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurDroiteId = 4,
//                MurDroite = new Mur
//                {
//                    MurId = 4,
//                    Nom = "Mur Est",
//                    Longueur = 10,
//                    Hauteur = 3
//                }
//            };




//            // Initialisation du contrôleur avec le mock
//            _salleController = new SalleController(_mockRepository.Object);

//            // test unitaire 
//            salle = new Salle
//            {
//                SalleId = 999,
//                Nom = "Salle de réunion",
//                Surface = 45.5,
//                TypeSalleId = 999,
//                TypeSalle = new TypeSalle
//                {
//                    TypeSalleId = 999,
//                    Nom = "Réunion",
//                    Description = "Salle équipée pour des réunions d'équipe"
//                },
//                BatimentId = 1,
//                Batiment = new Batiment
//                {
//                    BatimentId = 999,
//                    Nom = "Bâtiment Principal",
//                    Adresse = "123 Rue Test"
//                },
//                MurFaceId = 500,
//                MurFace = new Mur
//                {
//                    MurId = 999,
//                    Nom = "Mur Nord",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurEntreeId = 501,
//                MurEntree = new Mur
//                {
//                    MurId = 998,
//                    Nom = "Mur Sud",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurGaucheId = 502,
//                MurGauche = new Mur
//                {
//                    MurId = 997,
//                    Nom = "Mur Ouest",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurDroiteId = 503,
//                MurDroite = new Mur
//                {
//                    MurId = 986,
//                    Nom = "Mur Est",
//                    Longueur = 10,
//                    Hauteur = 3
//                }
//            };

//            // Appel à l'initialisation de la classe de base
//            base.BaseInitialize();
//            dataRepository = new SalleManager(Context);
//            controller = new SalleController(dataRepository);

//        }



//        [TestCleanup]
//        public void Teardown()
//        {
//            //Code pour nettoyer la base de données après chaque test
//            using (var connection = new NpgsqlConnection("Server=localhost;port=5432;database=sae_rasp;uid=postgres;password=postgres"))
//            {
//                connection.Open();
//                // chnager les id a delete quand les donnees seront ok 
//                using (var command = new NpgsqlCommand("DELETE FROM t_e_salle_sal where sal_id = 999;DELETE FROM t_e_batiment_bat where bat_id = 999; DELETE FROM t_e_mur_mur where mur_id = 999; DELETE FROM t_e_mur_mur where mur_id = 998 ; DELETE FROM t_e_mur_mur where mur_id = 997; DELETE FROM t_e_mur_mur where mur_id = 986; DELETE FROM t_e_typesalle_tys where tys_id = 999 ", connection))
//                {
//                    command.ExecuteNonQuery();
//                }
//            }
//        }


//        [TestMethod()]
//        public void GetSalleTest_Moq()
//        {
           
//            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_salle);

//            // Act
//            var actionResult = _salleController.GetSalle(1).Result;

//            // Assert
//            Assert.IsNotNull(actionResult);
//            Assert.IsNotNull(actionResult.Value);
//            Assert.AreEqual(_salle, actionResult.Value as Salle);
//        }


//        [TestMethod()]
//        public void GetSallesTest()
//        {
//            var expectedList = Context.Salles.ToList();

//            Task<ActionResult<IEnumerable<Salle>>> listsal = controller.GetSalles();
//            ActionResult<IEnumerable<Salle>> resultat = listsal.Result;
//            List<Salle> listSalles = resultat.Value.ToList();


//            CollectionAssert.AreEqual(expectedList, listSalles);
//        }


//        [TestMethod()]
//        public void PostSalleTest()
//        {
//            // Act
//            var result = controller.PostSalle(salle).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

//            // Assert
//            var salleRecupere = controller.GetSalle(salle.SalleId).Result;
//            Salle salles = salleRecupere.Value;

//            // Comparer les propriétés
//            Assert.IsNotNull(salles, "Le bâtiment récupéré ne doit pas être null");
//            Assert.AreEqual(salle.SalleId, salles.SalleId, "Les IDs doivent correspondre");
//            Assert.AreEqual(salle.Nom, salles.Nom, "Les noms doivent correspondre");
//            Assert.AreEqual(salle.MurDroite, salles.MurDroite, "Les murs doivent correspondre");
//        }



//        [TestMethod()]
//        public async Task PutSalleTest()
//        {
//            // Arrange         
//            await controller.PostSalle(salle);

//            // Création d'une nouvelle catégorie avec des données mises à jour
//            var salleUpdate = new Salle
//            {
//                SalleId = 999,
//                Nom = "Salle de réunion udapte",
//                Surface = 45.5,
//                TypeSalleId = 999,
//                TypeSalle = new TypeSalle
//                {
//                    TypeSalleId = 999,
//                    Nom = "Réunion",
//                    Description = "Salle équipée pour des réunions d'équipe"
//                },
//                BatimentId = 1,
//                Batiment = new Batiment
//                {
//                    BatimentId = 999,
//                    Nom = "Bâtiment Principal",
//                    Adresse = "123 Rue Test"
//                },
//                MurFaceId = 500,
//                MurFace = new Mur
//                {
//                    MurId = 999,
//                    Nom = "Mur Nord",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurEntreeId = 501,
//                MurEntree = new Mur
//                {
//                    MurId = 998,
//                    Nom = "Mur Sud",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurGaucheId = 502,
//                MurGauche = new Mur
//                {
//                    MurId = 997,
//                    Nom = "Mur Ouest",
//                    Longueur = 10,
//                    Hauteur = 3
//                },
//                MurDroiteId = 503,
//                MurDroite = new Mur
//                {
//                    MurId = 986,
//                    Nom = "Mur Est",
//                    Longueur = 10,
//                    Hauteur = 3
//                }
//            };

//            // Act
//            // Appel de la méthode PutCategorie du contrôleur avec la catégorie mise à jour
//            var result = await controller.PutSalle(salleUpdate.SalleId, salleUpdate);

//            // Assert
//            // Vérification que la mise à jour a bien été effectuée
//            Salle salleRecuperee = await Context.Salles.FirstOrDefaultAsync(c => c.SalleId == salleUpdate.SalleId);
//            Assert.IsNotNull(salleRecuperee, "Le mur  n'a pas été trouvée dans la base de données après la mise à jour");
//            Assert.AreEqual(salleUpdate.Nom, salleRecuperee.Nom, "Le nom du mur mise à jour ne correspond pas");
//        }


//        [TestMethod()]
//        public void DeleteSalleTest()
//        {

//            controller.PostSalle(salle);

//            // Act
//            var result = controller.DeleteSalle(salle.SalleId).Result; // Appel de la méthode DeleteCategorie pour supprimer la catégorie

//            // Assert
//            // Vérifier si la catégorie a été supprimée correctement
//            Salle salleApresSuppression = Context.Salles.FirstOrDefault(c => c.SalleId == salle.SalleId);
//            Assert.IsNull(salleApresSuppression, "La catégorie existe toujours après la suppression");
//        }



//        [TestMethod()]
//        public void PutSalleTest_Moq()
//        {
            
//            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_salleUpdate);
           
//            // Act
//            var actionResult = _salleController.PutSalle(_salleUpdate.SalleId, _salle).Result;

//            // Assert
//            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

//            // Vérification que la mise à jour a bien été faite 
//            Assert.AreEqual("Salle de réunion update", _salleUpdate.Nom, "Le nom de la salle n'a pas été mis à jour");
//            Assert.AreEqual(46, _salleUpdate.Surface, "La surface n'a pas été mis à jour");

//        }

//        [TestMethod()]
//        public void PostSalleTest_Moq()
//        {
            
//            // Act 
//            var actionResult = _salleController.PostSalle(_salle).Result;

//            // Assert
//            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Salle>), "Le retour n'est pas du type ActionResult<Salle>");
//            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
//            var result = actionResult.Result as CreatedAtActionResult;
//            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
//            Assert.AreEqual(_salle.Nom, ((Salle)result.Value).Nom, "Le nom de la salle ne correspond pas");


//        }

//        [TestMethod()]
//        public void DeleteSalleTest_Moq()
//        {
//            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_salle);
//            // Act
//            var actionResult = _salleController.DeleteSalle(1).Result;
//            // Assert
//            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

//        }
//    }
//}