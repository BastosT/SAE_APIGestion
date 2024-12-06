(function () {
    console.log("Initializing babylonInterop");

    // Garantir que babylonInterop existe
    window.babylonInterop = window.babylonInterop || {};

    // Étendre l'objet existant
    Object.assign(window.babylonInterop, {
    config: {
        scale: 0.01,
        wallDepth: 0.10,
        equipmentDepth: 0.1,
        floorHeight: 0.1,
        buildingSpacing: 5,
        roomSpacing: 0.25,
        buildingWallThickness: 0.3
    },
    sharedMaterials: {
        inside: null,
        outside: null
    },

    initSharedMaterials: function (scene) {
        const baseProperties = {
            diffuseColor: new BABYLON.Color3(0.725, 0.608, 0.486),
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

    convertToNewModel: function (oldModel) {
        return oldModel.map(batiment => ({
            name: batiment.nom,
            rooms: batiment.salles.map(salle => ({
                name: salle.nom,
                walls: {
                    frontWall: this.convertWall(salle.murFace),
                    entranceWall: this.convertWall(salle.murEntree),
                    leftWall: this.convertWall(salle.murGauche),
                    rightWall: this.convertWall(salle.murDroite)
                }
            }))
        }));
    },

    convertWall: function (oldWall) {
        if (!oldWall) return {
            largeur: 0,
            hauteur: 0,
            equipements: []
        };

        const wall = {
            largeur: oldWall.longueur,
            hauteur: oldWall.hauteur,
            equipements: []
        };

        // Convertir les équipements standards
        if (oldWall.equipements) {
            wall.equipements.push(...oldWall.equipements.map(eq => ({
                nom: eq.nom,
                type: this.getEquipmentType(eq.typeEquipement?.nom),
                largeur: eq.longueur,
                hauteur: eq.hauteur,
                positionX: eq.positionX,
                positionY: eq.positionY
            })));
        }

        // Convertir les capteurs
        if (oldWall.capteurs) {
            wall.equipements.push(...oldWall.capteurs.map(cap => ({
                nom: cap.nom,
                type: 4, // Type Capteur
                largeur: cap.longeur,
                hauteur: cap.hauteur,
                positionX: cap.positionX,
                positionY: cap.positionY
            })));
        }

        return wall;
    },

    getEquipmentType: function (typeName) {
        switch (typeName?.toLowerCase()) {
            case 'fenetre': return 0;
            case 'vitre': return 1;
            case 'porte': return 2;
            case 'radiateur': return 3;
            default: return 5;
        }
    },

    createWallWithHoles: function (scene, wallData) {
        const { scale, wallDepth } = this.config;
        const width = wallData.largeur * scale;
        const height = wallData.hauteur * scale;
        const wallContainer = new BABYLON.TransformNode("wallContainer", scene);
        const doorMargin = 0.1;

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

                const frame = opening.type === 2 ?
                    this.createDoorFrame(scene, opening, width, height) :
                    this.createWindowFrame(scene, opening, width, height);
                frame.parent = wallContainer;

                currentX = (opening.positionX + opening.largeur) * scale;
            }
        }

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

        section.position.x = params.x;
        if (params.y) section.position.y = params.y;

        const sectionOutside = section.clone("outside" + params.name);

        section.material = this.sharedMaterials.inside;
        sectionOutside.material = this.sharedMaterials.outside;

        sectionOutside.scaling.z = -1;

        section.parent = parent;
        sectionOutside.parent = parent;
    },

    createDoorFrame: function (scene, door, wallWidth, wallHeight) {
        const doorMargin = 0.005;
        const actualWidth = door.largeur * this.config.scale;
        const actualHeight = door.hauteur * this.config.scale;
        const frameWidth = actualWidth - (doorMargin * 2);
        const frameThickness = 0.035;
        const frame = new BABYLON.TransformNode("doorFrame", scene);

        // Ajuster les dimensions du cadre pour qu'elles soient symétriques
        const parts = [
            {
                // Partie supérieure
                dimensions: {
                    width: frameWidth,
                    height: frameThickness,
                    depth: this.config.wallDepth
                },
                position: new BABYLON.Vector3(0, actualHeight / 2 - frameThickness / 2, 0)
            },
            {
                // Montant gauche
                dimensions: {
                    width: frameThickness,
                    height: actualHeight,
                    depth: this.config.wallDepth
                },
                position: new BABYLON.Vector3(-frameWidth / 2 + frameThickness / 2, 0, 0)
            },
            {
                // Montant droit
                dimensions: {
                    width: frameThickness,
                    height: actualHeight,
                    depth: this.config.wallDepth
                },
                position: new BABYLON.Vector3(frameWidth / 2 - frameThickness / 2, 0, 0)
            }
        ];

        // Matériau pour le cadre de la porte (plus foncé)
        const frameMaterial = new BABYLON.StandardMaterial("doorFrameMaterial", scene);
        frameMaterial.diffuseColor = new BABYLON.Color3(0.4, 0.2, 0.1); // Marron foncé

        parts.forEach((part, index) => {
            const mesh = BABYLON.MeshBuilder.CreateBox("doorFramePart" + index, part.dimensions, scene);
            mesh.position = part.position;
            mesh.material = frameMaterial;
            mesh.parent = frame;

            // Créer une copie pour l'autre côté du mur
            const meshOutside = mesh.clone("doorFramePartOutside" + index);
            meshOutside.position.z = -mesh.position.z;
            meshOutside.parent = frame;
        });

        // Matériau pour le panneau de la porte (plus clair)
        const doorPanelMaterial = new BABYLON.StandardMaterial("doorPanelMaterial", scene);
        doorPanelMaterial.diffuseColor = new BABYLON.Color3(0.6, 0.4, 0.2); // Marron clair
        doorPanelMaterial.specularColor = new BABYLON.Color3(0.1, 0.1, 0.1);
        doorPanelMaterial.roughness = 0.7;

        // Créer le panneau de porte avec des dimensions ajustées
        const doorPanelWidth = frameWidth - (frameThickness * 2);
        const doorPanelHeight = actualHeight - (frameThickness * 2);

        const door_panel = BABYLON.MeshBuilder.CreateBox("doorPanel", {
            width: doorPanelWidth,
            height: doorPanelHeight,
            depth: frameThickness
        }, scene);

        door_panel.material = doorPanelMaterial;
        door_panel.parent = frame;
        // Centrer le panneau de porte dans l'ouverture
        door_panel.position.z = 0;

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
            const sensorContainer = new BABYLON.TransformNode("sensorContainer", scene);

            const sensorWidth = 0.12;
            const sensorHeight = 0.08;
            const sensorDepth = 0.03;

            // Boîtier principal du capteur avec rotation de 180 degrés par défaut
            const sensorBox = BABYLON.MeshBuilder.CreateBox("sensorBox", {
                width: sensorWidth,
                height: sensorHeight,
                depth: sensorDepth,
                updatable: false
            }, scene);

            // Rotation de 180 degrés autour de l'axe Y pour le boîtier de base
            sensorBox.rotation.y = Math.PI;

            const boxMaterial = new BABYLON.StandardMaterial("sensorBoxMaterial", scene);
            boxMaterial.diffuseColor = new BABYLON.Color3(0.9, 0.9, 0.9);
            boxMaterial.metallic = 0.3;
            boxMaterial.roughness = 0.7;
            sensorBox.material = boxMaterial;

            // Création de l'écran LCD
            const screenWidth = sensorWidth * 0.7;
            const screenHeight = sensorHeight * 0.5;
            const screenDepth = 0.001;

            const screen = BABYLON.MeshBuilder.CreateBox("screen", {
                width: screenWidth,
                height: screenHeight,
                depth: screenDepth
            }, scene);

            // Matériau pour l'écran LCD
            const screenMaterial = new BABYLON.StandardMaterial("screenMaterial", scene);
            screenMaterial.diffuseColor = new BABYLON.Color3(0.1, 0.1, 0.1);
            screenMaterial.emissiveColor = new BABYLON.Color3(0.2, 0.3, 0.4); // Lueur bleutée
            screenMaterial.specularColor = new BABYLON.Color3(0.2, 0.2, 0.2);
            screen.material = screenMaterial;

            // Grille de ventilation en bas
            const ventWidth = sensorWidth * 0.6;
            const ventHeight = sensorHeight * 0.15;
            const ventilation = BABYLON.MeshBuilder.CreatePlane("ventilation", {
                width: ventWidth,
                height: ventHeight
            }, scene);

            const ventMaterial = new BABYLON.StandardMaterial("ventMaterial", scene);
            ventMaterial.diffuseColor = new BABYLON.Color3(0.3, 0.3, 0.3);
            ventMaterial.alpha = 0.8;
            ventilation.material = ventMaterial;

            const xPos = equipment.positionX * this.config.scale;
            const yPos = equipment.positionY * this.config.scale;

            // Position de base du capteur
            sensorBox.position = new BABYLON.Vector3(
                -wallWidth / 2 + xPos + sensorWidth / 2,
                wallHeight / 2 - yPos - sensorHeight / 2,
                -(this.config.wallDepth / 2 + sensorDepth / 2)
            );

            // Ajuster l'orientation en fonction de la rotation du mur parent
            if (parentWall.rotation) {
                const currentRotation = parentWall.rotation.y;

                if (Math.abs(currentRotation) === Math.PI / 2) {
                    // Murs latéraux
                    const direction = Math.sign(currentRotation);
                    sensorBox.position.x = direction * (this.config.wallDepth / 2 + sensorDepth / 2);
                    sensorBox.position.z = -wallWidth / 2 + xPos + sensorWidth / 2;
                    // Ajuster la rotation pour faire face à l'intérieur
                    sensorBox.rotation.y = Math.PI + currentRotation;
                } else if (Math.abs(currentRotation) === Math.PI) {
                    // Mur d'entrée
                    sensorBox.position.z = this.config.wallDepth / 2 + sensorDepth / 2;
                    // Garder la rotation de base pour le mur d'entrée
                } else {
                    // Mur avant (face)
                    sensorBox.position.z = -(this.config.wallDepth / 2 + sensorDepth / 2);
                }
            }

            // Positionnement de l'écran et de la ventilation toujours sur la face avant du boîtier
            screen.position = new BABYLON.Vector3(0, 0, sensorDepth / 2 + screenDepth / 2);
            ventilation.position = new BABYLON.Vector3(0, -sensorHeight / 3, sensorDepth / 2 + 0.001);

            screen.parent = sensorBox;
            ventilation.parent = sensorBox;
            sensorBox.parent = sensorContainer;
            sensorContainer.parent = parentWall;

            return sensorContainer;
        }

        // Pour les autres types d'équipements
        const width = equipment.largeur * this.config.scale;
        const height = equipment.hauteur * this.config.scale;
        const depth = this.config.equipmentDepth;

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

        return equipmentMesh;
    },

    createFloor: function (scene, width, depth, wallHeight) {
        const floor = BABYLON.MeshBuilder.CreateBox("floor", {
            width: width + (this.config.wallDepth * 2),
            height: this.config.floorHeight,
            depth: depth + (this.config.wallDepth * 2)
        }, scene);

        // Positionner le sol à Y=0
        floor.position.y = 0;

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
        cornerMaterial.diffuseColor = new BABYLON.Color3(0.725, 0.608, 0.486);
        corner.material = cornerMaterial;
        cornerOutside.material = cornerMaterial;

        cornerOutside.scaling = new BABYLON.Vector3(-1, 1, -1);

        corner.position = position;
        cornerOutside.position = position;

        corner.parent = cornerContainer;
        cornerOutside.parent = cornerContainer;

        return cornerContainer;
    },

    createRoom: function (scene, roomData, parent) {
        const roomContainer = new BABYLON.TransformNode("room_" + roomData.name, scene);
        roomContainer.parent = parent;

        const frontWidth = roomData.walls.frontWall.largeur * this.config.scale;
        const sideWidth = roomData.walls.leftWall.largeur * this.config.scale;
        const height = roomData.walls.frontWall.hauteur * this.config.scale;
        const wallDepthOffset = this.config.wallDepth / 2;

        // Créer le sol au niveau 0
        const floor = this.createFloor(scene, frontWidth, sideWidth, height);
        floor.parent = roomContainer;

        // Les murs commencent au niveau du sol (Y=0) et montent vers le haut
        const wallsStartY = this.config.floorHeight / 2;  // Commencer juste au-dessus du sol

        const walls = {
            front: this.createWallWithHoles(scene, roomData.walls.frontWall),
            entrance: this.createWallWithHoles(scene, roomData.walls.entranceWall),
            left: this.createWallWithHoles(scene, roomData.walls.leftWall),
            right: this.createWallWithHoles(scene, roomData.walls.rightWall)
        };

        // Positionner tous les murs avec leur base au niveau du sol
        walls.front.position = new BABYLON.Vector3(
            0,
            height / 2 + wallsStartY,
            sideWidth / 2 + wallDepthOffset
        );
        walls.entrance.position = new BABYLON.Vector3(
            0,
            height / 2 + wallsStartY,
            -sideWidth / 2 - wallDepthOffset
        );
        walls.entrance.rotation = new BABYLON.Vector3(0, Math.PI, 0);

        walls.left.position = new BABYLON.Vector3(
            -frontWidth / 2 - wallDepthOffset,
            height / 2 + wallsStartY,
            0
        );
        walls.left.rotation = new BABYLON.Vector3(0, -Math.PI / 2, 0);

        walls.right.position = new BABYLON.Vector3(
            frontWidth / 2 + wallDepthOffset,
            height / 2 + wallsStartY,
            0
        );
        walls.right.rotation = new BABYLON.Vector3(0, Math.PI / 2, 0);

        Object.values(walls).forEach(wall => wall.parent = roomContainer);

        // Positionner les coins au même niveau que les murs
        const corners = [
            this.createCorner(scene, height,
                new BABYLON.Vector3(
                    -frontWidth / 2 - wallDepthOffset,
                    height / 2 + wallsStartY,
                    sideWidth / 2 + wallDepthOffset
                ),
                "cornerFrontLeft"),
            this.createCorner(scene, height,
                new BABYLON.Vector3(
                    frontWidth / 2 + wallDepthOffset,
                    height / 2 + wallsStartY,
                    sideWidth / 2 + wallDepthOffset
                ),
                "cornerFrontRight"),
            this.createCorner(scene, height,
                new BABYLON.Vector3(
                    -frontWidth / 2 - wallDepthOffset,
                    height / 2 + wallsStartY,
                    -sideWidth / 2 - wallDepthOffset
                ),
                "cornerBackLeft"),
            this.createCorner(scene, height,
                new BABYLON.Vector3(
                    frontWidth / 2 + wallDepthOffset,
                    height / 2 + wallsStartY,
                    -sideWidth / 2 - wallDepthOffset
                ),
                "cornerBackRight")
        ];

        corners.forEach(corner => corner.parent = roomContainer);

        return roomContainer;
    },

    findMaxBuildingHeight: function (buildingsData) {
        let maxHeight = 0;
        buildingsData.forEach(building => {
            const buildingHeight = this.calculateBuildingHeight(building);
            maxHeight = Math.max(maxHeight, buildingHeight);
        });
        return maxHeight;
    },

    calculateBuildingHeight: function (building) {
        let maxHeight = 0;
        building.rooms.forEach(room => {
            const roomHeight = room.walls.frontWall.hauteur * this.config.scale;
            maxHeight = Math.max(maxHeight, roomHeight);
        });
        return maxHeight;
    },

    calculateBuildingWidth: function (building) {
        let totalWidth = 0;
        building.rooms.forEach((room, index) => {
            // Ajouter la largeur de la salle
            totalWidth += (room.walls.frontWall.largeur * this.config.scale);
            // Ajouter l'espacement entre les salles, sauf pour la dernière salle
            if (index < building.rooms.length - 1) {
                totalWidth += this.config.roomSpacing + (this.config.wallDepth * 2);
            }
        });
        return totalWidth;
    },

    createBuilding: function (scene, buildingData, position) {
        const buildingContainer = new BABYLON.TransformNode("building_" + buildingData.name, scene);
        buildingContainer.position = new BABYLON.Vector3(position.x, 0, position.z);

        let totalWidth = this.calculateBuildingWidth(buildingData);
        let currentX = -totalWidth / 2;

        buildingData.rooms.forEach((room, index) => {
            const roomWidth = room.walls.frontWall.largeur * this.config.scale;
            const roomContainer = this.createRoom(scene, room, buildingContainer);
            roomContainer.position.x = currentX + (roomWidth / 2);

            currentX += roomWidth;
            if (index < buildingData.rooms.length - 1) {
                currentX += this.config.roomSpacing + (this.config.wallDepth * 2);
            }
        });

        return buildingContainer;
    },

        disposeScene: function () {
            console.log("disposeScene called");
            console.log("currentScene exists:", !!window.currentScene);
            console.log("currentEngine exists:", !!window.currentEngine);

            try {
                if (window.currentScene) {
                    console.log("Stopping render loop...");
                    if (window.currentEngine) {
                        window.currentEngine.stopRenderLoop();
                    }

                    // D'abord disposer les matériaux partagés
                    if (this.sharedMaterials) {
                        console.log("Disposing shared materials...");
                        Object.values(this.sharedMaterials).forEach(material => {
                            if (material) {
                                material.dispose();
                            }
                        });
                        this.sharedMaterials.inside = null;
                        this.sharedMaterials.outside = null;
                    }

                    // Ensuite les meshes
                    if (window.currentScene.meshes) {
                        console.log("Disposing meshes...");
                        // Créer une copie du tableau pour éviter les problèmes de modification pendant l'itération
                        [...window.currentScene.meshes].forEach(mesh => {
                            if (mesh) {
                                mesh.dispose(false, true); // dispose le mesh et ses enfants
                            }
                        });
                    }

                    // Attendre un court instant avant de disposer la scène
                    setTimeout(() => {
                        console.log("Disposing scene...");
                        window.currentScene.dispose();

                        if (window.currentEngine) {
                            console.log("Disposing engine...");
                            window.currentEngine.dispose();
                            window.currentEngine = null;
                        }

                        window.currentScene = null;
                        console.log("Scene disposal complete.");
                    }, 100);

                } else {
                    console.log("No scene to dispose.");
                }
            } catch (error) {
                console.error("Error during scene disposal:", error);
            }
        },

    initializeScene: function (canvasId, buildingsDataJson) {
        console.log("initializeScene called with canvasId:", canvasId);
        console.log("window.babylonInterop exists:", !!window.babylonInterop);
        console.log("disposeScene exists:", !!window.babylonInterop.disposeScene);

        // Stocker une référence à l'objet window
        window.currentScene = null;
        window.currentEngine = null;

        const canvas = document.getElementById(canvasId);
        if (!canvas) throw new Error('Canvas not found');

        const buildingsData = this.convertToNewModel(JSON.parse(buildingsDataJson));
        const engine = new BABYLON.Engine(canvas, true);
        window.currentEngine = engine;
        const scene = new BABYLON.Scene(engine);
        window.currentScene = scene;
        scene.clearColor = new BABYLON.Color3(0.3, 0.3, 0.9);

        let totalWidth = 0;
        buildingsData.forEach(building => {
            const buildingWidth = this.calculateBuildingWidth(building);
            totalWidth += buildingWidth;
        });
        totalWidth += (buildingsData.length - 1) * this.config.buildingSpacing;

        let currentX = -totalWidth / 2;
        buildingsData.forEach(building => {
            const buildingWidth = this.calculateBuildingWidth(building);
            const buildingContainer = this.createBuilding(
                scene,
                building,
                new BABYLON.Vector3(currentX + (buildingWidth / 2), 0, 0)
            );
            currentX += buildingWidth + this.config.buildingSpacing;
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
    });

    console.log("babylonInterop initialization complete", {
        hasInitializeScene: typeof window.babylonInterop.initializeScene === 'function',
        hasDisposeScene: typeof window.babylonInterop.disposeScene === 'function'
    });
})();
