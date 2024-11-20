
var babylonInterop = {
    config: {
        scale: 0.01,
        wallDepth: 0.10,
        equipmentDepth: 0.1,
        floorHeight: 0.1,
        buildingSpacing: 0.10, 
        roomSpacing: 0.0035,
        buildingWallThickness: 0.3
    },
    sharedMaterials: {
        inside: null,
        outside: null
    },

    initSharedMaterials: function (scene) {
        const baseProperties = {
            diffuseColor: new BABYLON.Color3(0.725, 0.608, 0.486), // Couleur murs
            specularColor: new BABYLON.Color3(0.1, 0.1, 0.1),
            roughness: 0.8,
            backFaceCulling: true
        };

        if (!this.sharedMaterials.inside) {
            this.sharedMaterials.inside = new BABYLON.StandardMaterial("sharedInside", scene);
            Object.assign(this.sharedMaterials.inside, baseProperties);
        }

        if (!this.sharedMaterials.outside) {
            this.sharedMaterials.outside = new BABYLON.StandardMaterial("sharedOutside", scene);
            Object.assign(this.sharedMaterials.outside, baseProperties);
            this.sharedMaterials.outside.alpha = 0.3;
            this.sharedMaterials.outside.transparencyMode = BABYLON.Material.MATERIAL_ALPHABLEND;
        }
    },

    createWallWithHoles: function (scene, wallData) {
        const { scale, wallDepth } = this.config;
        const width = wallData.largeur * scale;
        const height = wallData.hauteur * scale;
        const wallContainer = new BABYLON.TransformNode("wallContainer", scene);
        const doorMargin = 0.1;

        // Séparation et tri des équipements
        const openings = (wallData.equipements?.filter(e => e.type <= 2) || [])
            .map(opening => ({
                ...opening,
                largeur: opening.type === 2 ? opening.largeur + (doorMargin * 4 / scale) : opening.largeur,
                positionX: opening.type === 2 ? opening.positionX - (doorMargin * 2 / scale) : opening.positionX,
            }))
            .sort((a, b) => a.positionX - b.positionX);

        const otherEquipments = wallData.equipements?.filter(e => e.type > 2) || [];

        let currentX = 0;
        for (let i = 0; i <= openings.length; i++) {
            // Section de mur principale
            const sectionWidth = i === openings.length ?
                width - currentX :
                (openings[i].positionX * scale) - currentX;

            if (sectionWidth > 0) {
                this.createWallSection(scene, {
                    width: sectionWidth,
                    height,
                    depth: wallDepth,
                    x: -width / 2 + currentX + sectionWidth / 2,
                    name: `wallSection${i}`
                }, wallContainer);
            }

            if (i < openings.length) {
                const opening = openings[i];
                const openingWidth = opening.largeur * scale;
                const openingHeight = opening.hauteur * scale;
                const openingY = opening.positionY * scale;
                const openingX = -width / 2 + opening.positionX * scale + openingWidth / 2;

                // Section au-dessus de l'ouverture
                if (openingY > 0) {
                    this.createWallSection(scene, {
                        width: openingWidth,
                        height: openingY,
                        depth: wallDepth,
                        x: openingX,
                        y: height / 2 - openingY / 2,
                        name: `wallTopSection${i}`
                    }, wallContainer);
                }

                // Section du bas pour les fenêtres
                if (opening.type !== 2) {
                    const bottomHeight = height - openingY - openingHeight;
                    if (bottomHeight > 0) {
                        this.createWallSection(scene, {
                            width: openingWidth,
                            height: bottomHeight,
                            depth: wallDepth,
                            x: openingX,
                            y: -height / 2 + bottomHeight / 2,
                            name: `wallBottomSection${i}`
                        }, wallContainer);
                    }
                }

                // Cadre
                const frame = opening.type === 2 ?
                    this.createDoorFrame(scene, opening, width, height) :
                    this.createWindowFrame(scene, opening, width, height);
                frame.parent = wallContainer;

                currentX = (opening.positionX + opening.largeur) * scale;
            }
        }

        // Autres équipements
        otherEquipments.forEach(equipment => {
            const mesh = this.createEquipment(scene, equipment, wallContainer, width, height);
            if (mesh) mesh.parent = wallContainer;
        });

        return wallContainer;
    },

    createWallSection: function (scene, params, parent) {

        this.initSharedMaterials(scene);


        const section = BABYLON.MeshBuilder.CreateBox(params.name, {
            width: params.width,
            height: params.height,
            depth: params.depth
        }, scene);

        // Positionner la section
        section.position.x = params.x;
        if (params.y) section.position.y = params.y;

        // S'assurer que le clone est créé et configuré correctement
        const sectionOutside = section.clone("outside" + params.name);

        // Appliquer explicitement les matériaux
        section.material = this.sharedMaterials.inside;
        sectionOutside.material = this.sharedMaterials.outside;

        // S'assurer que la face externe est correctement inversée
        sectionOutside.scaling.z = -1;

        section.parent = parent;
        sectionOutside.parent = parent;
    },

    createDoorFrame: function (scene, door, wallWidth, wallHeight) {
        const doorMargin = 0.005;
        const actualWidth = door.largeur * this.config.scale;
        const actualHeight = door.hauteur * this.config.scale;
        const frameWidth = actualWidth - (doorMargin * 2);
        const frameThickness = 0.015;
        const frame = new BABYLON.TransformNode("doorFrame", scene);

        const parts = [
            {
                dimensions: {
                    width: frameWidth + frameThickness * 2,
                    height: frameThickness,
                    depth: this.config.wallDepth
                },
                position: new BABYLON.Vector3(0, actualHeight / 2 - frameThickness / 2, 0)
            },
            {
                dimensions: {
                    width: frameThickness,
                    height: actualHeight,
                    depth: this.config.wallDepth
                },
                position: new BABYLON.Vector3(-frameWidth / 2 - frameThickness / 2, 0, 0)
            },
            {
                dimensions: {
                    width: frameThickness,
                    height: actualHeight,
                    depth: this.config.wallDepth
                },
                position: new BABYLON.Vector3(frameWidth / 2 + frameThickness / 2, 0, 0)
            }
        ];

        const frameMaterial = new BABYLON.StandardMaterial("doorFrameMaterial", scene);
        frameMaterial.diffuseColor = new BABYLON.Color3(0.4, 0.2, 0.1);

        parts.forEach((part, index) => {
            const mesh = BABYLON.MeshBuilder.CreateBox("doorFramePart" + index, part.dimensions, scene);
            mesh.position = part.position;
            mesh.material = frameMaterial;
            mesh.parent = frame;
        });

        const door_panel = BABYLON.MeshBuilder.CreateBox("doorPanel", {
            width: frameWidth,
            height: actualHeight - frameThickness * 2,
            depth: frameThickness
        }, scene);

        door_panel.material = frameMaterial;
        door_panel.parent = frame;
        door_panel.position.z = this.config.wallDepth / 4;

        frame.position = new BABYLON.Vector3(
            -wallWidth / 2 + door.positionX * this.config.scale + actualWidth / 2,
            wallHeight / 2 - door.positionY * this.config.scale - actualHeight / 2,
            0
        );

        return frame;
    },

    createWindowFrame: function (scene, window, wallWidth, wallHeight) {
        const frameWidth = window.largeur * this.config.scale;
        const frameHeight = window.hauteur * this.config.scale;
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

        frame.position = new BABYLON.Vector3(
            -wallWidth / 2 + window.positionX * this.config.scale + frameWidth / 2,
            wallHeight / 2 - window.positionY * this.config.scale - frameHeight / 2,
            0
        );

        return frame;
    },

    createEquipment: function (scene, equipment, parentWall, wallWidth, wallHeight) {
        if (equipment.type === 0 || equipment.type === 1) return null;

        if (equipment.type === 3) { // Radiateur
            const radiatorContainer = new BABYLON.TransformNode("radiatorContainer", scene);

            const width = equipment.largeur * this.config.scale;
            const height = equipment.hauteur * this.config.scale;
            const depth = this.config.equipmentDepth;

            // Paramètres pour les ailettes
            const finSpacing = 0.03; // 3cm entre chaque ailette
            const finThickness = 0.005; // 5mm d'épaisseur pour chaque ailette
            const finDepth = depth * 0.8; // Profondeur légèrement réduite pour les ailettes
            const numberOfFins = Math.floor(width / finSpacing);

            // Création du cadre principal du radiateur
            const frame = [
                // Support supérieur
                {
                    dimensions: { width: width, height: 0.02, depth: depth },
                    position: new BABYLON.Vector3(0, height / 2 - 0.01, 0)
                },
                // Support inférieur
                {
                    dimensions: { width: width, height: 0.02, depth: depth },
                    position: new BABYLON.Vector3(0, -height / 2 + 0.01, 0)
                },
                // Support vertical gauche
                {
                    dimensions: { width: 0.02, height: height, depth: depth },
                    position: new BABYLON.Vector3(-width / 2 + 0.01, 0, 0)
                },
                // Support vertical droit
                {
                    dimensions: { width: 0.02, height: height, depth: depth },
                    position: new BABYLON.Vector3(width / 2 - 0.01, 0, 0)
                }
            ];

            // Création du cadre
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

            // Création des ailettes
            for (let i = 0; i < numberOfFins; i++) {
                const fin = BABYLON.MeshBuilder.CreateBox(`radiatorFin${i}`, {
                    width: finThickness,
                    height: height - 0.04, // Hauteur légèrement réduite pour tenir dans le cadre
                    depth: finDepth
                }, scene);

                // Position de l'ailette
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

            // Création des tuyaux de connexion
            const pipeRadius = 0.01; // 1cm de rayon
            const pipeHeight = 0.1; // 10cm de hauteur

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

            // Position globale du radiateur
            const xPos = equipment.positionX * this.config.scale;
            const yPos = equipment.positionY * this.config.scale;

            radiatorContainer.position = new BABYLON.Vector3(
                -wallWidth / 2 + xPos + width / 2,
                wallHeight / 2 - yPos - height / 2,
                -(this.config.wallDepth / 2 + depth / 2)
            );

            radiatorContainer.parent = parentWall;
            return radiatorContainer;

        } else if (equipment.type === 4) { // Capteur
            // Code existant pour les capteurs...
            const sensorContainer = new BABYLON.TransformNode("sensorContainer", scene);

            const sensorWidth = 0.12;
            const sensorHeight = 0.08;
            const sensorDepth = 0.03;

            const sensorBox = BABYLON.MeshBuilder.CreateBox("sensorBox", {
                width: sensorWidth,
                height: sensorHeight,
                depth: sensorDepth,
                updatable: false
            }, scene);

            const boxMaterial = new BABYLON.StandardMaterial("sensorBoxMaterial", scene);
            boxMaterial.diffuseColor = new BABYLON.Color3(0.9, 0.9, 0.9);
            boxMaterial.metallic = 0.3;
            boxMaterial.roughness = 0.7;
            sensorBox.material = boxMaterial;

            const ventWidth = sensorWidth * 0.7;
            const ventHeight = sensorHeight * 0.3;
            const ventilation = BABYLON.MeshBuilder.CreatePlane("ventilation", {
                width: ventWidth,
                height: ventHeight
            }, scene);

            const ventMaterial = new BABYLON.StandardMaterial("ventMaterial", scene);
            ventMaterial.diffuseColor = new BABYLON.Color3(0.7, 0.7, 0.7);
            ventMaterial.alpha = 0.8;
            ventilation.material = ventMaterial;
            ventilation.position = new BABYLON.Vector3(0, 0, sensorDepth / 2 + 0.001);
            ventilation.parent = sensorBox;

            const ledSize = 0.005;
            const led = BABYLON.MeshBuilder.CreateBox("led", {
                width: ledSize,
                height: ledSize,
                depth: ledSize
            }, scene);

            const ledMaterial = new BABYLON.StandardMaterial("ledMaterial", scene);
            ledMaterial.emissiveColor = new BABYLON.Color3(0.0, 0.7, 0);
            ledMaterial.specularColor = new BABYLON.Color3(0, 0, 0);
            led.material = ledMaterial;

            led.position = new BABYLON.Vector3(
                sensorWidth / 4,
                -sensorHeight / 3,
                sensorDepth / 2 + 0.001
            );
            led.parent = sensorBox;

            const xPos = equipment.positionX * this.config.scale;
            const yPos = equipment.positionY * this.config.scale;

            sensorBox.position = new BABYLON.Vector3(
                -wallWidth / 2 + xPos + sensorWidth / 2,
                wallHeight / 2 - yPos - sensorHeight / 2,
                -(this.config.wallDepth / 2 + sensorDepth / 2)
            );

            sensorBox.parent = parentWall;
            return sensorContainer;
        } else {
            // Code existant pour les autres équipements...
            const equipmentMesh = BABYLON.MeshBuilder.CreateBox(equipment.nom, {
                width: width,
                height: height,
                depth: depth,
                updatable: false
            }, scene);

            const xPos = equipment.positionX * this.config.scale;
            const yPos = equipment.positionY * this.config.scale;

            equipmentMesh.position = new BABYLON.Vector3(
                -wallWidth / 2 + xPos + width / 2,
                wallHeight / 2 - yPos - height / 2,
                -(this.config.wallDepth / 2 + depth / 2)
            );

            const material = new BABYLON.StandardMaterial(equipment.nom + "Material", scene);
            material.diffuseColor = new BABYLON.Color3(0.9, 0.9, 0.9);

            equipmentMesh.material = material;
            equipmentMesh.parent = parentWall;
            return equipmentMesh;
        }
    },

    createFloor: function (scene, width, depth, position, wallHeight) {
        const floor = BABYLON.MeshBuilder.CreateBox("floor", {
            width: width + (this.config.wallDepth * 2),
            height: this.config.floorHeight,
            depth: depth + (this.config.wallDepth * 2)
        }, scene);

        const scaledHeight = wallHeight * this.config.scale;

        floor.position = new BABYLON.Vector3(
            position.x,
            -(scaledHeight / 2) - (this.config.floorHeight / 2),
            position.z
        );

        const floorMaterial = new BABYLON.StandardMaterial("floorMaterial", scene);
        floorMaterial.diffuseColor = new BABYLON.Color3(0.4, 0.4, 0.4);
        floorMaterial.roughness = 0.9;
        floor.material = floorMaterial;

        return floor;
    },

    createCorner: function (scene, height, position, name) {
        this.initSharedMaterials(scene);
        const cornerContainer = new BABYLON.TransformNode(name, scene);

        const corner = BABYLON.MeshBuilder.CreateBox(name + "Inside", {
            width: this.config.wallDepth,
            height: height,
            depth: this.config.wallDepth
        }, scene);

        const cornerOutside = corner.clone(name + "Outside");

        const cornerMaterial = new BABYLON.StandardMaterial(name + "Material", scene);
        cornerMaterial.diffuseColor = new BABYLON.Color3(0.725, 0.608, 0.486); // Darker beige for corners
        corner.material = cornerMaterial;
        cornerOutside.material = cornerMaterial;

        cornerOutside.scaling = new BABYLON.Vector3(-1, 1, -1);

        corner.position = position;
        cornerOutside.position = position;

        corner.parent = cornerContainer;
        cornerOutside.parent = cornerContainer;

        return cornerContainer;
    },

    createWallWithEdges: function (scene, wallData, position, rotation, name, addEdges = false) {
        // Suppression du paramètre addEdges dans l'appel à createWallWithHoles
        const wallContainer = this.createWallWithHoles(scene, wallData, false);
        wallContainer.position = position;
        wallContainer.rotation = rotation;
        return wallContainer;
    },

    calculateBuildingWidth: function (building) {
        let totalWidth = 0;
        building.rooms.forEach(room => {
            totalWidth += room.walls.frontWall.largeur * this.config.scale;
        });
        totalWidth += (building.rooms.length - 1) * this.config.roomSpacing;
        return totalWidth;
    },

    createRoom: function (scene, roomData, parent) {
        const roomContainer = new BABYLON.TransformNode("room_" + roomData.name, scene);
        roomContainer.parent = parent;

        const frontWidth = roomData.walls.frontWall.largeur * this.config.scale;
        const sideWidth = roomData.walls.leftWall.largeur * this.config.scale;
        const height = roomData.walls.frontWall.hauteur * this.config.scale;
        const wallDepthOffset = this.config.wallDepth / 2;

        // Create floor
        this.createFloor(scene,
            frontWidth,
            sideWidth,
            new BABYLON.Vector3(0, 0, 0),
            roomData.walls.frontWall.hauteur
        ).parent = roomContainer;

        // Create walls - notez la suppression du paramètre addEdges
        const frontWall = this.createWallWithEdges(scene, roomData.walls.frontWall,
            new BABYLON.Vector3(0, 0, sideWidth / 2 + wallDepthOffset),
            new BABYLON.Vector3(0, 0, 0),
            "frontWall"
        );
        frontWall.parent = roomContainer;

        const entranceWall = this.createWallWithEdges(scene, roomData.walls.entranceWall,
            new BABYLON.Vector3(0, 0, -sideWidth / 2 - wallDepthOffset),
            new BABYLON.Vector3(0, Math.PI, 0),
            "entranceWall"
        );
        entranceWall.parent = roomContainer;

        const leftWall = this.createWallWithEdges(scene, roomData.walls.leftWall,
            new BABYLON.Vector3(-frontWidth / 2 - wallDepthOffset, 0, 0),
            new BABYLON.Vector3(0, -Math.PI / 2, 0),
            "leftWall"
        );
        leftWall.parent = roomContainer;

        const rightWall = this.createWallWithEdges(scene, roomData.walls.rightWall,
            new BABYLON.Vector3(frontWidth / 2 + wallDepthOffset, 0, 0),
            new BABYLON.Vector3(0, Math.PI / 2, 0),
            "rightWall"
        );
        rightWall.parent = roomContainer;

        // Create corners
        const corners = [
            // Front-Left corner
            this.createCorner(scene,
                height,
                new BABYLON.Vector3(
                    -frontWidth / 2 - wallDepthOffset,
                    0,
                    sideWidth / 2 + wallDepthOffset
                ),
                "cornerFrontLeft"
            ),
            // Front-Right corner
            this.createCorner(scene,
                height,
                new BABYLON.Vector3(
                    frontWidth / 2 + wallDepthOffset,
                    0,
                    sideWidth / 2 + wallDepthOffset
                ),
                "cornerFrontRight"
            ),
            // Back-Left corner
            this.createCorner(scene,
                height,
                new BABYLON.Vector3(
                    -frontWidth / 2 - wallDepthOffset,
                    0,
                    -sideWidth / 2 - wallDepthOffset
                ),
                "cornerBackLeft"
            ),
            // Back-Right corner
            this.createCorner(scene,
                height,
                new BABYLON.Vector3(
                    frontWidth / 2 + wallDepthOffset,
                    0,
                    -sideWidth / 2 - wallDepthOffset
                ),
                "cornerBackRight"
            )
        ];

        corners.forEach(corner => corner.parent = roomContainer);

        return roomContainer;
    },

    createBuilding: function (scene, buildingData, position) {
        const buildingContainer = new BABYLON.TransformNode("building_" + buildingData.name, scene);
        buildingContainer.position = position;

        // Calculer les dimensions totales du bâtiment
        let totalWidth = 0;
        let maxDepth = 0;
        buildingData.rooms.forEach(room => {
            totalWidth += room.walls.frontWall.largeur * this.config.scale;
            maxDepth = Math.max(maxDepth, room.walls.leftWall.largeur * this.config.scale);
        });
        totalWidth += (buildingData.rooms.length - 1) * this.config.roomSpacing;

        // Créer les murs du bâtiment
        const buildingWalls = {
            front: BABYLON.MeshBuilder.CreateBox("buildingFrontWall", {
                width: totalWidth + this.config.buildingWallThickness,
                height: buildingData.rooms[0].walls.frontWall.hauteur * this.config.scale,
                depth: this.config.buildingWallThickness
            }, scene),
            back: BABYLON.MeshBuilder.CreateBox("buildingBackWall", {
                width: totalWidth + this.config.buildingWallThickness,
                height: buildingData.rooms[0].walls.frontWall.hauteur * this.config.scale,
                depth: this.config.buildingWallThickness
            }, scene),
            left: BABYLON.MeshBuilder.CreateBox("buildingLeftWall", {
                width: this.config.buildingWallThickness,
                height: buildingData.rooms[0].walls.frontWall.hauteur * this.config.scale,
                depth: maxDepth + this.config.buildingWallThickness
            }, scene),
            right: BABYLON.MeshBuilder.CreateBox("buildingRightWall", {
                width: this.config.buildingWallThickness,
                height: buildingData.rooms[0].walls.frontWall.hauteur * this.config.scale,
                depth: maxDepth + this.config.buildingWallThickness
            }, scene)
        };

        // Positionner les murs du bâtiment
        buildingWalls.front.position = new BABYLON.Vector3(0, 0, maxDepth / 2 + this.config.buildingWallThickness / 2);
        buildingWalls.back.position = new BABYLON.Vector3(0, 0, -maxDepth / 2 - this.config.buildingWallThickness / 2);
        buildingWalls.left.position = new BABYLON.Vector3(-totalWidth / 2 - this.config.buildingWallThickness / 2, 0, 0);
        buildingWalls.right.position = new BABYLON.Vector3(totalWidth / 2 + this.config.buildingWallThickness / 2, 0, 0);

        // Matériau pour les murs du bâtiment
        const buildingWallMaterial = new BABYLON.StandardMaterial("buildingWallMaterial", scene);
        buildingWallMaterial.diffuseColor = new BABYLON.Color3(0.7, 0.7, 0.7);
        buildingWallMaterial.alpha = 0.3;

        Object.values(buildingWalls).forEach(wall => {
            wall.material = buildingWallMaterial;
            wall.parent = buildingContainer;
        });

        // Créer les salles
        let roomX = -totalWidth / 2;
        buildingData.rooms.forEach(room => {
            const roomContainer = this.createRoom(scene, room, buildingContainer);
            roomContainer.position.x = roomX;
            roomX += room.walls.frontWall.largeur * this.config.scale + this.config.roomSpacing;
        });

        return buildingContainer;
    },

    initializeScene: function (canvasId, buildingsDataJson) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) throw new Error('Canvas not found');
        const buildingsData = typeof buildingsDataJson === 'string' ? JSON.parse(buildingsDataJson) : buildingsDataJson;

        const engine = new BABYLON.Engine(canvas, true);
        const scene = new BABYLON.Scene(engine);
        scene.clearColor = new BABYLON.Color3(0.3, 0.3, 0.9);

        let maxBuildingWidth = 0;
        let totalWidth = 0;
        buildingsData.forEach(building => {
            
            const buildingWidth = this.calculateBuildingWidth(building);
            maxBuildingWidth = Math.max(maxBuildingWidth, buildingWidth);
            totalWidth += buildingWidth;
        });
        totalWidth += (buildingsData.length - 1) * (this.config.buildingSpacing / this.config.scale);

        let currentX = -totalWidth / 2;

        buildingsData.forEach(building => {
            const buildingContainer = new BABYLON.TransformNode("building_" + building.name, scene);
            buildingContainer.roomCount = building.rooms.length;
            const buildingWidth = this.calculateBuildingWidth(building);

            buildingContainer.position.x = currentX + (buildingWidth / 2);

            let roomX = 0;
            building.rooms.forEach((room, index) => {
                room.index = index;
                const roomContainer = this.createRoom(scene, room, buildingContainer);
                roomContainer.position.x = roomX;
                roomX += (room.walls.frontWall.largeur * this.config.scale) + (this.config.roomSpacing / this.config.scale);
            });

            currentX += buildingWidth + (this.config.buildingSpacing / this.config.scale);
        });

        const camera = new BABYLON.ArcRotateCamera("camera",
            Math.PI / 4,
            Math.PI / 3,
            totalWidth,
            BABYLON.Vector3.Zero(),
            scene
        );
        camera.attachControl(canvas, true);
        camera.lowerRadiusLimit = totalWidth / 32;
        camera.upperRadiusLimit = totalWidth * 2;
        camera.wheelPrecision = 35;
        camera.pinchPrecision = 50;

        const light = new BABYLON.HemisphericLight("light",
            new BABYLON.Vector3(0, 1, 0),
            scene
        );
        light.intensity = 0.8;

        engine.runRenderLoop(() => scene.render());
        window.addEventListener("resize", () => engine.resize());
        return scene;
    }
};

window.babylonInterop = babylonInterop;