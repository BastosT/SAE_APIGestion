var babylonInterop = {
    config: {
        scale: 0.01,
        wallDepth: 0.15,
        equipmentDepth: 0.1,
        floorHeight: 0.1
    },
    createEquipment: function (scene, equipment, parentWall, wallWidth, wallHeight) {
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

        // L'énumération est sérialisée en nombre, donc on utilise les valeurs numériques
        switch (equipment.type) {
            case 3: // Radiator
                material.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
                material.metallic = 0.8;
                material.roughness = 0.3;
                break;
            case 0: // Window
            case 1: // DoorWindow
                material.diffuseColor = new BABYLON.Color3(0.6, 0.8, 1.0);
                material.alpha = 0.5;
                break;
            case 2: // Door
                material.diffuseColor = new BABYLON.Color3(0.6, 0.4, 0.2);
                break;
            case 4: // Sensor
                material.diffuseColor = new BABYLON.Color3(1.0, 0.2, 0.2);
                break;
            default: // Other - 5
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
        floorMaterial.diffuseColor = new BABYLON.Color3(0.4, 0.4, 0.4); // Couleur plus foncée (gris foncé)
        floorMaterial.roughness = 0.9;
        floor.material = floorMaterial;

        return floor;
    },

    createWallWithEdges: function (scene, wallData, position, rotation, name, addEdges = false) {
        const wallContainer = new BABYLON.TransformNode("container-" + name, scene);
        wallContainer.position = position;

        const width = wallData.largeur * this.config.scale;
        const height = wallData.hauteur * this.config.scale;
        const depth = this.config.wallDepth;

        const wall = BABYLON.MeshBuilder.CreateBox(name, {
            width: width + (addEdges ? this.config.wallDepth * 2 : 0),
            height: height,
            depth: depth
        }, scene);

        // Création de deux matériaux pour les faces intérieures et extérieures
        const wallMaterialInside = new BABYLON.StandardMaterial(name + "MaterialInside", scene);
        wallMaterialInside.diffuseColor = new BABYLON.Color3(0.95, 0.95, 0.95);
        wallMaterialInside.roughness = 0.8;
        wallMaterialInside.backFaceCulling = true; // Ne montre que les faces intérieures

        const wallMaterialOutside = new BABYLON.StandardMaterial(name + "MaterialOutside", scene);
        wallMaterialOutside.diffuseColor = new BABYLON.Color3(0.95, 0.95, 0.95);
        wallMaterialOutside.alpha = 0.3; // Rend le mur semi-transparent de l'extérieur
        wallMaterialOutside.roughness = 0.8;
        wallMaterialOutside.backFaceCulling = true;

        // Créer une copie du mur pour la face extérieure
        const wallOutside = wall.clone(name + "Outside");
        wall.material = wallMaterialInside;
        wallOutside.material = wallMaterialOutside;

        // Inverser les normales de la face extérieure
        wallOutside.scaling.z = -1;

        wall.parent = wallContainer;
        wallOutside.parent = wallContainer;

        if (wallData.equipements?.length > 0) {
            wallData.equipements.forEach(equipment => {
                this.createEquipment(scene, equipment, wallContainer, width, height);
            });
        }

        wallContainer.rotation = rotation;

        return wallContainer;
    },

    createCorner: function (scene, height, position, name) {
        const corner = BABYLON.MeshBuilder.CreateBox(name, {
            width: this.config.wallDepth,
            height: height,
            depth: this.config.wallDepth
        }, scene);

        corner.position = position;

        // Même approche pour les coins
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

        // Créer le sol en premier pour qu'il soit en dessous
        this.createFloor(scene,
            roomData.murFace.largeur * this.config.scale,
            roomData.murGauche.largeur * this.config.scale,
            new BABYLON.Vector3(0, 0, 0),
            roomData.murFace.hauteur // Passage de la hauteur du mur face
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

        // Coins
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
window.babylonInterop = babylonInterop;