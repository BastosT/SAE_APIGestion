﻿(function () {

    window.babylonInterop = window.babylonInterop || {};

    Object.assign(window.babylonInterop, {
        config: {
            scale: 0.09,
            wallDepth: 0.30,
            equipmentDepth: 0.15
        },

        renderEquipment: function (scene, equipment, wall, wallWidth, wallHeight) {
            // Pour les fenêtres et vitres

            if (equipment.typeEquipement &&
                (equipment.typeEquipement.nom.toLowerCase() === "fenetre" ||
                    equipment.typeEquipement.nom.toLowerCase() === "vitre")) {
                const frameWidth = equipment.longueur * this.config.scale;
                const frameHeight = equipment.hauteur * this.config.scale;
                const frameThickness = 0.02;
                const frame = new BABYLON.TransformNode("windowFrame", scene);

                const parts = [
                    {
                        dimensions: {
                            width: frameWidth,
                            height: frameThickness,
                            depth: this.config.wallDepth
                        },
                        position: new BABYLON.Vector3(0, frameHeight / 2, 0)
                    },
                    {
                        dimensions: {
                            width: frameWidth,
                            height: frameThickness,
                            depth: this.config.wallDepth
                        },
                        position: new BABYLON.Vector3(0, -frameHeight / 2, 0)
                    },
                    {
                        dimensions: {
                            width: frameThickness,
                            height: frameHeight,
                            depth: this.config.wallDepth
                        },
                        position: new BABYLON.Vector3(-frameWidth / 2, 0, 0)
                    },
                    {
                        dimensions: {
                            width: frameThickness,
                            height: frameHeight,
                            depth: this.config.wallDepth
                        },
                        position: new BABYLON.Vector3(frameWidth / 2, 0, 0)
                    }
                ];

                const frameMaterial = new BABYLON.StandardMaterial("frameMaterial", scene);
                frameMaterial.diffuseColor = new BABYLON.Color3(0.7, 0.7, 0.7);

                parts.forEach((part, index) => {
                    const mesh = BABYLON.MeshBuilder.CreateBox("framePart" + index, part.dimensions, scene);
                    mesh.position = part.position;
                    mesh.material = frameMaterial;
                    mesh.parent = frame;
                });

                const glass = BABYLON.MeshBuilder.CreatePlane("windowGlass", {
                    width: frameWidth - frameThickness * 2,
                    height: frameHeight - frameThickness * 2,
                    sideOrientation: BABYLON.Mesh.DOUBLESIDE 
                }, scene);

                const glassMaterial = new BABYLON.StandardMaterial("glassMaterial", scene);
                glassMaterial.diffuseColor = new BABYLON.Color3(0.8, 0.8, 1.0);
                glassMaterial.alpha = 0.3;
                glassMaterial.backFaceCulling = false;  // Ajouter cette ligne aussi
                glass.material = glassMaterial;
                glass.position.z = frameThickness / 2;
                glass.parent = frame;

                // Nouveau calcul de position aligné avec les trous
                const xPos = -wallWidth / 2 + equipment.positionX * this.config.scale + frameWidth / 2;
                const yPos = wallHeight / 2 - equipment.positionY * this.config.scale - frameHeight / 2;

                frame.position = new BABYLON.Vector3(
                    xPos,
                    yPos,
                    0  // Centré dans le mur
                );

                frame.parent = wall;
                return frame;
            }

            // Pour les radiateurs
            else if (equipment.typeEquipement && equipment.typeEquipement.nom.toLowerCase() === "radiateur") {
                const radiatorContainer = new BABYLON.TransformNode("radiatorContainer", scene);
                const width = equipment.longueur * this.config.scale;
                const height = equipment.hauteur * this.config.scale;
                const depth = this.config.equipmentDepth;

                // Paramètres pour les sections
                const numberOfSections = 3; // Nombre de sections du radiateur
                const sectionSpacing = 0.02; // Espacement entre les sections
                const sectionWidth = (width - (sectionSpacing * (numberOfSections - 1))) / numberOfSections;

                // Paramètres des ailettes
                const finSpacing = 0.12;
                const finThickness = 0.06;
                const finDepth = depth * 0.85;

                // Création du panneau arrière principal
                const mainBackPanel = BABYLON.MeshBuilder.CreateBox("mainBackPanel", {
                    width: width,
                    height: height,
                    depth: 0.02
                }, scene);
                mainBackPanel.position = new BABYLON.Vector3(0, 0, -depth / 2 + 0.01);
                const backPanelMaterial = new BABYLON.PBRMaterial("mainBackPanelMaterial", scene);
                backPanelMaterial.metallic = 0.7;
                backPanelMaterial.roughness = 0.4;
                backPanelMaterial.albedoColor = new BABYLON.Color3(0.85, 0.85, 0.85);
                mainBackPanel.material = backPanelMaterial;
                mainBackPanel.parent = radiatorContainer;

                // Création des sections
                for (let section = 0; section < numberOfSections; section++) {
                    const sectionContainer = new BABYLON.TransformNode(`section${section}`, scene);
                    sectionContainer.parent = radiatorContainer;

                    // Position de la section
                    const sectionOffset = -width / 2 + (sectionWidth / 2) + (section * (sectionWidth + sectionSpacing));
                    sectionContainer.position.x = sectionOffset;

                    // Cadre de la section
                    const sectionFrame = [
                        // Haut
                        {
                            dimensions: { width: sectionWidth, height: 0.04, depth: depth },
                            position: new BABYLON.Vector3(0, height / 2 - 0.02, 0)
                        },
                        // Bas
                        {
                            dimensions: { width: sectionWidth, height: 0.04, depth: depth },
                            position: new BABYLON.Vector3(0, -height / 2 + 0.02, 0)
                        },
                        // Gauche
                        {
                            dimensions: { width: 0.04, height: height, depth: depth },
                            position: new BABYLON.Vector3(-sectionWidth / 2 + 0.02, 0, 0)
                        },
                        // Droite
                        {
                            dimensions: { width: 0.04, height: height, depth: depth },
                            position: new BABYLON.Vector3(sectionWidth / 2 - 0.02, 0, 0)
                        }
                    ];

                    // Création du cadre de section
                    sectionFrame.forEach((part, idx) => {
                        const framePart = BABYLON.MeshBuilder.CreateBox(`sectionFrame${section}_${idx}`, part.dimensions, scene);
                        framePart.position = part.position;
                        const frameMaterial = new BABYLON.PBRMaterial(`sectionFrameMaterial${section}_${idx}`, scene);
                        frameMaterial.metallic = 0.8;
                        frameMaterial.roughness = 0.3;
                        frameMaterial.albedoColor = new BABYLON.Color3(0.9, 0.9, 0.9);
                        framePart.material = frameMaterial;
                        framePart.parent = sectionContainer;
                    });

                    // Ailettes de la section
                    const numberOfFins = Math.floor(sectionWidth / finSpacing);
                    for (let i = 0; i < numberOfFins; i++) {
                        const fin = BABYLON.MeshBuilder.CreateBox(`fin${section}_${i}`, {
                            width: finThickness,
                            height: height - 0.08,
                            depth: finDepth
                        }, scene);

                        fin.position = new BABYLON.Vector3(
                            -sectionWidth / 2 + 0.04 + (i * finSpacing),
                            0,
                            0
                        );

                        const finMaterial = new BABYLON.PBRMaterial(`finMaterial${section}_${i}`, scene);
                        finMaterial.metallic = 0.85;
                        finMaterial.roughness = 0.35;
                        finMaterial.albedoColor = new BABYLON.Color3(0.92, 0.92, 0.92);
                        fin.material = finMaterial;
                        fin.parent = sectionContainer;
                    }

                    // Ajout de détails décoratifs entre les sections
                    if (section < numberOfSections - 1) {
                        const connector = BABYLON.MeshBuilder.CreateBox(`sectionConnector${section}`, {
                            width: sectionSpacing,
                            height: height * 0.8,
                            depth: depth * 0.7
                        }, scene);
                        connector.position = new BABYLON.Vector3(
                            sectionOffset + sectionWidth / 2 + sectionSpacing / 2,
                            0,
                            0
                        );
                        const connectorMaterial = new BABYLON.PBRMaterial(`connectorMaterial${section}`, scene);
                        connectorMaterial.metallic = 0.75;
                        connectorMaterial.roughness = 0.45;
                        connectorMaterial.albedoColor = new BABYLON.Color3(0.88, 0.88, 0.88);
                        connector.material = connectorMaterial;
                        connector.parent = radiatorContainer;
                    }
                }

                // Tuyaux avec connexions améliorées
                const pipeRadius = 0.012;
                const pipeHeight = 0.15;

                for (let i = 0; i < 2; i++) {
                    // Tuyau principal
                    const pipe = BABYLON.MeshBuilder.CreateCylinder(`mainPipe${i}`, {
                        height: pipeHeight,
                        diameter: pipeRadius * 2,
                        tessellation: 20
                    }, scene);
                    pipe.position = new BABYLON.Vector3(
                        (i === 0 ? -1 : 1) * (width / 4),
                        -height / 2 - pipeHeight / 2,
                        0
                    );

                    // Connecteur horizontal
                    const connector = BABYLON.MeshBuilder.CreateCylinder(`pipeConnector${i}`, {
                        height: 0.05,
                        diameter: pipeRadius * 1.8,
                        tessellation: 20
                    }, scene);
                    connector.rotation.z = Math.PI / 2;
                    connector.position = new BABYLON.Vector3(
                        (i === 0 ? -1 : 1) * (width / 4),
                        -height / 2 - 0.03,
                        0
                    );

                    // Vanne décorative
                    const valve = BABYLON.MeshBuilder.CreateTorus(`valve${i}`, {
                        diameter: pipeRadius * 4,
                        thickness: pipeRadius * 0.5,
                        tessellation: 20
                    }, scene);
                    valve.rotation.x = Math.PI / 2;
                    valve.position = new BABYLON.Vector3(
                        (i === 0 ? -1 : 1) * (width / 4),
                        -height / 2 - pipeHeight + 0.02,
                        pipeRadius * 2
                    );

                    const pipeMaterial = new BABYLON.PBRMaterial(`pipeMaterial${i}`, scene);
                    pipeMaterial.metallic = 0.9;
                    pipeMaterial.roughness = 0.15;
                    pipeMaterial.albedoColor = new BABYLON.Color3(0.8, 0.8, 0.8);

                    pipe.material = pipeMaterial;
                    connector.material = pipeMaterial;
                    valve.material = pipeMaterial;

                    pipe.parent = radiatorContainer;
                    connector.parent = radiatorContainer;
                    valve.parent = radiatorContainer;
                }

                let xPos = -wallWidth / 2 + equipment.positionX * this.config.scale + width / 2;
                let yPos = wallHeight / 2 - equipment.positionY * this.config.scale - height / 2;
                let zPos = this.config.wallDepth / 2 + depth / 2;

                radiatorContainer.position = new BABYLON.Vector3(-xPos, yPos, zPos);
                radiatorContainer.rotation.y = Math.PI;
                radiatorContainer.parent = wall;

                return radiatorContainer;
            }
            // Pour les portes
            else if (equipment.typeEquipement && equipment.typeEquipement.nom.toLowerCase() === "porte") {
                const doorFrame = new BABYLON.TransformNode("doorFrame", scene);
                const frameWidth = equipment.longueur * this.config.scale;
                const frameHeight = equipment.hauteur * this.config.scale;
                const frameThickness = 0.04;  // Plus épais que les fenêtres pour plus de solidité

                // Créer le cadre de la porte
                const parts = [
                    // Haut du cadre
                    {
                        dimensions: {
                            width: frameWidth,
                            height: frameThickness,
                            depth: this.config.wallDepth
                        },
                        position: new BABYLON.Vector3(0, frameHeight / 2, 0)
                    },
                    // Côté gauche
                    {
                        dimensions: {
                            width: frameThickness,
                            height: frameHeight,
                            depth: this.config.wallDepth
                        },
                        position: new BABYLON.Vector3(-frameWidth / 2, 0, 0)
                    },
                    // Côté droit
                    {
                        dimensions: {
                            width: frameThickness,
                            height: frameHeight,
                            depth: this.config.wallDepth
                        },
                        position: new BABYLON.Vector3(frameWidth / 2, 0, 0)
                    }
                ];

                // Matériau pour le cadre
                const frameMaterial = new BABYLON.StandardMaterial("doorFrameMaterial", scene);
                frameMaterial.diffuseColor = new BABYLON.Color3(0.4, 0.2, 0.1);

                // Créer les parties du cadre
                parts.forEach((part, index) => {
                    const mesh = BABYLON.MeshBuilder.CreateBox("doorFramePart" + index, part.dimensions, scene);
                    mesh.position = part.position;
                    mesh.material = frameMaterial;
                    mesh.parent = doorFrame;
                });

                // Créer la porte elle-même
                const door = BABYLON.MeshBuilder.CreateBox("doorPanel", {
                    width: frameWidth - frameThickness * 2,
                    height: frameHeight,
                    depth: frameThickness
                }, scene);

                const doorMaterial = new BABYLON.StandardMaterial("doorMaterial", scene);
                doorMaterial.diffuseColor = new BABYLON.Color3(0.4, 0.2, 0.1);
                door.material = doorMaterial;
                door.position.z = this.config.wallDepth / 2;
                door.parent = doorFrame;

                // Positionner le tout
                const xPos = -wallWidth / 2 + equipment.positionX * this.config.scale + frameWidth / 2;
                const yPos = wallHeight / 2 - equipment.positionY * this.config.scale - frameHeight / 2;

                doorFrame.position = new BABYLON.Vector3(
                    -xPos,
                    yPos,
                    0  // Centré dans le mur
                );

                doorFrame.parent = wall;
                doorFrame.rotation.y = Math.PI;
                return doorFrame;
            }
            // Pour les autres équipements (comportement par défaut)
            else {
                const width = equipment.longueur * this.config.scale;
                const height = equipment.hauteur * this.config.scale;
                const depth = this.config.equipmentDepth;

                let xPos = -wallWidth / 2 + equipment.positionX * this.config.scale + width / 2;
                let yPos = wallHeight / 2 - equipment.positionY * this.config.scale - height / 2;
                let zPos = this.config.wallDepth / 2 + depth / 2;

                const equipmentMesh = BABYLON.MeshBuilder.CreateBox(equipment.nom, {
                    width: width,
                    height: height,
                    depth: depth
                }, scene);

                const material = new BABYLON.StandardMaterial(equipment.nom + "Material", scene);
                material.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
                equipmentMesh.material = material;

                equipmentMesh.position = new BABYLON.Vector3(-xPos, yPos, zPos);
                equipmentMesh.rotation.y = Math.PI;
                equipmentMesh.parent = wall;

                return equipmentMesh;
            }
        },

        renderSensor: function (scene, sensor, wall, wallWidth, wallHeight) {
            const width = sensor.longueur * this.config.scale;
            const height = sensor.hauteur * this.config.scale;
            const depth = this.config.equipmentDepth * 0.5;

            const sensorContainer = new BABYLON.TransformNode("sensorContainer", scene);

            // Boîtier principal du capteur
            const sensorBox = BABYLON.MeshBuilder.CreateBox("sensorBox", {
                width: width,
                height: height,
                depth: depth
            }, scene);

            const boxMaterial = new BABYLON.StandardMaterial("sensorBoxMaterial", scene);
            boxMaterial.diffuseColor = new BABYLON.Color3(0.9, 0.9, 0.9);
            boxMaterial.metallic = 0.3;
            boxMaterial.roughness = 0.7;
            sensorBox.material = boxMaterial;

            // Écran avec marge de 15%
            const marginRatio = 0.15;
            const screenWidth = width * (1 - 2 * marginRatio);
            const screenHeight = height * (1 - 2 * marginRatio);
            const screen = BABYLON.MeshBuilder.CreateBox("screen", {
                width: screenWidth,
                height: screenHeight,
                depth: 0.001
            }, scene);

            const screenMaterial = new BABYLON.StandardMaterial("screenMaterial", scene);
            if (sensor.estActif) {
                // Lorsque le capteur est actif, l'écran sera lumineux
                screenMaterial.diffuseColor = new BABYLON.Color3(0.1, 0.1, 0.1);  // Couleur diffuse sombre mais visible
                screenMaterial.emissiveColor = new BABYLON.Color3(0.2, 0.3, 0.4); // Écran lumineux (bleu-gris)
                screenMaterial.specularColor = new BABYLON.Color3(0.2, 0.2, 0.2); // Réflexions modérées
            } else {
                // Lorsque le capteur est inactif, l'écran devient noir (éteint)
                screenMaterial.diffuseColor = new BABYLON.Color3(0, 0, 0);  // Couleur diffuse noire (écran éteint)
                screenMaterial.emissiveColor = new BABYLON.Color3(0, 0, 0); // Pas de lumière émissive
                screenMaterial.specularColor = new BABYLON.Color3(0, 0, 0); // Pas de réflexions
            }


            screen.material = screenMaterial;

            // Décaler l'écran vers l'avant pour qu'il soit visible
            screen.position.z = depth * 0.5 + 0.01; // Décalage positif pour le faire ressortir légèrement du boîtier

            // Grille de ventilation
            const ventWidth = width * 0.6;
            const ventHeight = height * 0.15;
            const ventilation = BABYLON.MeshBuilder.CreatePlane("ventilation", {
                width: ventWidth,
                height: ventHeight
            }, scene);

            const ventMaterial = new BABYLON.StandardMaterial("ventMaterial", scene);
            ventMaterial.diffuseColor = new BABYLON.Color3(0.3, 0.3, 0.3);
            ventMaterial.alpha = 0.8;
            ventilation.material = ventMaterial;

            // Ajout de la LED avec taille minimum
            const minLedSize = 0.25; // Taille minimum de la LED en unités Babylon
            const proportionalLedSize = Math.min(width, height) * 0.15;
            const ledSize = Math.max(minLedSize, proportionalLedSize); // Prend la plus grande des deux valeurs
            const ledContainer = new BABYLON.TransformNode("ledContainer", scene);

            // Base de la LED (cylindre noir)
            const ledBase = BABYLON.MeshBuilder.CreateCylinder("ledBase", {
                height: depth * 0.3,
                diameter: ledSize,
                tessellation: 32
            }, scene);
            const ledBaseMaterial = new BABYLON.StandardMaterial("ledBaseMaterial", scene);
            ledBaseMaterial.diffuseColor = new BABYLON.Color3(0.2, 0.2, 0.2);
            ledBase.material = ledBaseMaterial;

            // Partie lumineuse de la LED (dôme)
            const ledDome = BABYLON.MeshBuilder.CreateSphere("ledDome", {
                diameter: ledSize,
                segments: 32,
                slice: 0.5
            }, scene);
            const ledMaterial = new BABYLON.PBRMaterial("ledMaterial", scene);
            ledMaterial.metallic = 0;
            ledMaterial.roughness = 0.3;
            ledMaterial.emissiveIntensity = 0.8;

            // Définir la couleur de la LED en fonction de estActif
            if (sensor.estActif) {
                ledMaterial.emissiveColor = new BABYLON.Color3(0.1, 0.8, 0.1); // Vert vif
                ledMaterial.albedoColor = new BABYLON.Color3(0.2, 1.0, 0.2);
            } else {
                ledMaterial.emissiveColor = new BABYLON.Color3(0.8, 0.1, 0.1); // Rouge vif
                ledMaterial.albedoColor = new BABYLON.Color3(1.0, 0.2, 0.2);
            }

            ledDome.material = ledMaterial;

            // Position de la LED
            ledDome.position.y = depth * 0.15;
            ledBase.parent = ledContainer;
            ledDome.parent = ledContainer;

            // Positionner la LED en haut à droite du capteur
            ledContainer.position = new BABYLON.Vector3(
                width * 0.35,
                height * 0.35,
                depth / 2 + 0.001
            );
            ledContainer.rotation = new BABYLON.Vector3(-Math.PI / 2, 0, 0);

            let xPos = -wallWidth / 2 + sensor.positionX * this.config.scale + width / 2;
            let yPos = wallHeight / 2 - sensor.positionY * this.config.scale - height / 2;
            let zPos = this.config.wallDepth / 2 + depth / 2;

            sensorContainer.position = new BABYLON.Vector3(-xPos, yPos, zPos);
            screen.position.z = depth * 0.5 - 0.10; // Décalage de l'écran pour qu'il soit visible
            ventilation.position = new BABYLON.Vector3(0, -height / 3, depth / 2 + 0.001);

            sensorContainer.rotation.y = Math.PI;
            screen.parent = sensorContainer;
            ventilation.parent = sensorContainer;
            sensorBox.parent = sensorContainer;
            ledContainer.parent = sensorContainer;
            sensorContainer.parent = wall;

            return sensorContainer;
        },


        renderRoom: function (scene, room, startPoint) {
            if (!room.murs || room.murs.length === 0) {
                return { walls: [], size: { width: 0, depth: 0 } };
            }

            let maxX = 0, maxZ = 0, currentX = 0, currentZ = 0;
            room.murs.forEach(wall => {
                const length = wall.longueur * this.config.scale;
                switch (wall.orientation) {
                    case 0: currentZ -= length; break;
                    case 1: currentX -= length; break;
                    case 2: currentZ += length; break;
                    case 3: currentX += length; break;
                }
                maxX = Math.max(Math.abs(currentX), maxX);
                maxZ = Math.max(Math.abs(currentZ), maxZ);
            });
            const roomSize = { width: maxX * 2, depth: maxZ * 2 };

            let currentPoint = new BABYLON.Vector3(startPoint.x, startPoint.y, startPoint.z);
            let walls = [];
            let previousEnd = currentPoint.clone();

            for (let i = 0; i < room.murs.length; i++) {
                const wallData = room.murs[i];
                const width = wallData.longueur * this.config.scale;
                const height = wallData.hauteur * this.config.scale;
                const wallContainer = new BABYLON.TransformNode("wallContainer_" + wallData.name, scene);
                const wallMesh = BABYLON.MeshBuilder.CreateBox("wall_" + wallData.name, {
                    width: width,
                    height: height,
                    depth: this.config.wallDepth
                }, scene);

                const innerMaterial = new BABYLON.StandardMaterial("wallInnerMaterial_" + i, scene);
                innerMaterial.diffuseColor = new BABYLON.Color3(0.96, 0.93, 0.86);
                const outerMaterial = new BABYLON.StandardMaterial("wallOuterMaterial_" + i, scene);
                outerMaterial.diffuseColor = new BABYLON.Color3(0.96, 0.93, 0.86);

                const multiMat = new BABYLON.MultiMaterial("multiMat_" + i, scene);
                multiMat.subMaterials = [innerMaterial, outerMaterial, outerMaterial, outerMaterial, outerMaterial, outerMaterial];
                wallMesh.material = multiMat;

                let mainCSG = BABYLON.CSG.FromMesh(wallMesh);

                const openings = (wallData.equipements || []).filter(eq => eq.typeEquipement &&
                    (eq.typeEquipement.nom.toLowerCase() === "fenetre" ||
                        eq.typeEquipement.nom.toLowerCase() === "vitre" ||
                        eq.typeEquipement.nom.toLowerCase() === "porte"));

                openings.forEach(opening => {
                    const openingWidth = opening.longueur * this.config.scale;
                    const openingHeight = opening.hauteur * this.config.scale;
                    const openingX = opening.positionX * this.config.scale;
                    const openingY = opening.positionY * this.config.scale;
                    const hole = BABYLON.MeshBuilder.CreateBox("hole", {
                        width: openingWidth,
                        height: openingHeight,
                        depth: this.config.wallDepth * 2
                    }, scene);

                    if (opening.typeEquipement.nom.toLowerCase() === "porte") {
                        hole.position = new BABYLON.Vector3(
                            width / 2 - openingX - openingWidth / 2,
                            height / 2 - openingY - openingHeight / 2,
                            0
                        );
                    } else {
                        hole.position = new BABYLON.Vector3(
                            -width / 2 + openingX + openingWidth / 2,
                            height / 2 - openingY - openingHeight / 2,
                            0
                        );
                    }

                    const holeCSG = BABYLON.CSG.FromMesh(hole);
                    mainCSG = mainCSG.subtract(holeCSG);
                    hole.dispose();
                });

                const finalWall = mainCSG.toMesh("finalWall", wallMesh.material, scene);
                finalWall.parent = wallContainer;
                wallMesh.dispose();

                let wallEndPosition = new BABYLON.Vector3(currentPoint.x, 0, currentPoint.z);

                switch (wallData.orientation) {
                    case 0:
                        wallContainer.rotation.y = -Math.PI / 2;
                        currentPoint.z -= width;
                        wallContainer.position = new BABYLON.Vector3(
                            startPoint.x + currentPoint.x + this.config.wallDepth / 2,
                            height / 2,
                            startPoint.z + (currentPoint.z + (currentPoint.z + width)) / 2
                        );
                        break;
                    case 1:
                        wallContainer.rotation.y = 0;
                        currentPoint.x -= width;
                        wallContainer.position = new BABYLON.Vector3(
                            startPoint.x + (currentPoint.x + (currentPoint.x + width)) / 2,
                            height / 2,
                            startPoint.z + currentPoint.z - this.config.wallDepth / 2
                        );
                        break;
                    case 2:
                        wallContainer.rotation.y = Math.PI / 2;
                        currentPoint.z += width;
                        wallContainer.position = new BABYLON.Vector3(
                            startPoint.x + currentPoint.x - this.config.wallDepth / 2,
                            height / 2,
                            startPoint.z + (currentPoint.z + (currentPoint.z - width)) / 2
                        );
                        break;
                    case 3:
                        wallContainer.rotation.y = Math.PI;
                        currentPoint.x += width;
                        wallContainer.position = new BABYLON.Vector3(
                            startPoint.x + (currentPoint.x + (currentPoint.x - width)) / 2,
                            height / 2,
                            startPoint.z + currentPoint.z + this.config.wallDepth / 2
                        );
                        break;
                }

                wallContainer.position.y = height / 2;
                previousEnd = wallEndPosition.clone();

                if (wallData.equipements) {
                    wallData.equipements.forEach(equipment => {
                        if (equipment.typeEquipement &&
                            (equipment.typeEquipement.nom.toLowerCase() === "fenetre" ||
                                equipment.typeEquipement.nom.toLowerCase() === "vitre")) {
                            this.renderEquipment(scene, equipment, wallContainer, width, height);
                        } else {
                            this.renderEquipment(scene, equipment, wallContainer, width, height);
                        }
                    });
                }

                if (wallData.capteurs) {
                    wallData.capteurs.forEach(sensor => {
                        this.renderSensor(scene, sensor, wallContainer, width, height);
                    });
                }
                walls.push(wallContainer);
            }

            return { walls, size: roomSize };
        },

        initializeScene: function (canvasId, buildingsDataJson) {
            const canvas = document.getElementById(canvasId);
            if (!canvas) throw new Error('Canvas not found');

            try {
                const parsedData = JSON.parse(buildingsDataJson);
                const engine = new BABYLON.Engine(canvas, true);
                window.currentEngine = engine;

                const scene = new BABYLON.Scene(engine);
                window.currentScene = scene;
                scene.clearColor = new BABYLON.Color3(0.8, 0.8, 0.8);

                let globalX = 0;

                let maxX = 0;    // Pour suivre la largeur maximale
                let currentY = 0; // Pour la position verticale


                parsedData.forEach((building, buildingIndex) => {

                    // Position de départ pour ce bâtiment
                    let currentX = 0;
                    const buildingStartZ = currentY;

                    // Trouver la largeur maximale des salles pour ce bâtiment
                    let maxRoomWidth = 0;
                    (building.salles || []).forEach((room) => {
                        // Calculer approximativement la largeur de la salle
                        const roomWidth = (room.murs || []).reduce((max, wall) => {
                            return (wall.orientation === 1 || wall.orientation === 3)
                                ? Math.max(max, wall.longueur * this.config.scale)
                                : max;
                        }, 0);
                        maxRoomWidth = Math.max(maxRoomWidth, roomWidth);
                    });

                    // Afficher les salles
                    (building.salles || []).forEach((room, roomIndex) => {
                        const roomPosition = new BABYLON.Vector3(
                            currentX,
                            0,
                            -currentY  // Note le Z négatif pour l'orientation correcte
                        );

                        const result = this.renderRoom(scene, room, roomPosition);

                        // Mettre à jour la position X pour la prochaine salle
                        currentX += maxRoomWidth * 1.1; // 10% d'espace entre les salles

                        // Si on dépasse une certaine largeur, on passe à la ligne suivante
                        if (currentX > 200) { // Vous pouvez ajuster cette valeur
                            currentX = 0;
                            currentY += 70;  // Espacement vertical entre les lignes
                        }

                        maxX = Math.max(maxX, currentX);
                    });

                    // Passer au bâtiment suivant en dessous
                    currentY += 70; // Espacement vertical entre les bâtiments
                });

                const camera = new BABYLON.ArcRotateCamera("camera",
                    0, Math.PI / 3, Math.max(maxX, currentY) * 1.5,
                    new BABYLON.Vector3(maxX / 2, 0, -currentY / 2),
                    scene
                );

                // Augmenter la vitesse de rotation et de zoom
                camera.angularSensibilityX = 750; // Diminuer pour une rotation plus rapide (défaut: 1000)
                camera.angularSensibilityY = 750; // Diminuer pour une rotation plus rapide (défaut: 1000)
                camera.pinchPrecision = 50;       // Ajuster la sensibilité du pinch-zoom
                camera.wheelPrecision = 5;       // Vitesse du zoom avec la molette (diminuer pour zoom plus rapide)
                camera.panningSensibility = 50;    // Augmenter la vitesse du panning (défaut: 1000)



                // Optionnel : Ajouter des limites pour éviter d'aller trop loin
                camera.lowerRadiusLimit = 1;     // Distance minimale de zoom
                camera.upperRadiusLimit = 15000;   // Distance maximale de zoom

                camera.attachControl(canvas, true);

                const light = new BABYLON.HemisphericLight("light",
                    new BABYLON.Vector3(0, 1, 0),
                    scene
                );
                light.intensity = 0.7;

                const dirLight = new BABYLON.DirectionalLight("dirLight",
                    new BABYLON.Vector3(-1, -2, -1),
                    scene
                );
                dirLight.intensity = 0.5;

                engine.runRenderLoop(() => scene.render());
                window.addEventListener("resize", () => engine.resize());

                return scene;

            } catch (error) {
                console.error("Error during scene initialization:", error);
                throw error;
            }
        },

        disposeScene: function () {
            if (window.currentScene) {
                if (window.currentEngine) {
                    window.currentEngine.stopRenderLoop();
                }
                window.currentScene.dispose();
                if (window.currentEngine) {
                    window.currentEngine.dispose();
                    window.currentEngine = null;
                }
                window.currentScene = null;
            }
        }
    });

})();