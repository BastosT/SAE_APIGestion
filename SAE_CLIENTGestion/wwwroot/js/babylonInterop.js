(function () {
    console.log("Initializing simplified babylonInterop");

    window.babylonInterop = window.babylonInterop || {};

    Object.assign(window.babylonInterop, {
        config: {
            scale: 0.01,
            wallDepth: 0.10
        },

        createWall: function (scene, wallData) {
            const width = wallData.longueur * this.config.scale;
            const height = wallData.hauteur * this.config.scale;
            const wall = BABYLON.MeshBuilder.CreateBox("wall", {
                width: width,
                height: height,
                depth: this.config.wallDepth
            }, scene);

            const material = new BABYLON.StandardMaterial("wallMaterial", scene);
            material.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0.8);
            wall.material = material;

            return wall;
        },

        convertToNewModel: function (oldModel) {
            return oldModel.map(batiment => ({
                name: batiment.nom,
                rooms: (batiment.salles || []).map(salle => ({
                    name: salle.nom,
                    walls: (salle.murs || []).map(mur => ({
                        name: mur.nom,
                        longueur: mur.longueur,
                        hauteur: mur.hauteur,
                        orientation: mur.orientation === 0 ? 'Horizontal' : 'Vertical'
                    }))
                }))
            }));
        },

        initializeScene: function (canvasId, buildingsDataJson) {
            console.log("Initializing simple scene");

            const canvas = document.getElementById(canvasId);
            if (!canvas) throw new Error('Canvas not found');

            try {
                const parsedData = JSON.parse(buildingsDataJson);
                const buildingsData = this.convertToNewModel(parsedData);

                const engine = new BABYLON.Engine(canvas, true);
                window.currentEngine = engine;

                const scene = new BABYLON.Scene(engine);
                window.currentScene = scene;
                scene.clearColor = new BABYLON.Color3(0.3, 0.3, 0.9);

                let currentX = 0;

                // Parcourir tous les bâtiments
                buildingsData.forEach(building => {
                    // Parcourir toutes les pièces
                    building.rooms.forEach(room => {
                        // Parcourir tous les murs
                        room.walls.forEach(wallData => {
                            const wall = this.createWall(scene, wallData);

                            // Positionner le mur
                            wall.position.y = wallData.hauteur * this.config.scale / 2;

                            if (wallData.orientation === 'Horizontal') {
                                wall.position.x = currentX + (wallData.longueur * this.config.scale / 2);
                                currentX += wallData.longueur * this.config.scale + 1; // +1 pour l'espacement
                            } else {
                                wall.position.x = currentX + (wallData.longueur * this.config.scale / 2);
                                wall.rotation.y = Math.PI / 2; // Rotation 90 degrés
                                currentX += wallData.longueur * this.config.scale + 1;
                            }
                        });
                    });
                });

                // Caméra
                const camera = new BABYLON.ArcRotateCamera("camera",
                    Math.PI / 4,
                    Math.PI / 3,
                    20,
                    BABYLON.Vector3.Zero(),
                    scene
                );
                camera.attachControl(canvas, true);

                // Lumière
                const light = new BABYLON.HemisphericLight("light",
                    new BABYLON.Vector3(0, 1, 0),
                    scene
                );
                light.intensity = 0.8;

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

    console.log("Simple babylonInterop initialization complete");
})();