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
            const width = equipment.longueur * this.config.scale;
            const height = equipment.hauteur * this.config.scale;
            const depth = this.config.equipmentDepth;

            // Position de base, en partant du haut à gauche
            let xPos = -wallWidth / 2 + equipment.positionX * this.config.scale + width / 2;
            let yPos = wallHeight / 2 - equipment.positionY * this.config.scale - height / 2;
            let zPos = this.config.wallDepth / 2 + depth / 2;

            const equipmentMesh = BABYLON.MeshBuilder.CreateBox(equipment.nom, {
                width: width,
                height: height,
                depth: depth
            }, scene);

            const material = new BABYLON.StandardMaterial(equipment.nom + "Material", scene);
            if (equipment.typeEquipement && equipment.typeEquipement.nom.toLowerCase() === "radiateur") {
                material.diffuseColor = new BABYLON.Color3(0.7, 0.7, 0.7);
            } else if (equipment.typeEquipement && equipment.typeEquipement.nom.toLowerCase() === "fenetre") {
                material.diffuseColor = new BABYLON.Color3(0.8, 0.9, 1);
                material.alpha = 0.5;
            } else {
                material.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
            }
            equipmentMesh.material = material;

            // Position ajustée pour partir du coin gauche
            equipmentMesh.position = new BABYLON.Vector3(-xPos, yPos, zPos);
            equipmentMesh.rotation.y = Math.PI;

            equipmentMesh.parent = wall;
            return equipmentMesh;
        },

        renderSensor: function (scene, sensor, wall, wallWidth, wallHeight) {
            const width = sensor.longueur * this.config.scale;
            const height = sensor.hauteur * this.config.scale;
            const depth = this.config.equipmentDepth * 0.5;

            // Position de base, en partant du haut à gauche
            let xPos = -wallWidth / 2 + sensor.positionX * this.config.scale + width / 2;
            let yPos = wallHeight / 2 - sensor.positionY * this.config.scale - height / 2;
            let zPos = this.config.wallDepth / 2 + depth / 2;

            const sensorContainer = new BABYLON.TransformNode("sensor_" + sensor.nom, scene);

            const sensorBox = BABYLON.MeshBuilder.CreateBox("sensorBox", {
                width: width,
                height: height,
                depth: depth
            }, scene);

            const boxMaterial = new BABYLON.StandardMaterial("sensorBoxMaterial", scene);
            boxMaterial.diffuseColor = new BABYLON.Color3(0.2, 0.2, 0.2);
            sensorBox.material = boxMaterial;

            const screen = BABYLON.MeshBuilder.CreatePlane("screen", {
                width: width * 0.7,
                height: height * 0.7
            }, scene);

            const screenMaterial = new BABYLON.StandardMaterial("screenMaterial", scene);
            screenMaterial.emissiveColor = new BABYLON.Color3(0, 0.5, 0);
            screen.material = screenMaterial;

            // Position ajustée pour partir du coin gauche
            sensorContainer.position = new BABYLON.Vector3(-xPos, yPos, zPos);

            screen.position.z = depth / 2 + 0.001;
            screen.rotation.y = 0;

            sensorBox.parent = sensorContainer;
            screen.parent = sensorContainer;
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
                for (let i = 0; i < 6; i++) {
                    wall.subMeshes.push(new BABYLON.SubMesh(i, 0, verticesCount, i * 6, 6, wall));
                }

                const wallDepth = this.config.wallDepth;
                let offset = { x: 0, z: 0 };
                switch (wallData.orientation) {
                    case 0: offset = { x: wallDepth, z: 0 }; break;
                    case 1: offset = { x: 0, z: -wallDepth }; break;
                    case 2: offset = { x: -wallDepth, z: 0 }; break;
                    case 3: offset = { x: 0, z: wallDepth }; break;
                }

                let endPoint = new BABYLON.Vector3(
                    currentPoint.x + offset.x,
                    currentPoint.y,
                    currentPoint.z + offset.z
                );

                switch (wallData.orientation) {
                    case 0: endPoint.z -= width; break;
                    case 1: endPoint.x -= width; break;
                    case 2: endPoint.z += width; break;
                    case 3: endPoint.x += width; break;
                }

                wall.position.x = (currentPoint.x + endPoint.x) / 2;
                wall.position.z = (currentPoint.z + endPoint.z) / 2;
                wall.position.y = wallData.hauteur * this.config.scale / 2;

                switch (wallData.orientation) {
                    case 0: wall.rotation.y = -Math.PI / 2; break;
                    case 1: wall.rotation.y = 0; break;
                    case 2: wall.rotation.y = Math.PI / 2; break;
                    case 3: wall.rotation.y = Math.PI; break;
                }

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
                currentPoint = new BABYLON.Vector3(
                    endPoint.x - offset.x,
                    endPoint.y,
                    endPoint.z - offset.z
                );
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