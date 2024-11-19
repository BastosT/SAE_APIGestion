var babylonInterop = {
    config: {
        scale: 0.01,
        wallDepth: 0.15,
        equipmentDepth: 0.1,
        floorHeight: 0.1
    },

    createWallWithHoles: function (scene, wallData, addEdges = false) {
        const width = wallData.largeur * this.config.scale;
        const height = wallData.hauteur * this.config.scale;
        const depth = this.config.wallDepth;
        const doorMargin = 0.1; // Même marge que dans createDoorFrame

        let wallSections = [];
        let currentX = 0;
        let openings = wallData.equipements?.filter(e => e.type === 0 || e.type === 1 || e.type === 2) || [];
        let otherEquipments = wallData.equipements?.filter(e => e.type !== 0 && e.type !== 1 && e.type !== 2) || [];

        openings = openings.map(opening => ({
            ...opening,
            // Augmenter la largeur de l'ouverture pour les portes
            largeur: opening.type === 2 ? opening.largeur + (doorMargin * 4 / this.config.scale) : opening.largeur,
            // Ajuster la position pour centrer
            positionX: opening.type === 2 ? opening.positionX - (doorMargin * 2 / this.config.scale) : opening.positionX,
        }));

        openings.sort((a, b) => a.positionX - b.positionX);

        // Créer les sections de mur entre les ouvertures
        for (let i = 0; i <= openings.length; i++) {
            let sectionWidth;
            if (i === openings.length) {
                sectionWidth = width - currentX;
            } else {
                sectionWidth = (openings[i].positionX * this.config.scale) - currentX;
            }

            if (sectionWidth > 0) {
                const section = BABYLON.MeshBuilder.CreateBox("wallSection" + i, {
                    width: sectionWidth,
                    height: height,
                    depth: depth
                }, scene);

                section.position.x = -width / 2 + currentX + sectionWidth / 2;
                wallSections.push(section);
            }

            if (i < openings.length) {
                currentX = (openings[i].positionX + openings[i].largeur) * this.config.scale;

                const openingHeight = openings[i].hauteur * this.config.scale;
                const openingY = openings[i].positionY * this.config.scale;

                // Section au-dessus de l'ouverture
                if (openingY > 0) {
                    const topSection = BABYLON.MeshBuilder.CreateBox("wallTopSection" + i, {
                        width: openings[i].largeur * this.config.scale,
                        height: openingY,
                        depth: depth
                    }, scene);
                    topSection.position.x = -width / 2 + openings[i].positionX * this.config.scale + (openings[i].largeur * this.config.scale) / 2;
                    topSection.position.y = height / 2 - openingY / 2;
                    wallSections.push(topSection);
                }

                // Si c'est une fenêtre ou une porte-fenêtre, ajouter la section du bas
                if (openings[i].type !== 2) {
                    const bottomHeight = height - openingY - openingHeight;
                    if (bottomHeight > 0) {
                        const bottomSection = BABYLON.MeshBuilder.CreateBox("wallBottomSection" + i, {
                            width: openings[i].largeur * this.config.scale,
                            height: bottomHeight,
                            depth: depth
                        }, scene);
                        bottomSection.position.x = -width / 2 + openings[i].positionX * this.config.scale + (openings[i].largeur * this.config.scale) / 2;
                        bottomSection.position.y = -height / 2 + bottomHeight / 2;
                        wallSections.push(bottomSection);
                    }
                }

                // Créer le cadre approprié selon le type
                if (openings[i].type === 2) {
                    const frame = this.createDoorFrame(scene, openings[i], width, height);
                    wallSections.push(frame);
                } else {
                    const frame = this.createWindowFrame(scene, openings[i], width, height);
                    wallSections.push(frame);
                }
            }
        }

        const wallContainer = new BABYLON.TransformNode("wallContainer", scene);

        wallSections.forEach(section => {
            if (section instanceof BABYLON.TransformNode) {
                section.parent = wallContainer;
                return;
            }

            const sectionOutside = section.clone("outside" + section.name);

            const materialInside = new BABYLON.StandardMaterial(section.name + "MaterialInside", scene);
            materialInside.diffuseColor = new BABYLON.Color3(0.95, 0.95, 0.95);
            materialInside.roughness = 0.8;
            materialInside.backFaceCulling = true;

            const materialOutside = new BABYLON.StandardMaterial(section.name + "MaterialOutside", scene);
            materialOutside.diffuseColor = new BABYLON.Color3(0.95, 0.95, 0.95);
            materialOutside.alpha = 0.3;
            materialOutside.roughness = 0.8;
            materialOutside.backFaceCulling = true;

            section.material = materialInside;
            sectionOutside.material = materialOutside;
            sectionOutside.scaling.z = -1;

            section.parent = wallContainer;
            sectionOutside.parent = wallContainer;
        });

        // Ajouter les autres équipements
        if (otherEquipments.length > 0) {
            otherEquipments.forEach(equipment => {
                const equipmentMesh = this.createEquipment(scene, equipment, wallContainer, width, height);
                if (equipmentMesh) {
                    equipmentMesh.parent = wallContainer;
                }
            });
        }

        return wallContainer;
    },

    createDoorFrame: function (scene, door, wallWidth, wallHeight) {
        // Augmentation significative de la marge
        const doorMargin = 0.1; // 10cm de marge
        const actualWidth = door.largeur * this.config.scale;
        const actualHeight = door.hauteur * this.config.scale;
        const frameWidth = actualWidth - (doorMargin * 2); // Réduire la taille du cadre plutôt que l'augmenter
        const frameHeight = actualHeight;
        const frameDepth = this.config.wallDepth;
        const frameThickness = 0.04;

        const frame = new BABYLON.TransformNode("doorFrame", scene);

        // Créer les montants du cadre légèrement à l'intérieur de l'ouverture
        const parts = [
            // Montant horizontal supérieur
            {
                dimensions: { width: frameWidth + frameThickness * 2, height: frameThickness, depth: frameDepth },
                position: new BABYLON.Vector3(0, frameHeight / 2 - frameThickness / 2, 0)
            },
            // Montant vertical gauche
            {
                dimensions: { width: frameThickness, height: frameHeight, depth: frameDepth },
                position: new BABYLON.Vector3(-frameWidth / 2, 0, 0)
            },
            // Montant vertical droit
            {
                dimensions: { width: frameThickness, height: frameHeight, depth: frameDepth },
                position: new BABYLON.Vector3(frameWidth / 2, 0, 0)
            }
        ];

        parts.forEach((part, index) => {
            const mesh = BABYLON.MeshBuilder.CreateBox("doorFramePart" + index, part.dimensions, scene);
            mesh.position = part.position;

            const material = new BABYLON.StandardMaterial("doorFrameMaterial" + index, scene);
            material.diffuseColor = new BABYLON.Color3(0.4, 0.2, 0.1);
            material.metallic = 0.1;
            material.roughness = 0.8;

            mesh.material = material;
            mesh.parent = frame;
        });

        // Ajouter la porte elle-même
        const door_panel = BABYLON.MeshBuilder.CreateBox("doorPanel", {
            width: frameWidth - frameThickness * 2,
            height: frameHeight - frameThickness * 2,
            depth: frameThickness
        }, scene);

        const doorMaterial = new BABYLON.StandardMaterial("doorMaterial", scene);
        doorMaterial.diffuseColor = new BABYLON.Color3(0.6, 0.4, 0.2);
        doorMaterial.metallic = 0.1;
        doorMaterial.roughness = 0.7;

        door_panel.material = doorMaterial;
        door_panel.parent = frame;
        door_panel.position.z = frameDepth / 4;

        // Positionner le cadre complet
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
        const frameDepth = this.config.wallDepth;
        const frameThickness = 0.02;

        const frame = new BABYLON.TransformNode("windowFrame", scene);

        // Créer les montants du cadre
        const parts = [
            // Montant horizontal supérieur
            {
                dimensions: { width: frameWidth, height: frameThickness, depth: frameDepth },
                position: new BABYLON.Vector3(0, frameHeight / 2, 0)
            },
            // Montant horizontal inférieur
            {
                dimensions: { width: frameWidth, height: frameThickness, depth: frameDepth },
                position: new BABYLON.Vector3(0, -frameHeight / 2, 0)
            },
            // Montant vertical gauche
            {
                dimensions: { width: frameThickness, height: frameHeight, depth: frameDepth },
                position: new BABYLON.Vector3(-frameWidth / 2, 0, 0)
            },
            // Montant vertical droit
            {
                dimensions: { width: frameThickness, height: frameHeight, depth: frameDepth },
                position: new BABYLON.Vector3(frameWidth / 2, 0, 0)
            }
        ];

        parts.forEach((part, index) => {
            const mesh = BABYLON.MeshBuilder.CreateBox("framePart" + index, part.dimensions, scene);
            mesh.position = part.position;

            const material = new BABYLON.StandardMaterial("frameMaterial" + index, scene);
            material.diffuseColor = new BABYLON.Color3(0.7, 0.7, 0.7);
            material.metallic = 0.3;
            material.roughness = 0.4;

            mesh.material = material;
            mesh.parent = frame;
        });

        // Positionner le cadre complet
        frame.position = new BABYLON.Vector3(
            -wallWidth / 2 + window.positionX * this.config.scale + frameWidth / 2,
            wallHeight / 2 - window.positionY * this.config.scale - frameHeight / 2,
            0
        );

        // Ajouter la vitre semi-transparente
        const glass = BABYLON.MeshBuilder.CreatePlane("windowGlass", {
            width: frameWidth - frameThickness * 2,
            height: frameHeight - frameThickness * 2
        }, scene);

        const glassMaterial = new BABYLON.StandardMaterial("glassMaterial", scene);
        glassMaterial.diffuseColor = new BABYLON.Color3(0.8, 0.8, 1.0);
        glassMaterial.alpha = 0.3;
        glassMaterial.backFaceCulling = false;

        glass.material = glassMaterial;
        glass.parent = frame;

        return frame;
    },

    createEquipment: function (scene, equipment, parentWall, wallWidth, wallHeight) {
        // Seulement pour les équipements qui ne sont pas des fenêtres
        if (equipment.type === 0 || equipment.type === 1) return null;

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

        switch (equipment.type) {
            case 3: // Radiator
                material.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
                material.metallic = 0.8;
                material.roughness = 0.3;
                break;
            case 2: // Door
                material.diffuseColor = new BABYLON.Color3(0.6, 0.4, 0.2);
                break;
            case 4: // Sensor
                material.diffuseColor = new BABYLON.Color3(1.0, 0.2, 0.2);
                break;
            default: // Other
                material.diffuseColor = new BABYLON.Color3(0.9, 0.9, 0.9);
                break;
        }

        equipmentMesh.material = material;
        equipmentMesh.parent = parentWall;
        return equipmentMesh;
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
        const corner = BABYLON.MeshBuilder.CreateBox(name, {
            width: this.config.wallDepth,
            height: height,
            depth: this.config.wallDepth
        }, scene);

        corner.position = position;

        const cornerMaterialInside = new BABYLON.StandardMaterial(name + "MaterialInside", scene);
        cornerMaterialInside.diffuseColor = new BABYLON.Color3(0.95, 0.95, 0.95);
        cornerMaterialInside.roughness = 0.8;
        cornerMaterialInside.backFaceCulling = true;

        const cornerMaterialOutside = new BABYLON.StandardMaterial(name + "MaterialOutside", scene);
        cornerMaterialOutside.diffuseColor = new BABYLON.Color3(0.95, 0.95, 0.95);
        cornerMaterialOutside.alpha = 0.3;
        cornerMaterialOutside.roughness = 0.8;
        cornerMaterialOutside.backFaceCulling = true;

        const cornerOutside = corner.clone(name + "Outside");
        corner.material = cornerMaterialInside;
        cornerOutside.material = cornerMaterialOutside;
        cornerOutside.scaling = new BABYLON.Vector3(-1, 1, -1);

        return corner;
    },

    createWallWithEdges: function (scene, wallData, position, rotation, name, addEdges = false) {
        const wallContainer = this.createWallWithHoles(scene, wallData, addEdges);
        wallContainer.position = position;
        wallContainer.rotation = rotation;
        return wallContainer;
    },

    initializeScene: function (canvasId, roomDataJson) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) throw new Error('Canvas not found');
        const roomData = typeof roomDataJson === 'string' ? JSON.parse(roomDataJson) : roomDataJson;

        const engine = new BABYLON.Engine(canvas, true);
        const scene = new BABYLON.Scene(engine);
        scene.clearColor = new BABYLON.Color3(0.9, 0.9, 0.9);

        const frontWidth = roomData.murFace.largeur * this.config.scale;
        const sideWidth = roomData.murGauche.largeur * this.config.scale;
        const height = roomData.murFace.hauteur * this.config.scale;
        const wallDepthOffset = this.config.wallDepth / 2;

        this.createFloor(scene,
            roomData.murFace.largeur * this.config.scale,
            roomData.murGauche.largeur * this.config.scale,
            new BABYLON.Vector3(0, 0, 0),
            roomData.murFace.hauteur
        );

        this.createWallWithEdges(scene, roomData.murFace,
            new BABYLON.Vector3(0, 0, sideWidth / 2 + wallDepthOffset),
            new BABYLON.Vector3(0, 0, 0),
            "murFace",
            true);

        this.createWallWithEdges(scene, roomData.murEntree,
            new BABYLON.Vector3(0, 0, -sideWidth / 2 - wallDepthOffset),
            new BABYLON.Vector3(0, Math.PI, 0),
            "murEntree",
            true);

        this.createWallWithEdges(scene, roomData.murGauche,
            new BABYLON.Vector3(-frontWidth / 2 - wallDepthOffset, 0, 0),
            new BABYLON.Vector3(0, -Math.PI / 2, 0),
            "murGauche");

        this.createWallWithEdges(scene, roomData.murDroite,
            new BABYLON.Vector3(frontWidth / 2 + wallDepthOffset, 0, 0),
            new BABYLON.Vector3(0, Math.PI / 2, 0),
            "murDroite");
        this.createCorner(scene, height,
            new BABYLON.Vector3(-frontWidth / 2 - wallDepthOffset, 0, sideWidth / 2 + wallDepthOffset),
            "cornerFrontLeft"
        );
        this.createCorner(scene, height,
            new BABYLON.Vector3(frontWidth / 2 + wallDepthOffset, 0, sideWidth / 2 + wallDepthOffset),
            "cornerFrontRight"
        );
        this.createCorner(scene, height,
            new BABYLON.Vector3(-frontWidth / 2 - wallDepthOffset, 0, -sideWidth / 2 - wallDepthOffset),
            "cornerBackLeft"
        );
        this.createCorner(scene, height,
            new BABYLON.Vector3(frontWidth / 2 + wallDepthOffset, 0, -sideWidth / 2 - wallDepthOffset),
            "cornerBackRight"
        );

        const camera = new BABYLON.ArcRotateCamera("camera",
            Math.PI / 4,
            Math.PI / 3,
            15,
            BABYLON.Vector3.Zero(),
            scene
        );
        camera.attachControl(canvas, true);
        camera.lowerRadiusLimit = 5;
        camera.upperRadiusLimit = 30;

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

// Exposition de l'objet babylonInterop dans la fenêtre globale
window.babylonInterop = babylonInterop;