(function () {
    console.log("Initializing wall chain builder with distinct inner/outer faces");

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
            screenMaterial.diffuseColor = new BABYLON.Color3(0.1, 0.1, 0.1);
            screenMaterial.emissiveColor = new BABYLON.Color3(0.2, 0.3, 0.4);
            screenMaterial.specularColor = new BABYLON.Color3(0.2, 0.2, 0.2);
            screen.material = screenMaterial;

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

            let xPos = -wallWidth / 2 + sensor.positionX * this.config.scale + width / 2;
            let yPos = wallHeight / 2 - sensor.positionY * this.config.scale - height / 2;
            let zPos = this.config.wallDepth / 2 + depth / 2;

            sensorContainer.position = new BABYLON.Vector3(-xPos, yPos, zPos);
            screen.position.z = depth / 2 + 0.001;
            ventilation.position = new BABYLON.Vector3(0, -height / 3, depth / 2 + 0.001);

            sensorContainer.rotation.y = Math.PI;
            screen.parent = sensorContainer;
            ventilation.parent = sensorContainer;
            sensorBox.parent = sensorContainer;
            sensorContainer.parent = wall;

            return sensorContainer;
        },
        renderRoom: function (scene, room, startPoint) {
            if (!room.murs || room.murs.length === 0) {
                console.log("No walls to render");
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

            for (let i = 0; i < room.murs.length; i++) {
                const wallData = room.murs[i];
                const width = wallData.longueur * this.config.scale;
                const height = wallData.hauteur * this.config.scale;

                const wallContainer = new BABYLON.TransformNode("wallContainer_" + wallData.name, scene);

                // Créer le mur principal complet
                const wallMesh = BABYLON.MeshBuilder.CreateBox("wall_" + wallData.name, {
                    width: width,
                    height: height,
                    depth: this.config.wallDepth
                }, scene);

                // Appliquer les matériaux
                const innerMaterial = new BABYLON.StandardMaterial("wallInnerMaterial_" + i, scene);
                innerMaterial.diffuseColor = new BABYLON.Color3(0.96, 0.93, 0.86);
                const outerMaterial = new BABYLON.StandardMaterial("wallOuterMaterial_" + i, scene);
                outerMaterial.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);

                const multiMat = new BABYLON.MultiMaterial("multiMat_" + i, scene);
                multiMat.subMaterials = [innerMaterial, outerMaterial, outerMaterial, outerMaterial, outerMaterial, outerMaterial];
                wallMesh.material = multiMat;

                // CSG pour créer les trous pour les fenêtres/vitres
                let mainCSG = BABYLON.CSG.FromMesh(wallMesh);

                // Filtrer les fenêtres et vitres
                const openings = (wallData.equipements || [])
                    .filter(eq => eq.typeEquipement &&
                        (eq.typeEquipement.nom.toLowerCase() === "fenetre" ||
                            eq.typeEquipement.nom.toLowerCase() === "vitre"));

                // Créer les trous pour chaque ouverture
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

                    hole.position = new BABYLON.Vector3(
                        -width / 2 + openingX + openingWidth / 2,
                        height / 2 - openingY - openingHeight / 2,
                        0
                    );

                    const holeCSG = BABYLON.CSG.FromMesh(hole);
                    mainCSG = mainCSG.subtract(holeCSG);
                    hole.dispose();
                });

                // Convertir le CSG final en mesh
                const finalWall = mainCSG.toMesh("finalWall", wallMesh.material, scene);
                finalWall.parent = wallContainer;

                // Nettoyer le mesh original
                wallMesh.dispose();

                // Positionner le mur selon son orientation
                switch (wallData.orientation) {
                    case 0:
                        wallContainer.rotation.y = -Math.PI / 2;
                        currentPoint.z -= width;
                        wallContainer.position.x = currentPoint.x + this.config.wallDepth / 2;
                        wallContainer.position.z = (currentPoint.z + (currentPoint.z + width)) / 2;
                        break;
                    case 1:
                        wallContainer.rotation.y = 0;
                        currentPoint.x -= width;
                        wallContainer.position.z = currentPoint.z - this.config.wallDepth / 2;
                        wallContainer.position.x = (currentPoint.x + (currentPoint.x + width)) / 2;
                        break;
                    case 2:
                        wallContainer.rotation.y = Math.PI / 2;
                        currentPoint.z += width;
                        wallContainer.position.x = currentPoint.x - this.config.wallDepth / 2;
                        wallContainer.position.z = (currentPoint.z + (currentPoint.z - width)) / 2;
                        break;
                    case 3:
                        wallContainer.rotation.y = Math.PI;
                        currentPoint.x += width;
                        wallContainer.position.z = currentPoint.z + this.config.wallDepth / 2;
                        wallContainer.position.x = (currentPoint.x + (currentPoint.x - width)) / 2;
                        break;
                }

                wallContainer.position.y = height / 2;

                // Ajouter les équipements (y compris les fenêtres et vitres)
                if (wallData.equipements) {
                    wallData.equipements.forEach(equipment => {
                        if (equipment.typeEquipement &&
                            (equipment.typeEquipement.nom.toLowerCase() === "fenetre" ||
                                equipment.typeEquipement.nom.toLowerCase() === "vitre")) {
                            // Rendre la fenêtre/vitre avec les mêmes coordonnées que son trou
                            this.renderEquipment(scene, equipment, wallContainer, width, height);
                        } else {
                            // Rendre les autres équipements normalement
                            this.renderEquipment(scene, equipment, wallContainer, width, height);
                        }
                    });
                }

                // Ajouter les capteurs
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
                console.log("Parsed buildings data:", parsedData);
                const engine = new BABYLON.Engine(canvas, true);
                window.currentEngine = engine;

                const scene = new BABYLON.Scene(engine);
                window.currentScene = scene;
                scene.clearColor = new BABYLON.Color3(0.9, 0.9, 0.9);

                let buildingOffset = new BABYLON.Vector3(0, 0, 0);

                parsedData.forEach((building, buildingIndex) => {
                    let roomOffset = buildingOffset.clone();

                    (building.salles || []).forEach((room, roomIndex) => {
                        const result = this.renderRoom(scene, room, roomOffset);
                        roomOffset.x += result.size.width * 1.2;
                    });

                    buildingOffset.z += 20;
                });

                const camera = new BABYLON.ArcRotateCamera("camera",
                    0, Math.PI / 3, 50,
                    new BABYLON.Vector3(0, 0, 0),
                    scene
                );
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

    console.log("Wall chain builder initialization complete");
})();