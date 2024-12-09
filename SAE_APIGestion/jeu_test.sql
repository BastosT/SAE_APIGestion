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

-- 3. Murs pour D101 et D102 (sans sal_id)
INSERT INTO t_e_mur_mur (mur_id, mur_nom, mur_longueur, mur_hauteur, mur_type, sal_id)
VALUES 
    (1, 'Mur Face', 575, 270, 1, null),
    (2, 'Mur Entree', 575, 270, 2, null),
    (3, 'Mur Droite', 736, 270, 3, null),
    (4, 'Mur Gauche', 736, 270, 4, null),
    (5, 'Mur Face', 775, 270, 1, null),
    (6, 'Mur Entree', 775, 270, 2, null),
    (7, 'Mur Droite', 936, 270, 3, null),
    (8, 'Mur Gauche', 936, 270, 4, null);
SELECT setval('t_e_mur_mur_mur_id_seq', (SELECT MAX(mur_id) FROM t_e_mur_mur));

-- 4. Salles
INSERT INTO t_e_salle_sal (sal_id, sal_nom, sal_surface, tys_id, bat_id, mur_faceid, mur_entreeid, mur_droiteid, mur_gaucheid)
VALUES 
    (1, 'D101', 10, 1, 1, 1, 2, 3, 4),
    (2, 'D102', 5, 1, 1, 5, 6, 7, 8);
SELECT setval('t_e_salle_sal_sal_id_seq', (SELECT MAX(sal_id) FROM t_e_salle_sal));


-- Mise à jour des murs avec leur sal_id
UPDATE t_e_mur_mur SET sal_id = 1 WHERE mur_id IN (1, 2, 3, 4);
UPDATE t_e_mur_mur SET sal_id = 2 WHERE mur_id IN (5, 6, 7, 8);

-- 5. Capteurs
INSERT INTO t_e_capteur_cap (
    cap_id, cap_nom, cap_estactif, 
    cap_distancefenetre, cap_longueur, cap_hauteur, 
    cap_positionx, cap_positiony, 
    cap_distanceporte, cap_distancechauffage,
    sal_id, mur_id
)
VALUES 
    (1, 'Capteur D1', true, 0, 15, 15, 178, 98, 0, 0, 1, 3),
    (2, 'Capteur D2', true, 0, 10, 10, 585, 161, 0, 0, 1, 3),
    (3, 'Capteur D1', true, 0, 10, 10, 316, 70, 0, 0, 1, 4),
    (4, 'Capteur D2', true, 0, 10, 10, 662, 156, 0, 0, 1, 4);
SELECT setval('t_e_capteur_cap_cap_id_seq', (SELECT MAX(cap_id) FROM t_e_capteur_cap));

-- 6. Equipements
INSERT INTO t_e_equipement_equ (
    equ_id, equ_nom, equ_hauteur, equ_longueur, 
    equ_positionx, equ_positiony, 
    tye_id, mur_id, sal_id
)
VALUES 
    -- Radiateurs
    (1, 'Radiateur 1', 80, 100, 34, 180, 1, 1, 1),
    (2, 'Radiateur 2', 80, 100, 256, 180, 1, 1, 1),
    -- Fenetres
    (3, 'Fenetre 1', 165, 100, 6, 3, 2, 1, 1),
    (4, 'Fenetre 2', 165, 100, 345, 3, 2, 1, 1),
    -- Vitres
    (5, 'Fenetre 1', 161, 89, 125, 6, 3, 1, 1),
    (6, 'Fenetre 2', 161, 89, 237, 6, 3, 1, 1),
    (7, 'Fenetre 3', 161, 89, 482, 6, 3, 1, 1),
    -- Porte
    (8, 'Porte', 205, 93, 55, 67, 4, 2, 1);
SELECT setval('t_e_equipement_equ_equ_id_seq', (SELECT MAX(equ_id) FROM t_e_equipement_equ));

-- 7. Données des capteurs (exemple avec quelques valeurs)
INSERT INTO t_e_donneescapteur_dcp (dcp_id, cap_id, tdc_id, dcp_valeur, dcp_timestamp)
VALUES 
    (1, 1, 1, 21.5, CURRENT_TIMESTAMP),
    (2, 1, 2, 45.0, CURRENT_TIMESTAMP),
    (3, 2, 1, 22.0, CURRENT_TIMESTAMP),
    (4, 2, 2, 46.0, CURRENT_TIMESTAMP);
SELECT setval('t_e_donneescapteur_dcp_dcp_id_seq', (SELECT MAX(dcp_id) FROM t_e_donneescapteur_dcp));