var babylonInterop = {
    config: {
        scale: 0.01,
        wallDepth: 0.3,
        equipmentDepth: 0.1,
        radiatorOffset: 0.05  // Offset from wall bottom
    },

    createEquipment: function (scene, equipment, parentWall, wallWidth, wallHeight, isInterior = true) {
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
            xPos - (wallWidth / 2) + (width / 2),
            (wallHeight / 2) - yPos - (height / 2),
            this.config.wallDepth
        );

        const material = new BABYLON.StandardMaterial(equipment.nom + "Material", scene);
        if (equipment.nom.toLowerCase().includes("fenetre") || equipment.nom.toLowerCase().includes("vitre")) {
            material.diffuseColor = new BABYLON.Color3(0.7, 0.9, 1);
            material.alpha = 0.5;
            material.transparencyMode = BABYLON.Material.MATERIAL_ALPHABLEND;
            material.backFaceCulling = false;
        } else if (equipment.nom.toLowerCase().includes("radiateur")) {
            material.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
            material.metallic = 0.8;
            material.roughness = 0.3;
        }

        equipmentMesh.material = material;
        equipmentMesh.parent = parentWall;

        return equipmentMesh;
    },
    createWall: function (scene, wallData, position, rotation, isInterior = true) {
        console.log('Creating wall with data:', wallData);

        const width = wallData.largeur * this.config.scale;
        const height = wallData.hauteur * this.config.scale;
        const depth = this.config.wallDepth;

        const wall = BABYLON.MeshBuilder.CreateBox("wall", {
            width: width,
            height: height,
            depth: depth,
            updatable: false
        }, scene);

        wall.position = position;
        if (rotation) {
            wall.rotation = rotation;
        }

        const wallMaterial = new BABYLON.StandardMaterial("wallMaterial", scene);
        wallMaterial.diffuseColor = new BABYLON.Color3(0.95, 0.95, 0.95);
        wallMaterial.roughness = 0.8;
        wall.material = wallMaterial;

        if (wallData.equipements && wallData.equipements.length > 0) {
            console.log('Processing', wallData.equipements.length, 'equipment items');
            wallData.equipements.forEach(equipment => {
                this.createEquipment(scene, equipment, wall, width, height, isInterior);
            });
        } else {
            console.log('No equipment to process');
        }

        return wall;
    },

    createRoom: function (scene, roomData) {
        console.log('Creating room with data:', roomData);

        const room = new BABYLON.TransformNode("room", scene);
        const wallHeight = roomData.murFace.hauteur * this.config.scale;
        const wallWidth = roomData.murFace.largeur * this.config.scale;

        // Position le mur face à la caméra
        const frontWall = this.createWall(
            scene,
            roomData.murFace,
            new BABYLON.Vector3(0, wallHeight / 2, 0),
            new BABYLON.Vector3(0, Math.PI, 0), // Rotation pour que la face avant soit visible
            true
        );

        frontWall.parent = room;
        return room;
    },

    initializeScene: function (canvasId, roomDataJson) {
        try {
            console.log('Initializing scene with data:', roomDataJson);

            const canvas = document.getElementById(canvasId);
            if (!canvas) {
                throw new Error('Canvas not found: ' + canvasId);
            }

            // Parse JSON si nécessaire
            const roomData = roomDataJson ?
                (typeof roomDataJson === 'string' ? JSON.parse(roomDataJson) : roomDataJson)
                : {
                    murFace: {
                        largeur: 575,
                        hauteur: 270,
                        equipements: []
                    }
                };

            console.log('Parsed room data:', roomData);

            const engine = new BABYLON.Engine(canvas, true);
            const scene = new BABYLON.Scene(engine);

            // Position la caméra pour voir la face avant du mur
            const camera = new BABYLON.ArcRotateCamera("camera",
                Math.PI,
                Math.PI / 2,
                15,
                new BABYLON.Vector3(0, roomData.murFace.hauteur * this.config.scale / 2, 0),
                scene
            );
            camera.attachControl(canvas, true);
            camera.lowerRadiusLimit = 5;
            camera.upperRadiusLimit = 30;
            camera.wheelDeltaPercentage = 0.01;

            // Éclairage dirigé vers la face avant du mur
            const hemisphericLight = new BABYLON.HemisphericLight("hemisphericLight",
                new BABYLON.Vector3(0, 1, -1),
                scene
            );
            hemisphericLight.intensity = 0.7;

            const pointLight = new BABYLON.PointLight("pointLight",
                new BABYLON.Vector3(0, roomData.murFace.hauteur * this.config.scale, -5),
                scene
            );
            pointLight.intensity = 0.5;

            this.createRoom(scene, roomData);

            // Ajout d'axes de référence pour debug
            const showAxis = function (size) {
                const axisX = BABYLON.MeshBuilder.CreateLines("axisX", {
                    points: [
                        new BABYLON.Vector3.Zero(),
                        new BABYLON.Vector3(size, 0, 0)
                    ]
                }, scene);
                axisX.color = new BABYLON.Color3(1, 0, 0);

                const axisY = BABYLON.MeshBuilder.CreateLines("axisY", {
                    points: [
                        new BABYLON.Vector3.Zero(),
                        new BABYLON.Vector3(0, size, 0)
                    ]
                }, scene);
                axisY.color = new BABYLON.Color3(0, 1, 0);

                const axisZ = BABYLON.MeshBuilder.CreateLines("axisZ", {
                    points: [
                        new BABYLON.Vector3.Zero(),
                        new BABYLON.Vector3(0, 0, size)
                    ]
                }, scene);
                axisZ.color = new BABYLON.Color3(0, 0, 1);
            };
            showAxis(10);

            engine.runRenderLoop(() => {
                scene.render();
            });

            window.addEventListener("resize", () => {
                engine.resize();
            });

            console.log('Scene initialized successfully');
            return scene;

        } catch (error) {
            console.error('Error in initializeScene:', error);
            throw error;
        }
    }
};

window.babylonInterop = babylonInterop;