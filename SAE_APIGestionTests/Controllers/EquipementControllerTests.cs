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
    public class EquipementControllerTests : BaseTest
    {
        private EquipementController controller;
        private IDataRepository<Equipement> dataRepository;
        private Equipement _equipement;
        private Equipement _equipementUpdate;
        private Equipement equipement;
        private Mock<IDataRepository<Equipement>> _mockRepository;
        private EquipementController _equipementController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<Equipement>>();


            _equipement= new Equipement
            {
                EquipementId = 1,
                Nom = "Projecteur 1",
                TypeEquipementId = 1,
                Hauteur = 2.0,
                Longueur = 1.0,
                PositionX = 5.0,
                PositionY = 10.0,
                MurId = 1,
                SalleId = 1,
            };

            _equipementUpdate = new Equipement
            {
                EquipementId = 1,
                Nom = "Projecteur Update",
                TypeEquipementId = 1,
                Hauteur = 2.0,
                Longueur = 1.0,
                PositionX = 5.0,
                PositionY = 10.0,
                MurId = 1,
                SalleId = 1,
            };

            // Initialisation du contrôleur avec le mock
            _equipementController = new EquipementController(_mockRepository.Object);

            // pour les test unitaire 

            equipement = new Equipement
            {
                EquipementId = 999,
                Nom = "Projecteur 1",
                TypeEquipementId = 1,
                Hauteur = 2.0,
                Longueur = 1.0,
                PositionX = 5.0,
                PositionY = 10.0,
                MurId = 1,
                SalleId = 1,
            };

            // Appel à l'initialisation de la classe de base
            base.BaseInitialize();
            dataRepository = new EquipementManager(Context);
            controller = new EquipementController(dataRepository);

        }


        [TestCleanup]
        public void Teardown()
        {
            //Code pour nettoyer la base de données après chaque test
            using (var connection = new NpgsqlConnection("Server=localhost;port=5432;database=sae_rasp;uid=postgres;password=postgres"))
            {
                connection.Open();
                // chnager les id a delete quand les donnees seront ok 
                using (var command = new NpgsqlCommand("DELETE FROM t_e_equipement_equ where equ_id = 999", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        [TestMethod()]
        public void GetEquipementsTest()
        {
            var expectedList = Context.Equipements.ToList();

            Task<ActionResult<IEnumerable<Equipement>>> listEqu = controller.GetEquipements();
            ActionResult<IEnumerable<Equipement>> resultat = listEqu.Result;
            List<Equipement> listEquipement = resultat.Value.ToList();


            CollectionAssert.AreEqual(expectedList, listEquipement);
        }



        [TestMethod()]
        public void PostEquipementTest()
        {
            // Act
            var result = controller.PostEquipement(equipement).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            // Assert
            var equRecupere = controller.GetEquipement(equipement.EquipementId).Result;
            Equipement equ = equRecupere.Value;

            // Comparer les propriétés
            Assert.IsNotNull(equ, "L'Equipement récupéré ne doit pas être null");
            Assert.AreEqual(equipement.EquipementId, equ.EquipementId, "Les IDs doivent correspondre");
            Assert.AreEqual(equipement.Nom, equ.Nom, "Les noms doivent correspondre");
            Assert.AreEqual(equipement.Hauteur, equ.Hauteur, "La Hauteur doivent correspondre");
        }


        //[TestMethod()]
        //public void PutEquipementTest()
        //{
        //    // Arrange         
        //    controller.PostEquipement(equipement);

        //    // Création d'une nouvelle catégorie avec des données mises à jour
        //    var equipementUpdate = new Equipement
        //    {s
        //        EquipementId = 999,
        //        Nom = "Projecteur 1 update",
        //        TypeEquipementId = 1,
        //        Hauteur = 2.0,
        //        Longueur = 1.0,
        //        PositionX = 5.0,
        //        PositionY = 10.0,
        //        MurId = 1,
        //        SalleId = 1,
        //    };

        //    // Act
        //    // Appel de la méthode PutCategorie du contrôleur avec la catégorie mise à jour
        //    var result = controller.PutEquipement(equipementUpdate.EquipementId, equipementUpdate).Result;

        //    // Assert
        //    // Vérification que la mise à jour a bien été effectuée
        //    Equipement equipementRecuperee = Context.Equipements.FirstOrDefault(c => c.EquipementId == equipementUpdate.EquipementId);
        //    //Batiment batimentRecuperee = controller.GetBatiment(batimentUpdate.BatimentId).Result;
        //    Assert.IsNotNull(equipementRecuperee, "La catégorie n'a pas été trouvée dans la base de données après la mise à jour");
        //    Assert.AreEqual(equipementUpdate.Nom, equipementRecuperee.Nom, "Le nom de la catégorie mise à jour ne correspond pas");
        //}



        [TestMethod()]
        public void DeleteEquipementTest()
        {

            controller.PostEquipement(equipement);

            // Act
            var result = controller.DeleteEquipement(equipement.EquipementId).Result; // Appel de la méthode DeleteCategorie pour supprimer la catégorie

            // Assert
            // Vérifier si la catégorie a été supprimée correctement
            Equipement equipementApresSuppression = Context.Equipements.FirstOrDefault(c => c.EquipementId == equipement.EquipementId);
            Assert.IsNull(equipementApresSuppression, "La catégorie existe toujours après la suppression");
        }


        [TestMethod()]
        public void GetEquipementTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_equipement);

            // Act
            var actionResult = _equipementController.GetEquipement(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_equipement, actionResult.Value as Equipement);
        }





        [TestMethod()]
        public void PutEquipementTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_equipementUpdate);

            // Act
            var actionResult = _equipementController.PutEquipement(_equipementUpdate.EquipementId, _equipement).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Projecteur Update", _equipementUpdate.Nom, "Le nom de l'quipement n'a pas été mis à jour");
        }
    

        [TestMethod()]
        public void PostEquipementTest_Moq()
        {
            // Act 
            var actionResult = _equipementController.PostEquipement(_equipement).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Equipement>), "Le retour n'est pas du type ActionResult<Equipement>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_equipement.Nom, ((Equipement)result.Value).Nom, "Le nom de l'Equipement ne correspond pas");
        }

        [TestMethod()]
        public void DeleteEquipementTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_equipement);
            // Act
            var actionResult = _equipementController.DeleteEquipement(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

        }
    }
}