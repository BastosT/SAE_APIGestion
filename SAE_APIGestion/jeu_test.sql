-- Suppression des données existantes (dans l'ordre inverse des dépendances)
DELETE FROM t_e_donneescapteur_dcp;
DELETE FROM t_e_equipement_equ;
DELETE FROM t_e_capteur_cap;
DELETE FROM t_e_salle_sal;
DELETE FROM t_e_mur_mur;
DELETE FROM t_e_batiment_bat;
DELETE FROM t_e_typedonneescapteur_tdc;
DELETE FROM t_e_typeequipement_tye;
DELETE FROM t_e_typesalle_tys;

-- 1. Types de base
INSERT INTO t_e_typesalle_tys (tys_id, tys_nom, tys_description)
VALUES (1, 'type salle', 'aucune');
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
-- Orientation: 0 = Horizontal, 1 = Vertical
INSERT INTO t_e_mur_mur (mur_id, mur_nom, mur_longueur, mur_hauteur, mur_orientation, sal_id)
VALUES 
    -- Partie principale
    (1, 'Mur Nord 1', 500, 270, 0, null),     -- Mur horizontal supérieur
    (2, 'Mur Est 1', 400, 270, 1, null),      -- Mur vertical droit
    (3, 'Mur Sud 1', 300, 270, 0, null),      -- Mur horizontal inférieur partie 1
    -- Extension en L
    (4, 'Mur Ouest', 600, 270, 1, null),      -- Mur vertical gauche
    (5, 'Mur Sud 2', 200, 270, 0, null),      -- Mur horizontal inférieur partie 2
    (6, 'Mur Est 2', 200, 270, 1, null);      -- Petit mur vertical droit de l'extension
SELECT setval('t_e_mur_mur_mur_id_seq', (SELECT MAX(mur_id) FROM t_e_mur_mur));

-- 4. Salle en L
INSERT INTO t_e_salle_sal (sal_id, sal_nom, sal_surface, tys_id, bat_id)
VALUES (1, 'D103', 15, 1, 1);
SELECT setval('t_e_salle_sal_sal_id_seq', (SELECT MAX(sal_id) FROM t_e_salle_sal));

-- Mise à jour des murs avec leur sal_id
UPDATE t_e_mur_mur SET sal_id = 1 WHERE mur_id IN (1, 2, 3, 4, 5, 6);

-- 5. Capteurs
INSERT INTO t_e_capteur_cap (
    cap_id, cap_nom, cap_estactif, 
    cap_distancefenetre, cap_longueur, cap_hauteur, 
    cap_positionx, cap_positiony, 
    cap_distanceporte, cap_distancechauffage,
    sal_id, mur_id
)
VALUES 
    (1, 'Capteur 1', true, 50, 15, 15, 200, 98, 150, 100, 1, 2),
    (2, 'Capteur 2', true, 0, 10, 10, 400, 161, 200, 150, 1, 4),
    (3, 'Capteur 3', true, 100, 10, 10, 150, 70, 100, 80, 1, 6);
SELECT setval('t_e_capteur_cap_cap_id_seq', (SELECT MAX(cap_id) FROM t_e_capteur_cap));

-- 6. Equipements
INSERT INTO t_e_equipement_equ (
    equ_id, equ_nom, equ_hauteur, equ_longueur, 
    equ_positionx, equ_positiony, 
    tye_id, mur_id, sal_id
)
VALUES 
    -- Radiateurs
    (1, 'Radiateur 1', 80, 100, 150, 180, 1, 1, 1),
    (2, 'Radiateur 2', 80, 100, 350, 180, 1, 3, 1),
    -- Fenetres
    (3, 'Fenetre 1', 165, 100, 100, 3, 2, 1, 1),
    (4, 'Fenetre 2', 165, 100, 300, 3, 2, 5, 1),
    -- Vitres
    (5, 'Vitre 1', 161, 89, 200, 6, 3, 2, 1),
    (6, 'Vitre 2', 161, 89, 50, 6, 3, 4, 1),
    -- Porte
    (7, 'Porte', 205, 93, 55, 67, 4, 6, 1);
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