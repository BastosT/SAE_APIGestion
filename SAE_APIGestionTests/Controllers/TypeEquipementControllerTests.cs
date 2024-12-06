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
    public class TypeEquipementControllerTests : BaseTest
    {

        private TypeEquipementController controller;
        private IDataRepository<TypeEquipement> dataRepository;
        private TypeEquipement _typeEquipement;
        private TypeEquipement _typeEquipementUpdate;
        private TypeEquipement typeEquipement;
        private Mock<IDataRepository<TypeEquipement>> _mockRepository;
        private TypeEquipementController _typeequipementController;



        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<TypeEquipement>>();

            _typeEquipement = new TypeEquipement
            {
                TypeEquipementId = 1,
                Nom = "Projecteur"
            };

            _typeEquipementUpdate = new TypeEquipement
            {
                TypeEquipementId = 1,
                Nom = "Projecteur Update"
            };

            // Initialisation du contrôleur avec le mock
            _typeequipementController = new TypeEquipementController(_mockRepository.Object);


            // pour les test unitaire 

            typeEquipement = new TypeEquipement
            {
                TypeEquipementId = 999,
                Nom = "Projecteur"
            };

            // Appel à l'initialisation de la classe de base
            base.BaseInitialize();
            dataRepository = new TypeEquipementManager(Context);
            controller = new TypeEquipementController(dataRepository);

        }


        [TestCleanup]
        public void Teardown()
        {
            //Code pour nettoyer la base de données après chaque test
            using (var connection = new NpgsqlConnection("Server=localhost;port=5432;database=sae_rasp;uid=postgres;password=postgres"))
            {
                connection.Open();
                // chnager les id a delete quand les donnees seront ok 
                using (var command = new NpgsqlCommand("DELETE FROM t_e_typeequipement_tye where tye_id = 999", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod()]
        public void GetTypeEquipementsTest()
        {
            var expectedList = Context.TypesEquipements.ToList();

            Task<ActionResult<IEnumerable<TypeEquipement>>> listtype = controller.GetTypeEquipements();
            ActionResult<IEnumerable<TypeEquipement>> resultat = listtype.Result;
            List<TypeEquipement> listBatiment = resultat.Value.ToList();


            CollectionAssert.AreEqual(expectedList, listBatiment);
        }

        [TestMethod()]
        public void PostTypeEquipementTest()
        {
            // Act
            var result = controller.PostTypeEquipement(typeEquipement).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout

            // Assert
            var typeRecupere = controller.GetTypeEquipement(typeEquipement.TypeEquipementId).Result;
            TypeEquipement type = typeRecupere.Value;

            // Comparer les propriétés
            Assert.IsNotNull(type, "Le bâtiment récupéré ne doit pas être null");
            Assert.AreEqual(typeEquipement.TypeEquipementId, type.TypeEquipementId, "Les IDs doivent correspondre");
            Assert.AreEqual(typeEquipement.Nom, type.Nom, "Les noms doivent correspondre");
        }



        [TestMethod()]
        public async Task PutTypeEquipementTest()
        {
            // Arrange         
            await controller.PostTypeEquipement(typeEquipement);

            // Création d'une nouvelle catégorie avec des données mises à jour
            var typeEquipementUpdate = new TypeEquipement
            {
                TypeEquipementId = 999,
                Nom = "Projecteur update "
            };

            // Act
            // Appel de la méthode PutCategorie du contrôleur avec la catégorie mise à jour
            var result = await controller.PutTypeEquipement(typeEquipementUpdate.TypeEquipementId, typeEquipementUpdate);

            // Assert
            // Vérification que la mise à jour a bien été effectuée
            TypeEquipement donneesRecuperee = await Context.TypesEquipements.FirstOrDefaultAsync(c => c.TypeEquipementId == typeEquipementUpdate.TypeEquipementId);
            //Batiment batimentRecuperee = controller.GetBatiment(batimentUpdate.BatimentId).Result;
            Assert.IsNotNull(donneesRecuperee, "La catégorie n'a pas été trouvée dans la base de données après la mise à jour");
            Assert.AreEqual(typeEquipementUpdate.Nom, donneesRecuperee.Nom, "Le nom de la catégorie mise à jour ne correspond pas");
        }


        [TestMethod()]
        public async Task DeleteTypeEquipementTest()
        {

            await controller.PostTypeEquipement(typeEquipement);

            // Act
            var result = await controller.DeleteTypeEquipement(typeEquipement.TypeEquipementId); // Appel de la méthode DeleteCategorie pour supprimer la catégorie

            // Assert
            // Vérifier si la catégorie a été supprimée correctement
            TypeEquipement typeApresSuppression =  await Context.TypesEquipements.FirstOrDefaultAsync(c => c.TypeEquipementId == typeEquipement.TypeEquipementId);
            Assert.IsNull(typeApresSuppression, "La catégorie existe toujours après la suppression");
        }


        [TestMethod()]
        public void GetTypeEquipementTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeEquipement);

            // Act
            var actionResult = _typeequipementController.GetTypeEquipement(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_typeEquipement, actionResult.Value as TypeEquipement);
        }

        [TestMethod()]
        public void PutTypeEquipementTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeEquipementUpdate);

            // Act
            var actionResult = _typeequipementController.PutTypeEquipement(_typeEquipementUpdate.TypeEquipementId, _typeEquipement).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Projecteur Update", _typeEquipementUpdate.Nom, "Le nom du type n'a pas été mis à jour");
        }

        [TestMethod()]
        public void PostTypeEquipementTest_Moq()
        {
            // Act 
            var actionResult = _typeequipementController.PostTypeEquipement(_typeEquipement).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<TypeEquipement>), "Le retour n'est pas du type ActionResult<TypeEquipement>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_typeEquipement.Nom, ((TypeEquipement)result.Value).Nom, "Le nom du TypeEquipement ne correspond pas");
        }

        [TestMethod()]
        public void DeleteTypeEquipementTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_typeEquipement);
            // Act
            var actionResult = _typeequipementController.DeleteTypeEquipement(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}