export function initializeD3(element, data) {
    // S�lectionner le conteneur
    const svg = d3.select(element)
        .append('svg')
        .attr('width', '100%')
        .attr('height', '100%');

    // Cr�er un groupe pour le drag
    const g = svg.append('g');

    // D�finir le zoom
    const zoom = d3.zoom()
        .scaleExtent([0.1, 4])
        .on('zoom', (event) => {
            g.attr('transform', event.transform);
        });

    svg.call(zoom);

    // Fonction pour le drag des b�timents
    const dragBuilding = d3.drag()
        .on('start', dragStarted)
        .on('drag', dragged)
        .on('end', dragEnded);

    // Dessiner les b�timents
    const buildings = g.selectAll('.building')
        .data(data)
        .enter()
        .append('g')
        .attr('class', 'building')
        .attr('transform', d => `translate(${d.x},${d.y})`)
        .call(dragBuilding);

    // Rectangles des b�timents
    buildings.append('rect')
        .attr('width', d => d.width)
        .attr('height', d => d.height)
        .attr('fill', '#e3e3e3')
        .attr('stroke', '#666')
        .attr('stroke-width', 2)
        .attr('rx', 5);

    // Titres des b�timents
    buildings.append('text')
        .text(d => d.name)
        .attr('x', 10)
        .attr('y', 25)
        .attr('fill', '#333')
        .attr('font-size', '14px');

    // Dessiner les salles pour chaque b�timent
    buildings.each(function(building) {
        const buildingGroup = d3.select(this);
        
        const rooms = buildingGroup.selectAll('.room')
            .data(building.rooms)
            .enter()
            .append('g')
            .attr('class', 'room')
            .attr('transform', d => `translate(${d.x},${d.y})`);

        // Rectangles des salles
        rooms.append('rect')
            .attr('width', d => d.width)
            .attr('height', d => d.height)
            .attr('fill', '#fff')
            .attr('stroke', '#999')
            .attr('stroke-width', 1)
            .attr('rx', 3);

        // Noms des salles
        rooms.append('text')
            .text(d => `${d.name} (${d.type})`)
            .attr('x', 5)
            .attr('y', 15)
            .attr('fill', '#666')
            .attr('font-size', '12px');
    });

    function dragStarted(event, d) {
        d3.select(this).raise().classed('active', true);
    }

    function dragged(event, d) {
        d3.select(this)
            .attr('transform', `translate(${event.x},${event.y})`);
        d.x = event.x;
        d.y = event.y;
    }

    function dragEnded(event, d) {
        d3.select(this).classed('active', false);
    }

    // Ajouter cette fonction � la fin du fichier d3Visualizer.js
    export function updateVisualization(data) {
        // S�lectionner le SVG existant
        const svg = d3.select('#d3-container svg');
        const g = svg.select('g');

        // Mettre � jour les b�timents
        const buildings = g.selectAll('.building')
            .data(data, d => d.id);

        // Supprimer les anciens b�timents
        buildings.exit().remove();

        // Ajouter les nouveaux b�timents
        const newBuildings = buildings.enter()
            .append('g')
            .attr('class', 'building')
            .attr('transform', d => `translate(${d.x},${d.y})`)
            .call(dragBuilding);

        // Ajouter les rectangles pour les nouveaux b�timents
        newBuildings.append('rect')
            .attr('width', d => d.width)
            .attr('height', d => d.height)
            .attr('fill', '#e3e3e3')
            .attr('stroke', '#666')
            .attr('stroke-width', 2)
            .attr('rx', 5);

        // Ajouter les titres pour les nouveaux b�timents
        newBuildings.append('text')
            .text(d => d.name)
            .attr('x', 10)
            .attr('y', 25)
            .attr('fill', '#333')
            .attr('font-size', '14px');

        // Mettre � jour les propri�t�s des b�timents existants
        buildings.select('rect')
            .attr('width', d => d.width)
            .attr('height', d => d.height);

        buildings.select('text')
            .text(d => d.name);
    }
}