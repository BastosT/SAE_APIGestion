(function () {
    console.log("Initializing wall chain builder with distinct inner/outer faces");

    window.babylonInterop = window.babylonInterop || {};

    Object.assign(window.babylonInterop, {
        config: {
            scale: 0.01,
            wallDepth: 0.10,
            debugPillarSize: 0.1
        },

        calculateCornerAdjustment: function (currentOrientation, nextOrientation) {
            const wallDepth = this.config.wallDepth;

            // Tableau des vecteurs de direction pour chaque orientation
            const directionVectors = {
                0: { x: 0, z: -1 },  // Nord
                1: { x: -1, z: 0 },  // Ouest
                2: { x: 0, z: 1 },   // Sud
                3: { x: 1, z: 0 }    // Est
            };

            // Obtenir les vecteurs de direction pour les murs actuels et suivants
            const currentDir = directionVectors[currentOrientation];
            const nextDir = directionVectors[nextOrientation];

            // Calculer l'angle entre les murs (sens horaire positif)
            const angle = Math.atan2(
                currentDir.x * nextDir.z - currentDir.z * nextDir.x,
                currentDir.x * nextDir.x + currentDir.z * nextDir.z
            );

            // Calculer le vecteur de décalage pour le coin
            const offsetX = wallDepth * (
                (Math.abs(currentDir.x) + Math.abs(nextDir.x)) / 2
            );
            const offsetZ = wallDepth * (
                (Math.abs(currentDir.z) + Math.abs(nextDir.z)) / 2
            );

            // Déterminer si nous sommes sur un coin intérieur ou extérieur
            const isInteriorCorner = (
                (currentOrientation + 1) % 4 === nextOrientation ||
                (currentOrientation === 3 && nextOrientation === 0)
            );

            // Ajuster le décalage en fonction du type de coin
            const adjustX = isInteriorCorner ? -offsetX : offsetX;
            const adjustZ = isInteriorCorner ? -offsetZ : offsetZ;

            return [adjustX, adjustZ];
        },

        debugWallPosition: function(startPoint, endPoint, orientation) {
        console.log(`Wall: ${orientation}`);
        console.log(`Start: (${startPoint.x.toFixed(3)}, ${startPoint.z.toFixed(3)})`);
        console.log(`End: (${endPoint.x.toFixed(3)}, ${endPoint.z.toFixed(3)})`);
        console.log('---');
    },
        calculateCornerPosition: function (startPoint, currentWall, nextWall) {
            const length = currentWall.longueur * this.config.scale;

            let endPoint = startPoint.clone();

            // Position de base selon l'orientation actuelle
            switch (currentWall.orientation) {
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

            // Si il y a un mur suivant, ajuster la position du coin
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
        calculateWallOffset: function (orientation) {
            // Calculate offset based on wall orientation and thickness
            const offset = this.config.wallDepth / 2;
            switch (orientation) {
                case 0: // Nord
                    return new BABYLON.Vector3(offset, 0, 0);
                case 1: // Ouest
                    return new BABYLON.Vector3(0, 0, -offset);
                case 2: // Sud
                    return new BABYLON.Vector3(-offset, 0, 0);
                case 3: // Est
                    return new BABYLON.Vector3(0, 0, offset);
                default:
                    return new BABYLON.Vector3(0, 0, 0);
            }
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

            // Créer les faces du mur séparément
            const faceNames = ['front', 'back', 'left', 'right', 'top', 'bottom'];
            const wall = BABYLON.MeshBuilder.CreateBox("wall_" + wallData.name, {
                width: width,
                height: height,
                depth: this.config.wallDepth,
                faceUV: new Array(6).fill(new BABYLON.Vector4(0, 0, 1, 1))
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

        calculateEndPoint: function (startPoint, wallData) {
            const length = wallData.longueur * this.config.scale;
            const wallOffset = this.calculateWallOffset(wallData.orientation);

            let endPoint = new BABYLON.Vector3(
                startPoint.x + wallOffset.x,
                startPoint.y,
                startPoint.z + wallOffset.z
            );

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

        positionWall: function (wall, wallData, startPoint, nextWall) {
            const endPoint = this.calculateCornerPosition(startPoint, wallData, nextWall);

            // Calculer le point central du mur
            wall.position.x = (startPoint.x + endPoint.x) / 2;
            wall.position.z = (startPoint.z + endPoint.z) / 2;
            wall.position.y = wallData.hauteur * this.config.scale / 2;

            // Orientation du mur
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

            console.log(`Wall positioned at:`, {
                startPoint: startPoint,
                endPoint: endPoint,
                finalPosition: wall.position,
                orientation: wallData.orientation
            });
        },

        renderRoom: function (scene, room, startPoint) {
            if (!room.murs || room.murs.length === 0) {
                console.log("No walls to render");
                return;
            }

            let currentPoint = startPoint.clone();
            let walls = [];
            let debugPillars = [];

            console.log("Starting room render at:", currentPoint);
            debugPillars.push(this.createDebugPillar(scene, currentPoint));

            for (let i = 0; i < room.murs.length; i++) {
                const wallData = room.murs[i];
                const nextWall = room.murs[i + 1];

                console.log(`Processing wall ${i}:`, wallData);

                const wall = this.createWall(scene, wallData);
                this.positionWall(wall, wallData, currentPoint, nextWall);
                walls.push(wall);

                currentPoint = this.calculateCornerPosition(currentPoint, wallData, nextWall);
                debugPillars.push(this.createDebugPillar(scene, currentPoint));

                console.log(`Next wall will start at:`, currentPoint);
            }

            return { walls, debugPillars };
        },

        initializeScene: function (canvasId, buildingsDataJson) {
            console.log("Initializing scene with sequential wall rendering");

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
                        console.log(`Rendering room ${roomIndex} at offset:`, roomOffset);
                        this.renderRoom(scene, room, roomOffset);
                        roomOffset.x += 15;
                    });

                    buildingOffset.z += 15;
                });

                const camera = new BABYLON.ArcRotateCamera("camera",
                    0,
                    Math.PI / 3,
                    20,
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