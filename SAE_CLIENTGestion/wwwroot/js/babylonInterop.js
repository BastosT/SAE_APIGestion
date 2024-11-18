var babylonInterop = {
    config: {
        scale: 0.01,
        wallDepth: 0.3,
        equipmentDepth: 0.1,
    },
    createEquipment: function (scene, equipment, parentWall, wallWidth, wallHeight, isVerticalWall) {
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

        if (isVerticalWall) {
            const zPosition = -wallWidth / 2 + xPos + width / 2;
            equipmentMesh.position = new BABYLON.Vector3(
                -(this.config.wallDepth / 2 + depth / 2),  // Inversé le signe pour l'intérieur
                wallHeight / 2 - yPos - height / 2,
                zPosition
            );
        } else {
            const xPosition = -wallWidth / 2 + xPos + width / 2;
            equipmentMesh.position = new BABYLON.Vector3(
                xPosition,
                wallHeight / 2 - yPos - height / 2,
                -(this.config.wallDepth / 2 + depth / 2)  // Inversé le signe pour l'intérieur
            );
        }

        const material = new BABYLON.StandardMaterial(equipment.nom + "Material", scene);
        if (equipment.nom.toLowerCase().includes("radiateur")) {
            material.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
            material.metallic = 0.8;
            material.roughness = 0.3;
        } else {
            material.diffuseColor = new BABYLON.Color3(0.9, 0.9, 0.9);
        }
        equipmentMesh.material = material;
        equipmentMesh.parent = parentWall;
        return equipmentMesh;
    },
    createWall: function (scene, wallData, position, rotation) {
        const isVerticalWall = Math.abs(rotation.y) === Math.PI / 2;
        const width = wallData.largeur * this.config.scale;
        const height = wallData.hauteur * this.config.scale;
        const depth = this.config.wallDepth;

        let wallDimensions;
        if (isVerticalWall) {
            wallDimensions = {
                width: width,
                height: height,
                depth: depth
            };
        } else {
            wallDimensions = {
                width: width,
                height: height,
                depth: depth
            };
        }

        const wall = BABYLON.MeshBuilder.CreateBox("wall", wallDimensions, scene);

        if (isVerticalWall) {
            // For vertical walls, rotate them 90 degrees after creation
            wall.rotation = new BABYLON.Vector3(0, rotation.y, 0);
        } else {
            wall.rotation = rotation;
        }

        wall.position = position;

        const wallMaterial = new BABYLON.StandardMaterial("wallMaterial", scene);
        wallMaterial.diffuseColor = new BABYLON.Color3(0.95, 0.95, 0.95);
        wallMaterial.roughness = 0.8;
        wall.material = wallMaterial;

        if (wallData.equipements?.length > 0) {
            wallData.equipements.forEach(equipment => {
                this.createEquipment(scene, equipment, wall, width, height, isVerticalWall);
            });
        }
        return wall;
    },
    initializeScene: function (canvasId, roomDataJson) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) throw new Error('Canvas not found');
        const roomData = typeof roomDataJson === 'string' ? JSON.parse(roomDataJson) : roomDataJson;

        const engine = new BABYLON.Engine(canvas, true);
        const scene = new BABYLON.Scene(engine);

        const frontWidth = roomData.murFace.largeur * this.config.scale;
        const sideWidth = roomData.murGauche.largeur * this.config.scale;
        const height = roomData.murFace.hauteur * this.config.scale;

        // Create walls with adjusted positions
        const wallDepthOffset = this.config.wallDepth / 2;

        // Front and entrance walls (horizontal)
        this.createWall(scene, roomData.murFace,
            new BABYLON.Vector3(0, 0, sideWidth / 2 + wallDepthOffset),
            new BABYLON.Vector3(0, 0, 0));

        this.createWall(scene, roomData.murEntree,
            new BABYLON.Vector3(0, 0, -sideWidth / 2 - wallDepthOffset),
            new BABYLON.Vector3(0, Math.PI, 0));

        // Side walls (vertical)
        this.createWall(scene, roomData.murGauche,
            new BABYLON.Vector3(-frontWidth / 2 - wallDepthOffset, 0, 0),
            new BABYLON.Vector3(0, -Math.PI / 2, 0));

        this.createWall(scene, roomData.murDroite,
            new BABYLON.Vector3(frontWidth / 2 + wallDepthOffset, 0, 0),
            new BABYLON.Vector3(0, Math.PI / 2, 0));

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

        engine.runRenderLoop(() => scene.render());
        window.addEventListener("resize", () => engine.resize());
        return scene;
    }
};
window.babylonInterop = babylonInterop;