-- Suppression des données existantes (dans l'ordre inverse des dépendances)

TRUNCATE t_e_donneescapteur_dcp CASCADE;
TRUNCATE t_e_equipement_equ CASCADE;
TRUNCATE t_e_capteur_cap CASCADE;
TRUNCATE t_e_salle_sal CASCADE;
TRUNCATE t_e_mur_mur CASCADE;
TRUNCATE t_e_batiment_bat CASCADE;
TRUNCATE t_e_typedonneescapteur_tdc CASCADE;
TRUNCATE t_e_typeequipement_tye CASCADE;
TRUNCATE t_e_typesalle_tys CASCADE;

-- 1. Types de base
INSERT INTO t_e_typesalle_tys (tys_id, tys_nom, tys_description)
VALUES (1, 'Salle de pause', 'Aucune...');
SELECT setval('t_e_typesalle_tys_tys_id_seq', (SELECT MAX(tys_id) FROM t_e_typesalle_tys));

INSERT INTO t_e_typeequipement_tye (tye_id, tye_nom)
VALUES 
    (1, 'Radiateur'),
    (2, 'Fenetre'),
    (3, 'Vitre'),
    (4, 'Porte');
SELECT setval('t_e_typeequipement_tye_tye_id_seq', (SELECT MAX(tye_id) FROM t_e_typeequipement_tye));

INSERT INTO t_e_typedonneescapteur_tdc (tdc_id, tdc_nom, tdc_unite)
VALUES 
    (1, 'Temperature', 'Celsius'),
    (2, 'Humidite', '%'),
    (3, 'CO2', 'ppm');
SELECT setval('t_e_typedonneescapteur_tdc_tdc_id_seq', (SELECT MAX(tdc_id) FROM t_e_typedonneescapteur_tdc));

-- 2. Batiments
INSERT INTO t_e_batiment_bat (bat_id, bat_nom, bat_adresse)
VALUES 
    (1, 'Bat D', 'Batiment informatique.'),
    (2, 'Bat F', 'Batiment GEA');
SELECT setval('t_e_batiment_bat_bat_id_seq', (SELECT MAX(bat_id) FROM t_e_batiment_bat));

-- 3. Murs pour la salle en L (D103)
-- Orientation: 0 = Nord, 1 = Est, 2 = Sud, 3 = Ouest 
INSERT INTO t_e_mur_mur (mur_id, mur_nom, mur_longueur, mur_hauteur, mur_orientation, sal_id)
VALUES 
    -- D103 : Salle en L
    (1, 'M1', 401.0, 208.5, 0, null),
    (2, 'M6', 428.5, 208.5, 1, null),
    (3, 'M5', 243.5, 208.5, 2, null),
    (4, 'M4', 208.5, 208.5, 3, null),
    (5, 'M3', 157.5, 208.5, 2, null),
    (6, 'M2', 220.0, 208.5, 3, null),

    -- D104 : Salle rectangulaire simple
    (7, 'Mur Nord', 575, 270, 0, null),     -- Mur du haut
    (8, 'Mur Ouest', 736, 270, 1, null),   -- Mur de gauche
    (9, 'Mur Sud', 575, 270, 2, null),      -- Mur du bas
    (10, 'Mur Est', 736, 270, 3, null);      -- Mur de droite
SELECT setval('t_e_mur_mur_mur_id_seq', (SELECT MAX(mur_id) FROM t_e_mur_mur));

-- 4. Salle en L
INSERT INTO t_e_salle_sal (sal_id, sal_nom, sal_surface, tys_id, bat_id)
VALUES 
    (1, 'D103', 15, 1, 1),
    (2, 'D104', 12, 1, 1);

SELECT setval('t_e_salle_sal_sal_id_seq', (SELECT MAX(sal_id) FROM t_e_salle_sal));

-- Mise à jour des murs avec leur sal_id
UPDATE t_e_mur_mur SET sal_id = 1 WHERE mur_id IN (1, 2, 3, 4, 5, 6);
UPDATE t_e_mur_mur SET sal_id = 2 WHERE mur_id IN (7, 8, 9, 10);

-- 5. Capteurs
INSERT INTO t_e_capteur_cap (
    cap_id, cap_nom, cap_estactif, 
    cap_distancefenetre, cap_longueur, cap_hauteur, 
    cap_positionx, cap_positiony, 
    cap_distanceporte, cap_distancechauffage,
    sal_id, mur_id
)
VALUES 
    (1, 'Capteur 1', true, 50, 15, 15, 200, 98, 150, 100, 1, 8),
    (2, 'Capteur 2', true, 0, 10, 10, 400, 161, 200, 150, 1, 8),
    (3, 'DEBUG CAPTEUR', true, 100, 30, 30, 0, 10, 100, 80, 1, 10),

    (4, 'DEBUG CAPTEUR', true, 100, 30, 30, 0, 10, 100, 80, 1, 1),
    (5, 'DEBUG CAPTEUR', true, 100, 30, 30, 0, 10, 100, 80, 1, 2),
    (6, 'DEBUG CAPTEUR', true, 100, 30, 30, 0, 10, 100, 80, 1, 3),
    (7, 'DEBUG CAPTEUR', true, 100, 30, 30, 0, 10, 100, 80, 1, 4),
    (8, 'DEBUG CAPTEUR', true, 100, 30, 30, 0, 10, 100, 80, 1, 5),
    (9, 'DEBUG CAPTEUR', true, 100, 30, 30, 0, 10, 100, 80, 1, 6);

SELECT setval('t_e_capteur_cap_cap_id_seq', (SELECT MAX(cap_id) FROM t_e_capteur_cap));

-- 6. Equipements
INSERT INTO t_e_equipement_equ (
    equ_id, equ_nom, equ_hauteur, equ_longueur, 
    equ_positionx, equ_positiony, 
    tye_id, mur_id, sal_id
)
VALUES 
    -- Radiateurs
    (1, 'Radiateur 1', 80, 100, 34, 180, 1, 9, 1),
    (2, 'Radiateur 2', 80, 100, 256, 180, 1, 9, 1),
    -- Fenetres
    (3, 'Fenetre 1', 165, 100, 6, 3, 2, 9, 1),
    (4, 'Fenetre 2', 165, 100, 345, 3, 2, 9, 1),
    -- Vitres
    (5, 'Vitre 1', 161, 89, 125, 6, 3, 9, 1),
    (6, 'Vitre 2', 161, 89, 237, 6, 3, 9, 1),
    (7, 'Vitre 3', 161, 89, 482, 6, 3, 9, 1),
    -- Porte
    (8, 'Porte', 205, 93, 55, 67, 4, 7, 2);
SELECT setval('t_e_equipement_equ_equ_id_seq', (SELECT MAX(equ_id) FROM t_e_equipement_equ));

-- 7. Données des capteurs
INSERT INTO t_e_donneescapteur_dcp (dcp_id, cap_id, tdc_id, dcp_valeur, dcp_timestamp)
VALUES 
    (1, 1, 1, 21.5, CURRENT_TIMESTAMP),
    (2, 1, 2, 45.0, CURRENT_TIMESTAMP),
    (3, 2, 1, 22.0, CURRENT_TIMESTAMP),
    (4, 2, 2, 46.0, CURRENT_TIMESTAMP),
    (5, 3, 1, 21.8, CURRENT_TIMESTAMP),
    (6, 3, 2, 44.5, CURRENT_TIMESTAMP);
SELECT setval('t_e_donneescapteur_dcp_dcp_id_seq', (SELECT MAX(dcp_id) FROM t_e_donneescapteur_dcp));