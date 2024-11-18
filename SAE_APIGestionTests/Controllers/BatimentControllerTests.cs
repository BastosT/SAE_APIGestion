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

namespace SAE_APIGestion.Controllers.Tests
{
    [TestClass()]
    public class BatimentControllerTests
    {

        private BatimentController controller;
        private GlobalDBContext ctx;
        private IDataRepository<Batiment> dataRepository;

        [TestMethod()]
        public void GetBatimentsTest_Moq()
        {
            // Arrange
            var batiment = new Batiment
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
                        Murs = new List<Mur>
                        {
                            new Mur
                            {
                                MurId = 1,
                                Nom = "Mur Nord",
                                Longueur = 10,
                                Hauteur = 3
                            }
                        },
                        Equipements = new List<Equipement>
                        {
                            new Equipement
                            {
                                EquipementId = 1,
                                Nom = "Equipement 1",
                                TypeEquipement = new TypeEquipement
                                {
                                    TypeEquipementId = 1,
                                    Nom = "Type Equipement 1"
                                },
                                Largeur = 1.5,
                                Hauteur = 2,
                                PositionX = 0.5,
                                PositionY = 0.5
                            }
                        }
                    }
                }
            };


            var mockRepository = new Mock<IDataRepository<Batiment>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(batiment);

            var marqueController = new BatimentController(mockRepository.Object);

            // Act
            var actionResult = marqueController.GetBatiment(1).Result;


            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(batiment, actionResult.Value as Batiment);
        }

        [TestMethod()]
        public void GetBatimentTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutBatimentTest_moq()
        {
            // Arrange
            var batiment = new Batiment
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
                        Murs = new List<Mur>
                        {
                            new Mur
                            {
                                MurId = 1,
                                Nom = "Mur Nord",
                                Longueur = 10,
                                Hauteur = 3
                            }
                        },
                        Equipements = new List<Equipement>
                        {
                            new Equipement
                            {
                                EquipementId = 1,
                                Nom = "Equipement 1",
                                TypeEquipement = new TypeEquipement
                                {
                                    TypeEquipementId = 1,
                                    Nom = "Type Equipement 1"
                                },
                                Largeur = 1.5,
                                Hauteur = 2,
                                PositionX = 0.5,
                                PositionY = 0.5
                            }
                        }
                    }
                }
            };

            // Arrange
            var batimentUpdate = new Batiment
            {
                BatimentId = 1,
                Nom = "Batiment Test update",
                Adresse = "123 Rue Test update",
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
                        Murs = new List<Mur>
                        {
                            new Mur
                            {
                                MurId = 1,
                                Nom = "Mur Nord",
                                Longueur = 10,
                                Hauteur = 3
                            }
                        },
                        Equipements = new List<Equipement>
                        {
                            new Equipement
                            {
                                EquipementId = 1,
                                Nom = "Equipement 1",
                                TypeEquipement = new TypeEquipement
                                {
                                    TypeEquipementId = 1,
                                    Nom = "Type Equipement 1"
                                },
                                Largeur = 1.5,
                                Hauteur = 2,
                                PositionX = 0.5,
                                PositionY = 0.5
                            }
                        }
                    }
                }
            };


            var mockRepository = new Mock<IDataRepository<Batiment>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(batimentUpdate);
            var marqueController = new BatimentController(mockRepository.Object);

            // Act
            var actionResult = marqueController.PutBatiment(batimentUpdate.BatimentId, batiment).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour

            // Vérification que la mise à jour a bien été faite 
            Assert.AreEqual("Batiment Test update", batimentUpdate.Nom, "Le nom du batiment n'a pas été mis à jour");
            Assert.AreEqual("123 Rue Test update", batimentUpdate.Adresse, "L'adresse du batiment n'a pas été mise à jour");
        }

        [TestMethod()]
        public void PostBatimentTest()
        {
            // Arrange
            var batiment = new Batiment
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
                        Murs = new List<Mur>
                        {
                            new Mur
                            {
                                MurId = 1,
                                Nom = "Mur Nord",
                                Longueur = 10,
                                Hauteur = 3
                            }
                        },
                        Equipements = new List<Equipement>
                        {
                            new Equipement
                            {
                                EquipementId = 1,
                                Nom = "Equipement 1",
                                TypeEquipement = new TypeEquipement
                                {
                                    TypeEquipementId = 1,
                                    Nom = "Type Equipement 1"
                                },
                                Largeur = 1.5,
                                Hauteur = 2,
                                PositionX = 0.5,
                                PositionY = 0.5
                            }
                        }
                    }
                }
            };

            var mockRepository = new Mock<IDataRepository<Batiment>>();
            var marqueController = new BatimentController(mockRepository.Object);


            // Act 
            var actionResult = marqueController.PostBatiment(batiment).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Batiment>), "Le retour n'est pas du type ActionResult<Marque>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Le résultat n'est pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result.Value, "La valeur retournée est nulle");
            Assert.AreEqual(batiment.Nom, ((Batiment)result.Value).Nom, "Le nom de la marque ne correspond pas");
        }


        [TestMethod()]
        public void DeleteBatimentTest_MoqinMemory()
        {

            var inMemoryDb = new List<Batiment>
                     {
                new Batiment
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
                            Murs = new List<Mur>
                            {
                                new Mur
                                {
                                    MurId = 1,
                                    Nom = "Mur Nord",
                                    Longueur = 10,
                                    Hauteur = 3
                                }
                            },
                            Equipements = new List<Equipement>
                            {
                                new Equipement
                                {
                                    EquipementId = 1,
                                    Nom = "Equipement 1",
                                    TypeEquipement = new TypeEquipement
                                    {
                                        TypeEquipementId = 1,
                                        Nom = "Type Equipement 1"
                                    },
                                    Largeur = 1.5,
                                    Hauteur = 2,
                                    PositionX = 0.5,
                                    PositionY = 0.5
                                }
                            }
                        }
                    }
                }
            };

            var mockRepository = new Mock<IDataRepository<Batiment>>();

            // Retourne le `Batiment` de l'inMemoryDb
            mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => inMemoryDb.FirstOrDefault(b => b.BatimentId == id));

            // Supprime le `Batiment` de l'inMemoryDb
            mockRepository.Setup(x => x.DeleteAsync(It.IsAny<Batiment>())).Callback<Batiment>(batimentToDelete =>
            {
                inMemoryDb.RemoveAll(b => b.BatimentId == batimentToDelete.BatimentId);
            }).Returns(Task.CompletedTask);

            var marqueController = new BatimentController(mockRepository.Object);

            // act
            var actionResult = marqueController.DeleteBatiment(1).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");

            // Vérification que le Batiment a bien été supprimé de l'inMemoryDb
            var deletedBatiment = inMemoryDb.FirstOrDefault(b => b.BatimentId == 1);
            Assert.IsNull(deletedBatiment, "Le Batiment n'a pas été supprimé correctement");
        }


    }
}