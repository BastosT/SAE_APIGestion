(function () {
    console.log("Initializing wall chain builder with distinct inner/outer faces");

    window.babylonInterop = window.babylonInterop || {};

    Object.assign(window.babylonInterop, {
        config: {
            scale: 0.09,
            wallDepth: 0.10,
            debugPillarSize: 0.1,

        },

        isInteriorCorner: function (currentOrientation, nextOrientation) {
            return (currentOrientation + 1) % 4 === nextOrientation ||
                (currentOrientation === 3 && nextOrientation === 0);
        },

        calculateCornerAdjustment: function (currentOrientation, nextOrientation) {
            const wallDepth = this.config.wallDepth;

            // Direction vectors for each orientation
            const directionVectors = {
                0: { x: 0, z: -1 },  // North
                1: { x: -1, z: 0 },  // West
                2: { x: 0, z: 1 },   // South
                3: { x: 1, z: 0 }    // East
            };

            const currentDir = directionVectors[currentOrientation];
            const nextDir = directionVectors[nextOrientation];

            // Calculate angle between walls (clockwise positive)
            const angle = Math.atan2(
                currentDir.x * nextDir.z - currentDir.z * nextDir.x,
                currentDir.x * nextDir.x + currentDir.z * nextDir.z
            );

            // Determine if we're dealing with an interior or exterior corner
            const isInteriorCorner = (
                (currentOrientation + 1) % 4 === nextOrientation ||
                (currentOrientation === 3 && nextOrientation === 0)
            );

            // Calculate adjustment for the current wall end
            let adjustX = 0;
            let adjustZ = 0;

            if (isInteriorCorner) {
                // Interior corner adjustments
                switch (currentOrientation) {
                    case 0: // North
                        adjustX = -wallDepth;
                        break;
                    case 1: // West
                        adjustZ = -wallDepth;
                        break;
                    case 2: // South
                        adjustX = wallDepth;
                        break;
                    case 3: // East
                        adjustZ = wallDepth;
                        break;
                }
            } else {
                // Exterior corner adjustments
                switch (currentOrientation) {
                    case 0: // North
                        adjustX = wallDepth;
                        break;
                    case 1: // West
                        adjustZ = wallDepth;
                        break;
                    case 2: // South
                        adjustX = -wallDepth;
                        break;
                    case 3: // East
                        adjustZ = -wallDepth;
                        break;
                }
            }

            return [adjustX, adjustZ];
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
        calculateBuildingSize: function (building) {
            let maxWidth = 0;
            let totalDepth = 0;

            (building.salles || []).forEach(room => {
                const roomSize = this.calculateRoomSize(room);
                maxWidth = Math.max(maxWidth, roomSize.width);
                totalDepth += roomSize.depth;
            });

            return {
                width: maxWidth,
                depth: totalDepth
            };
        },

        debugWallPosition: function (startPoint, endPoint, orientation) {
            console.log(`Wall: ${orientation}`);
            console.log(`Start: (${startPoint.x.toFixed(3)}, ${startPoint.z.toFixed(3)})`);
            console.log(`End: (${endPoint.x.toFixed(3)}, ${endPoint.z.toFixed(3)})`);
            console.log('---');
        },

        calculateCornerPosition: function (startPoint, currentWall, nextWall) {
            const length = currentWall.longueur * this.config.scale;
            let endPoint = startPoint.clone();

            // Base position according to wall orientation
            switch (currentWall.orientation) {
                case 0: // North
                    endPoint.z -= length;
                    break;
                case 1: // West
                    endPoint.x -= length;
                    break;
                case 2: // South
                    endPoint.z += length;
                    break;
                case 3: // East
                    endPoint.x += length;
                    break;
            }

            // Apply corner adjustment if there's a next wall
            if (nextWall) {
                const [adjustX, adjustZ] = this.calculateCornerAdjustment(
                    currentWall.orientation,
                    nextWall.orientation
                );
                endPoint.x += adjustX;
                endPoint.z += adjustZ;
            }

            return endPoint;
        },

        calculateWallOffset: function (orientation, previousWall) {
            const offset = this.config.wallDepth / 2;
            let adjustedOffset = new BABYLON.Vector3(0, 0, 0);

            // Base offset based on current wall orientation
            switch (orientation) {
                case 0: // North
                    adjustedOffset.x = offset;
                    break;
                case 1: // West
                    adjustedOffset.z = -offset;
                    break;
                case 2: // South
                    adjustedOffset.x = -offset;
                    break;
                case 3: // East
                    adjustedOffset.z = offset;
                    break;
            }

            // If there's a previous wall, adjust the starting position
            if (previousWall) {
                const isInteriorCorner = (
                    (previousWall.orientation + 1) % 4 === orientation ||
                    (previousWall.orientation === 3 && orientation === 0)
                );

                if (isInteriorCorner) {
                    // Adjust the offset for interior corners
                    switch (previousWall.orientation) {
                        case 0: // Previous wall facing North
                            adjustedOffset.x += this.config.wallDepth;
                            break;
                        case 1: // Previous wall facing West
                            adjustedOffset.z += this.config.wallDepth;
                            break;
                        case 2: // Previous wall facing South
                            adjustedOffset.x -= this.config.wallDepth;
                            break;
                        case 3: // Previous wall facing East
                            adjustedOffset.z -= this.config.wallDepth;
                            break;
                    }
                }
            }

            return adjustedOffset;
        },
        createDebugPillar: function (scene, position) {
            const pillar = BABYLON.MeshBuilder.CreateBox("debugPillar", {
                height: 3,
                width: this.config.debugPillarSize,
                depth: this.config.debugPillarSize
            }, scene);

            const material = new BABYLON.StandardMaterial("pillarMaterial", scene);
            material.diffuseColor = new BABYLON.Color3(1, 0, 0);
            pillar.material = material;

            pillar.position = position.clone();
            pillar.position.y = 1.5;

            return pillar;
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
            let debugPillars = [];

            console.log("Starting room render at:", currentPoint);
            debugPillars.push(this.createDebugPillar(scene, currentPoint));

            for (let i = 0; i < room.murs.length; i++) {
                const wallData = room.murs[i];

                console.log(`Processing wall ${i}:`, wallData);

                const wall = this.createWall(scene, wallData);
                const nextPoint = this.positionWall(wall, wallData, currentPoint);
                walls.push(wall);

                currentPoint = new BABYLON.Vector3(nextPoint.x, nextPoint.y, nextPoint.z);
                debugPillars.push(this.createDebugPillar(scene, currentPoint));

                console.log(`Next wall will start at:`, currentPoint);
            }

            return { walls, debugPillars };
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
                    const buildingSize = this.calculateBuildingSize(building);
                    let roomOffset = buildingOffset.clone();

                    (building.salles || []).forEach((room, roomIndex) => {
                        const roomSize = this.calculateRoomSize(room);
                        console.log(`Rendering room ${roomIndex} at offset:`, roomOffset);
                        this.renderRoom(scene, room, roomOffset);
                        // Ajoute la taille de la pièce plus un petit espace (20% de la taille)
                        roomOffset.x += roomSize.width * 1.2;
                    });

                    // Ajoute la taille du bâtiment plus un petit espace (20% de la taille)
                    buildingOffset.z += buildingSize.depth * 1.2;
                });

                // Ajuste la caméra en fonction de la taille totale de la scène
                const camera = new BABYLON.ArcRotateCamera("camera",
                    0,
                    Math.PI / 3,
                    Math.max(buildingOffset.x, buildingOffset.z) * 2, // Distance ajustée automatiquement
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