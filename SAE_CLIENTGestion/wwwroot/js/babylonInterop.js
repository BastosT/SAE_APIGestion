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
                    height: frameHeight - frameThickness * 2
                }, scene);

                const glassMaterial = new BABYLON.StandardMaterial("glassMaterial", scene);
                glassMaterial.diffuseColor = new BABYLON.Color3(0.8, 0.8, 1.0);
                glassMaterial.alpha = 0.3;
                glass.material = glassMaterial;
                glass.parent = frame;

                let xPos = -wallWidth / 2 + equipment.positionX * this.config.scale + frameWidth / 2;
                let yPos = wallHeight / 2 - equipment.positionY * this.config.scale - frameHeight / 2;
                let zPos = this.config.wallDepth / 2 + this.config.equipmentDepth / 2;

                frame.position = new BABYLON.Vector3(-xPos, yPos, zPos);
                frame.rotation.y = Math.PI;
                frame.parent = wall;
                return frame;
            }
            // Pour les radiateurs
            else if (equipment.typeEquipement && equipment.typeEquipement.nom.toLowerCase() === "radiateur") {
                const radiatorContainer = new BABYLON.TransformNode("radiatorContainer", scene);
                const width = equipment.longueur * this.config.scale;
                const height = equipment.hauteur * this.config.scale;
                const depth = this.config.equipmentDepth;
                const finSpacing = 0.03;
                const finThickness = 0.005;
                const finDepth = depth * 0.8;
                const numberOfFins = Math.floor(width / finSpacing);

                const frame = [
                    {
                        dimensions: { width: width, height: 0.02, depth: depth },
                        position: new BABYLON.Vector3(0, height / 2 - 0.01, 0)
                    },
                    {
                        dimensions: { width: width, height: 0.02, depth: depth },
                        position: new BABYLON.Vector3(0, -height / 2 + 0.01, 0)
                    },
                    {
                        dimensions: { width: 0.02, height: height, depth: depth },
                        position: new BABYLON.Vector3(-width / 2 + 0.01, 0, 0)
                    },
                    {
                        dimensions: { width: 0.02, height: height, depth: depth },
                        position: new BABYLON.Vector3(width / 2 - 0.01, 0, 0)
                    }
                ];

                frame.forEach((part, index) => {
                    const framePart = BABYLON.MeshBuilder.CreateBox(`radiatorFrame${index}`, part.dimensions, scene);
                    framePart.position = part.position;
                    const frameMaterial = new BABYLON.StandardMaterial(`radiatorFrameMaterial${index}`, scene);
                    frameMaterial.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
                    frameMaterial.metallic = 0.8;
                    frameMaterial.roughness = 0.3;
                    framePart.material = frameMaterial;
                    framePart.parent = radiatorContainer;
                });

                for (let i = 0; i < numberOfFins; i++) {
                    const fin = BABYLON.MeshBuilder.CreateBox(`radiatorFin${i}`, {
                        width: finThickness,
                        height: height - 0.04,
                        depth: finDepth
                    }, scene);
                    fin.position = new BABYLON.Vector3(
                        -width / 2 + 0.02 + (i * finSpacing),
                        0,
                        0
                    );
                    const finMaterial = new BABYLON.StandardMaterial(`radiatorFinMaterial${i}`, scene);
                    finMaterial.diffuseColor = new BABYLON.Color3(0.85, 0.85, 0.85);
                    finMaterial.metallic = 0.7;
                    finMaterial.roughness = 0.4;
                    fin.material = finMaterial;
                    fin.parent = radiatorContainer;
                }

                const pipeRadius = 0.01;
                const pipeHeight = 0.1;
                for (let i = 0; i < 2; i++) {
                    const pipe = BABYLON.MeshBuilder.CreateCylinder(`radiatorPipe${i}`, {
                        height: pipeHeight,
                        diameter: pipeRadius * 2
                    }, scene);
                    pipe.position = new BABYLON.Vector3(
                        (i === 0 ? -1 : 1) * (width / 4),
                        -height / 2 - pipeHeight / 2,
                        0
                    );
                    const pipeMaterial = new BABYLON.StandardMaterial(`radiatorPipeMaterial${i}`, scene);
                    pipeMaterial.diffuseColor = new BABYLON.Color3(0.7, 0.7, 0.7);
                    pipeMaterial.metallic = 0.9;
                    pipeMaterial.roughness = 0.2;
                    pipe.material = pipeMaterial;
                    pipe.parent = radiatorContainer;
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
                const wall = BABYLON.MeshBuilder.CreateBox("wall_" + wallData.name, {
                    width: width,
                    height: height,
                    depth: this.config.wallDepth
                }, scene);

                const innerMaterial = new BABYLON.StandardMaterial("wallInnerMaterial", scene);
                innerMaterial.diffuseColor = new BABYLON.Color3(0.2, 0.6, 1);
                const outerMaterial = new BABYLON.StandardMaterial("wallOuterMaterial", scene);
                outerMaterial.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
                const multiMat = new BABYLON.MultiMaterial("multiMat", scene);
                multiMat.subMaterials = [innerMaterial, outerMaterial, outerMaterial, outerMaterial, outerMaterial, outerMaterial];
                wall.material = multiMat;
                wall.subMeshes = [];
                let verticesCount = wall.getTotalVertices();
                for (let j = 0; j < 6; j++) {
                    wall.subMeshes.push(new BABYLON.SubMesh(j, 0, verticesCount, j * 6, 6, wall));
                }

                const wallDepth = this.config.wallDepth;

                // Calcul de la position du mur suivant l'orientation
                let endPoint = currentPoint.clone();
                switch (wallData.orientation) {
                    case 0:
                        wall.rotation.y = -Math.PI / 2;
                        endPoint.z -= width;
                        wall.position.x = currentPoint.x + wallDepth / 2;
                        wall.position.z = (currentPoint.z + endPoint.z) / 2;
                        break;
                    case 1:
                        wall.rotation.y = 0;
                        endPoint.x -= width;
                        wall.position.z = currentPoint.z - wallDepth / 2;
                        wall.position.x = (currentPoint.x + endPoint.x) / 2;
                        break;
                    case 2:
                        wall.rotation.y = Math.PI / 2;
                        endPoint.z += width;
                        wall.position.x = currentPoint.x - wallDepth / 2;
                        wall.position.z = (currentPoint.z + endPoint.z) / 2;
                        break;
                    case 3:
                        wall.rotation.y = Math.PI;
                        endPoint.x += width;
                        wall.position.z = currentPoint.z + wallDepth / 2;
                        wall.position.x = (currentPoint.x + endPoint.x) / 2;
                        break;
                }

                wall.position.y = wallData.hauteur * this.config.scale / 2;

                if (wallData.equipements) {
                    wallData.equipements.forEach(equipment => {
                        this.renderEquipment(scene, equipment, wall, width, height);
                    });
                }
                if (wallData.capteurs) {
                    wallData.capteurs.forEach(sensor => {
                        this.renderSensor(scene, sensor, wall, width, height);
                    });
                }

                walls.push(wall);
                currentPoint = endPoint;
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