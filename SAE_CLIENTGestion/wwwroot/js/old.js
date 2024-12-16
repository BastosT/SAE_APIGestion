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