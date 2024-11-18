window.babylonInterop = {
    config: {
        building: {
            width: 20,
            depth: 15,
            height: 8,
            wallThickness: 0.5
        },
        room: {
            minSize: 4,
            wallHeight: 7.5,
            wallThickness: 0.3
        },
        equipment: {
            size: 1,
            height: 0.1
        }
    },

    createWall: function (scene, width, height, depth, position, name = "wall") {
        const wall = BABYLON.MeshBuilder.CreateBox(name, {
            width: width,
            height: height,
            depth: depth,
            updatable: false
        }, scene);

        wall.position = position;

        // Créer un matériau unique pour chaque mur
        const wallMaterial = new BABYLON.StandardMaterial(name + "Material", scene);
        wallMaterial.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
        wallMaterial.backFaceCulling = true;
        wall.material = wallMaterial;

        return wall;
    },

    createRoom: function (scene, width, depth, position, name = "room") {
        const room = new BABYLON.TransformNode(name, scene);
        const config = this.config.room;

        // Créer les murs de la pièce
        const northWall = this.createWall(scene,
            width,
            config.wallHeight,
            config.wallThickness,
            new BABYLON.Vector3(position.x, config.wallHeight / 2, position.z + depth / 2),
            `${name}NorthWall`
        );

        const southWall = this.createWall(scene,
            width,
            config.wallHeight,
            config.wallThickness,
            new BABYLON.Vector3(position.x, config.wallHeight / 2, position.z - depth / 2),
            `${name}SouthWall`
        );

        const eastWall = this.createWall(scene,
            config.wallThickness,
            config.wallHeight,
            depth,
            new BABYLON.Vector3(position.x + width / 2, config.wallHeight / 2, position.z),
            `${name}EastWall`
        );

        const westWall = this.createWall(scene,
            config.wallThickness,
            config.wallHeight,
            depth,
            new BABYLON.Vector3(position.x - width / 2, config.wallHeight / 2, position.z),
            `${name}WestWall`
        );

        [northWall, southWall, eastWall, westWall].forEach(wall => {
            wall.parent = room;
        });

        return room;
    },

    createEquipment: function (scene, position, name = "equipment") {
        const config = this.config.equipment;
        const equipment = BABYLON.MeshBuilder.CreateBox(name, {
            width: config.size,
            height: config.height,
            depth: config.size,
            updatable: false
        }, scene);

        equipment.position = position;

        const equipmentMaterial = new BABYLON.StandardMaterial(name + "Material", scene);
        equipmentMaterial.diffuseColor = new BABYLON.Color3(0.3, 0.5, 0.9);
        equipment.material = equipmentMaterial;

        return equipment;
    },

    createBuilding: function (scene, position, buildingIndex) {
        const building = new BABYLON.TransformNode(`building${buildingIndex}`, scene);
        const config = this.config.building;

        // Créer les murs extérieurs
        const northWall = this.createWall(scene,
            config.width,
            config.height,
            config.wallThickness,
            new BABYLON.Vector3(position.x, config.height / 2, position.z + config.depth / 2),
            `building${buildingIndex}NorthWall`
        );

        const southWall = this.createWall(scene,
            config.width,
            config.height,
            config.wallThickness,
            new BABYLON.Vector3(position.x, config.height / 2, position.z - config.depth / 2),
            `building${buildingIndex}SouthWall`
        );

        const eastWall = this.createWall(scene,
            config.wallThickness,
            config.height,
            config.depth,
            new BABYLON.Vector3(position.x + config.width / 2, config.height / 2, position.z),
            `building${buildingIndex}EastWall`
        );

        const westWall = this.createWall(scene,
            config.wallThickness,
            config.height,
            config.depth,
            new BABYLON.Vector3(position.x - config.width / 2, config.height / 2, position.z),
            `building${buildingIndex}WestWall`
        );

        // Créer le sol
        const floor = BABYLON.MeshBuilder.CreateGround(`building${buildingIndex}Floor`, {
            width: config.width,
            height: config.depth,
            updatable: false
        }, scene);

        floor.position = new BABYLON.Vector3(position.x, 0, position.z);

        const floorMaterial = new BABYLON.StandardMaterial(`building${buildingIndex}FloorMaterial`, scene);
        floorMaterial.diffuseColor = new BABYLON.Color3(0.9, 0.9, 0.9);
        floor.material = floorMaterial;

        // Rattacher tous les éléments au bâtiment
        [northWall, southWall, eastWall, westWall, floor].forEach(element => {
            element.parent = building;
        });

        // Créer les pièces avec des noms uniques
        const room1 = this.createRoom(scene, 8, 6,
            new BABYLON.Vector3(position.x - 5, 0, position.z - 3),
            `building${buildingIndex}Room1`
        );

        const room2 = this.createRoom(scene, 8, 6,
            new BABYLON.Vector3(position.x + 5, 0, position.z - 3),
            `building${buildingIndex}Room2`
        );

        const room3 = this.createRoom(scene, 8, 6,
            new BABYLON.Vector3(position.x, 0, position.z + 3),
            `building${buildingIndex}Room3`
        );

        [room1, room2, room3].forEach(room => {
            room.parent = building;
        });

        // Ajouter les équipements avec des noms uniques
        const equipment1 = this.createEquipment(scene,
            new BABYLON.Vector3(position.x - 5, 0.05, position.z - 3),
            `building${buildingIndex}Equipment1`
        );

        const equipment2 = this.createEquipment(scene,
            new BABYLON.Vector3(position.x + 5, 0.05, position.z - 3),
            `building${buildingIndex}Equipment2`
        );

        const equipment3 = this.createEquipment(scene,
            new BABYLON.Vector3(position.x, 0.05, position.z + 3),
            `building${buildingIndex}Equipment3`
        );

        [equipment1, equipment2, equipment3].forEach(equipment => {
            equipment.parent = building;
        });

        return building;
    },

    initializeScene: function (canvasId) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) {
            console.error('Canvas not found:', canvasId);
            return;
        }
        if (typeof BABYLON === 'undefined') {
            console.error('BABYLON is not loaded');
            return;
        }

        const engine = new BABYLON.Engine(canvas, true);
        const scene = new BABYLON.Scene(engine);

        // Configuration de la caméra
        const camera = new BABYLON.ArcRotateCamera("camera",
            -Math.PI / 4,
            Math.PI / 3,
            50,
            BABYLON.Vector3.Zero(),
            scene);
        camera.attachControl(canvas, true);
        camera.setPosition(new BABYLON.Vector3(40, 40, -40));
        camera.lowerBetaLimit = 0.1;
        camera.upperBetaLimit = Math.PI / 2;

        // Éclairage
        const hemisphericLight = new BABYLON.HemisphericLight("hemisphericLight",
            new BABYLON.Vector3(0, 1, 0),
            scene);
        hemisphericLight.intensity = 0.7;

        const directionalLight = new BABYLON.DirectionalLight("directionalLight",
            new BABYLON.Vector3(-1, -2, -1),
            scene);
        directionalLight.intensity = 0.5;

        // Créer les bâtiments avec des indices uniques
        this.createBuilding(scene, new BABYLON.Vector3(-25, 0, -25), 1);
        this.createBuilding(scene, new BABYLON.Vector3(25, 0, -25), 2);
        this.createBuilding(scene, new BABYLON.Vector3(-25, 0, 25), 3);
        this.createBuilding(scene, new BABYLON.Vector3(25, 0, 25), 4);

        engine.runRenderLoop(function () {
            scene.render();
        });

        window.addEventListener("resize", function () {
            engine.resize();
        });

        return scene;
    }
};