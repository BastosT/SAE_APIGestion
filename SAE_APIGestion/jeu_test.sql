-- Batiments
INSERT INTO t_e_batiment_bat (bat_nom, bat_adresse) VALUES
('Bâtiment A', '1 Rue des Sciences'),
('Bâtiment B', '2 Rue des Technologies');

-- Types de salle
INSERT INTO t_e_typesalle_tys (tys_nom, tys_description) VALUES 
('TD', 'Salle de travaux dirigés'),
('TP', 'Salle de travaux pratiques'),
('Amphi', 'Amphithéâtre');

-- Salles
INSERT INTO t_e_salle_sal (sal_nom, sal_surface, tys_id, bat_id) VALUES
('A101', 50.0, 1, 1),
('A102', 75.0, 2, 1),
('B201', 100.0, 3, 2);

-- Murs
INSERT INTO t_e_mur_mur (mur_nom, mur_longueur, mur_hauteur, sal_id) VALUES
('Mur Nord A101', 10.0, 3.0, 1),
('Mur Est A101', 5.0, 3.0, 1),
('Mur Nord A102', 15.0, 3.0, 2);

-- Types d'équipement
INSERT INTO t_e_typeequipement_tye (tye_nom) VALUES
('Tableau'),
('Vidéoprojecteur'),
('Climatisation');

-- Équipements
INSERT INTO t_e_equipement_equ (equ_nom, tye_id, equ_largeur, equ_hauteur, equ_positionx, equ_positiony, mur_id, sal_id) VALUES
('Tableau A101', 1, 2.0, 1.2, 1.5, 1.0, 1, 1),
('VP A101', 2, 0.5, 0.3, 2.0, 2.5, 1, 1),
('Clim A102', 3, 0.8, 0.4, 1.0, 2.2, 3, 2);

-- Types de données capteur
INSERT INTO t_e_typedonneescapteur_tdc (tdc_nom, tdc_unite) VALUES
('Température', '°C'),
('Humidité', '%'),
('CO2', 'ppm');

-- Capteurs
INSERT INTO t_e_capteur_cap (cap_estactif, cap_distancefenetre, cap_distanceporte, cap_distancechauffage, sal_id, mur_id) VALUES
(true, 2.5, 1.8, 3.0, 1, 1),
(true, 3.0, 2.0, 2.5, 2, 3);

-- Données capteurs
INSERT INTO t_e_donneescapteur_dcp (cap_id, tdc_id, dc_valeur, dc_timestamp) VALUES
(1, 1, 21.5, '2024-11-15 10:00:00'),
(1, 2, 45.0, '2024-11-15 10:00:00'),
(2, 1, 22.0, '2024-11-15 10:00:00'),
(2, 3, 800.0, '2024-11-15 10:00:00');