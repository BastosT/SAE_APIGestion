(function () {
    console.log("Initializing wall chain builder with distinct inner/outer faces");

    window.babylonInterop = window.babylonInterop || {};

    Object.assign(window.babylonInterop, {
        config: {
            scale: 0.09,
            wallDepth: 0.30,

        },

        calculateRoomSize: function (room) {
            if (!room.murs || room.murs.length === 0) return { width: 0, depth: 0 };

            let maxX = 0;
            let maxZ = 0;
            let currentX = 0;
            let currentZ = 0;

            room.murs.forEach(wall => {
                const length = wall.longueur * this.config.scale;
                switch (wall.orientation) {
                    case 0: // North
                        currentZ -= length;
                        break;
                    case 1: // West
                        currentX -= length;
                        break;
                    case 2: // South
                        currentZ += length;
                        break;
                    case 3: // East
                        currentX += length;
                        break;
                }
                maxX = Math.max(Math.abs(currentX), maxX);
                maxZ = Math.max(Math.abs(currentZ), maxZ);
            });

            return {
                width: maxX * 2, // *2 car on veut la largeur totale
                depth: maxZ * 2  // *2 car on veut la profondeur totale
            };
        },
      

        createWall: function (scene, wallData) {
            const width = wallData.longueur * this.config.scale;
            const height = wallData.hauteur * this.config.scale;

            // Créer le mur avec des dimensions exactes
            const wall = BABYLON.MeshBuilder.CreateBox("wall_" + wallData.name, {
                width: width,
                height: height,
                depth: this.config.wallDepth
            }, scene);

            // Créer des matériaux distincts pour les faces intérieures et extérieures
            const innerMaterial = new BABYLON.StandardMaterial("wallInnerMaterial", scene);
            innerMaterial.diffuseColor = new BABYLON.Color3(0.2, 0.6, 1); // Bleu pour l'intérieur

            const outerMaterial = new BABYLON.StandardMaterial("wallOuterMaterial", scene);
            outerMaterial.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8); // Gris pour l'extérieur

            // Créer un matériau multi-faces
            const multiMat = new BABYLON.MultiMaterial("multiMat", scene);

            // Assigner les matériaux aux faces
            // L'ordre des faces dans Babylon.js est : front, back, left, right, top, bottom
            multiMat.subMaterials = [
                innerMaterial,  // front (intérieur)
                outerMaterial, // back (extérieur)
                outerMaterial, // left
                outerMaterial, // right
                outerMaterial, // top
                outerMaterial  // bottom
            ];

            // Assigner le matériau multi-faces au mur
            wall.material = multiMat;

            // Permettre d'avoir des matériaux différents sur chaque face
            wall.subMeshes = [];
            let verticesCount = wall.getTotalVertices();

            // Créer des sous-mailles pour chaque face
            for (let i = 0; i < 6; i++) {
                wall.subMeshes.push(new BABYLON.SubMesh(i, 0, verticesCount, i * 6, 6, wall));
            }

            return wall;
        },

        getWallOffset: function (orientation) {
            const wallDepth = this.config.wallDepth;
            // Simple offset toujours appliqué de la même manière
            switch (orientation) {
                case 0: // Nord
                    return { x: wallDepth, z: 0 };
                case 1: // Ouest
                    return { x: 0, z: -wallDepth };
                case 2: // Sud
                    return { x: -wallDepth, z: 0 };
                case 3: // Est
                    return { x: 0, z: wallDepth };
            }
        },

        calculateEndPoint: function (startPoint, wallData) {
            const length = wallData.longueur * this.config.scale;
            const offset = this.getWallOffset(wallData.orientation);

            // Clone le point de départ
            let endPoint = new BABYLON.Vector3(
                startPoint.x + offset.x,
                startPoint.y,
                startPoint.z + offset.z
            );

            // Application stricte de la longueur sans ajustement
            switch (wallData.orientation) {
                case 0: // Nord
                    endPoint.z -= length;
                    break;
                case 1: // Ouest
                    endPoint.x -= length;
                    break;
                case 2: // Sud
                    endPoint.z += length;
                    break;
                case 3: // Est
                    endPoint.x += length;
                    break;
            }

            return endPoint;
        },

        positionWall: function (wall, wallData, startPoint) {
            const endPoint = this.calculateEndPoint(startPoint, wallData);
            const offset = this.getWallOffset(wallData.orientation);

            // Positionner le mur exactement entre les points
            wall.position.x = (startPoint.x + endPoint.x) / 2;
            wall.position.z = (startPoint.z + endPoint.z) / 2;
            wall.position.y = wallData.hauteur * this.config.scale / 2;

            // Rotation standard
            switch (wallData.orientation) {
                case 0: // Nord
                    wall.rotation.y = -Math.PI / 2;
                    break;
                case 1: // Ouest
                    wall.rotation.y = 0;
                    break;
                case 2: // Sud
                    wall.rotation.y = Math.PI / 2;
                    break;
                case 3: // Est
                    wall.rotation.y = Math.PI;
                    break;
            }

            // Point de départ du prochain mur, sans offset
            return {
                x: endPoint.x - offset.x,
                y: endPoint.y,
                z: endPoint.z - offset.z
            };
        },

        renderRoom: function (scene, room, startPoint) {
            if (!room.murs || room.murs.length === 0) {
                console.log("No walls to render");
                return;
            }

            let currentPoint = new BABYLON.Vector3(startPoint.x, startPoint.y, startPoint.z);
            let walls = [];

            console.log("Starting room render at:", currentPoint);

            for (let i = 0; i < room.murs.length; i++) {
                const wallData = room.murs[i];
                console.log(`Processing wall ${i}:`, wallData);
                const wall = this.createWall(scene, wallData);
                const nextPoint = this.positionWall(wall, wallData, currentPoint);
                walls.push(wall);
                currentPoint = new BABYLON.Vector3(nextPoint.x, nextPoint.y, nextPoint.z);
                console.log(`Next wall will start at:`, currentPoint);
            }

            return { walls };
        },

        initializeScene: function (canvasId, buildingsDataJson) {
            console.log("Initializing scene with dynamic spacing");

            const canvas = document.getElementById(canvasId);
            if (!canvas) throw new Error('Canvas not found');

            try {
                const parsedData = JSON.parse(buildingsDataJson);

                const engine = new BABYLON.Engine(canvas, true);
                window.currentEngine = engine;

                const scene = new BABYLON.Scene(engine);
                window.currentScene = scene;
                scene.clearColor = new BABYLON.Color3(0.9, 0.9, 0.9);

                let buildingOffset = new BABYLON.Vector3(0, 0, 0);

                parsedData.forEach((building, buildingIndex) => {
                    let roomOffset = buildingOffset.clone();

                    (building.salles || []).forEach((room, roomIndex) => {
                        const roomSize = this.calculateRoomSize(room);
                        console.log(`Rendering room ${roomIndex} at offset:`, roomOffset);
                        this.renderRoom(scene, room, roomOffset);
                        roomOffset.x += roomSize.width * 1.2;
                    });

                    buildingOffset.z += 20; // Espacement fixe entre les bâtiments
                });

                // Camera setup
                const camera = new BABYLON.ArcRotateCamera("camera",
                    0,
                    Math.PI / 3,
                    50, // Distance fixe qui peut être ajustée selon vos besoins
                    new BABYLON.Vector3(0, 0, 0),
                    scene
                );
                camera.attachControl(canvas, true);

                // Lighting setup
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