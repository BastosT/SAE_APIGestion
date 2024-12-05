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
    public class MurControllerTests : BaseTest
    {

        private MurController controller;
        private IDataRepository<Mur> dataRepository;
        private Mur _mur;
        private Mur _murUpdate;
        private Mur mur;
        private Mock<IDataRepository<Mur>> _mockRepository;
        private MurController _murController;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IDataRepository<Mur>>();

            _mur = new Mur
             {
                MurId = 1,
                Nom = "Mur Nord",
                Longueur = 10.0,
                Hauteur = 3.0,
             };

            _murUpdate = new Mur
            {
                MurId = 1,
                Nom = "Mur Update",
                Longueur = 11.0,
                Hauteur = 3.0,
            };

            // Initialisation du contrôleur avec le mock
            _murController= new MurController(_mockRepository.Object);

            // test unitaire 
            mur = new Mur
            {
                MurId = 999,
                Nom = "Mur Nord",
                Longueur = 10.0,
                Hauteur = 3.0,
            };

            // Appel à l'initialisation de la classe de base
            base.BaseInitialize();
            dataRepository = new MurManager(Context);
            controller = new MurController(dataRepository);

        }



        [TestCleanup]
        public void Teardown()
        {
            //Code pour nettoyer la base de données après chaque test
            using (var connection = new NpgsqlConnection("Server=localhost;port=5432;database=sae_rasp;uid=postgres;password=postgres"))
            {
                connection.Open();
                // chnager les id a delete quand les donnees seront ok 
                using (var command = new NpgsqlCommand("DELETE FROM t_e_mur_mur where mur_id = 999", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        [TestMethod()]
        public void GetMursTest()
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
            Mur murApresSuppression = Context.Murs.FirstOrDefault(c => c.MurId== mur.MurId);
            Assert.IsNull(murApresSuppression, "La catégorie existe toujours après la suppression");
        }




        [TestMethod()]
        public void GetMurTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_mur);

            // Act
            var actionResult = _murController.GetMur(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(_mur, actionResult.Value as Mur);
        }

        [TestMethod()]
        public void PutMurTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_murUpdate);

            // Act
            var actionResult = _murController.PutMur(_murUpdate.MurId, _mur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Mur Update", _murUpdate.Nom, "Le nom du mur n'a pas été mis à jour");
            Assert.AreEqual(11, _murUpdate.Longueur, "La longeur n'a pas été mise à jour");
        }

        [TestMethod()]
        public void PostMurTest_Moq()
        {
            // Act 
            var actionResult = _murController.PostMur(_mur).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Mur>), "Le retour n'est pas du type ActionResult<Mur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(_mur.Nom, ((Mur)result.Value).Nom, "Le nom du mur ne correspond pas");
        }

        [TestMethod()]
        public void DeleteMurTest_Moq()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(_mur);
            // Act
            var actionResult = _murController.DeleteMur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}