-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 22, 2025 at 11:53 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `cleo_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `appointments`
--

CREATE TABLE `appointments` (
  `appointment_id` int(11) NOT NULL,
  `client_id` int(11) NOT NULL,
  `employee_name` varchar(100) NOT NULL,
  `service` varchar(100) NOT NULL,
  `appointment_date` date NOT NULL,
  `start_time` time NOT NULL,
  `end_time` time NOT NULL,
  `status` enum('scheduled','completed','cancelled') DEFAULT 'scheduled',
  `notes` text DEFAULT NULL,
  `employee_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `appointments`
--

INSERT INTO `appointments` (`appointment_id`, `client_id`, `employee_name`, `service`, `appointment_date`, `start_time`, `end_time`, `status`, `notes`, `employee_id`) VALUES
(1, 1, 'Anna Kowalska', 'Strzyżenie damskie', '2024-03-04', '09:00:00', '09:30:00', 'scheduled', 'Klasyczne cięcie', 2),
(2, 2, 'Jan Nowak', 'Manicure', '2024-03-04', '10:00:00', '10:45:00', 'scheduled', 'Delikatne wykończenie', 3),
(3, 3, 'Ewa Zielińska', 'Masaż relaksacyjny', '2024-03-04', '11:00:00', '12:00:00', 'scheduled', 'Prośba o relaksacyjną muzykę', 7),
(4, 4, 'Piotr Wiśniewski', 'Barber', '2024-03-04', '12:30:00', '13:00:00', 'scheduled', 'Krótka broda', 6),
(5, 5, 'Maria Lewandowska', 'Masaż twarzy', '2024-03-04', '13:30:00', '14:30:00', 'scheduled', 'Wrażliwa cera', 7),
(6, 6, 'Anna Malinowska', 'Manicure', '2024-03-05', '09:00:00', '09:45:00', 'scheduled', NULL, 3),
(7, 7, 'Piotr Wiśniewski', 'Strzyżenie męskie', '2024-03-05', '10:00:00', '10:30:00', 'scheduled', 'Prośba o klasyczny styl', 2),
(8, 8, 'Katarzyna Nowicka', 'Masaż sportowy', '2024-03-05', '11:00:00', '12:00:00', 'scheduled', 'Prośba o większy nacisk', 7),
(9, 9, 'Tomasz Kowalczyk', 'Strzyżenie męskie', '2024-03-05', '12:30:00', '13:00:00', 'scheduled', NULL, 6),
(10, 10, 'Michał Zieliński', 'Manicure', '2024-03-05', '13:30:00', '14:15:00', 'scheduled', 'Prośba o delikatne wykończenie', 3),
(11, 11, 'Joanna Wiśniewska', 'Strzyżenie damskie', '2024-03-06', '09:00:00', '09:30:00', 'scheduled', 'Styl romantyczny', 2),
(12, 12, 'Karolina Lewandowska', 'Masaż relaksacyjny', '2024-03-06', '10:00:00', '11:00:00', 'scheduled', 'Prośba o aromaterapię', 7),
(13, 13, 'Adam Kowalski', 'Barber', '2024-03-06', '11:30:00', '12:00:00', 'scheduled', 'Krótka broda', 6),
(14, 14, 'Paulina Nowak', 'Manicure', '2024-03-06', '12:30:00', '13:15:00', 'scheduled', 'Zdobienie paznokci', 3),
(15, 15, 'Marcin Malinowski', 'Strzyżenie męskie', '2024-03-06', '14:00:00', '14:30:00', 'scheduled', NULL, 2),
(16, 16, 'Ewelina Kamińska', 'Masaż sportowy', '2024-03-07', '09:00:00', '10:00:00', 'scheduled', 'Prośba o relaksacyjną muzykę', 7),
(17, 17, 'Tomasz Lewandowski', 'Barber', '2024-03-07', '10:30:00', '11:00:00', 'scheduled', 'Delikatne cięcie', 6),
(18, 18, 'Agnieszka Zielińska', 'Manicure', '2024-03-07', '11:30:00', '12:15:00', 'scheduled', 'Subtelne zdobienie', 3),
(19, 19, 'Paweł Kwiatkowski', 'Strzyżenie męskie', '2024-03-07', '12:30:00', '13:00:00', 'scheduled', NULL, 2),
(20, 20, 'Ewa Kowalczyk', 'Masaż relaksacyjny', '2024-03-08', '09:00:00', '10:00:00', 'scheduled', 'Prośba o delikatny dotyk', 7),
(21, 21, 'Mateusz Nowakowski', 'Strzyżenie męskie', '2024-03-08', '10:30:00', '11:00:00', 'scheduled', 'Prośba o klasyczny styl', 2),
(22, 22, 'Monika Wiśniewska', 'Manicure', '2024-03-08', '11:30:00', '12:15:00', 'scheduled', NULL, 3),
(23, 23, 'Andrzej Kamiński', 'Barber', '2024-03-08', '12:30:00', '13:00:00', 'scheduled', 'Krótka broda', 6),
(24, 24, 'Magdalena Lewandowska', 'Masaż sportowy', '2024-03-08', '13:30:00', '14:30:00', 'scheduled', 'Mocniejsze techniki', 7),
(25, 25, 'Janina Zielińska', 'Manicure', '2024-03-09', '09:00:00', '09:45:00', 'scheduled', 'Delikatne wykończenie', 3),
(26, 1, 'Anna Kowalska', 'Masaż twarzy', '2024-03-09', '10:00:00', '11:00:00', 'scheduled', 'Prośba o naturalne kosmetyki', 7),
(27, 2, 'Jan Nowak', 'Barber', '2024-03-09', '11:30:00', '12:00:00', 'scheduled', 'Klasyczne cięcie', 6),
(28, 3, 'Ewa Zielińska', 'Strzyżenie damskie', '2024-03-09', '12:30:00', '13:00:00', 'scheduled', 'Styl romantyczny', 2),
(29, 4, 'Piotr Wiśniewski', 'Masaż relaksacyjny', '2024-03-09', '13:30:00', '14:30:00', 'scheduled', 'Prośba o ciszę', 7),
(30, 5, 'Maria Lewandowska', 'Manicure', '2024-03-10', '10:00:00', '10:45:00', 'scheduled', 'Naturalne zdobienia', 3),
(31, 6, 'Anna Malinowska', 'Barber', '2024-03-10', '11:00:00', '11:30:00', 'scheduled', 'Krótka broda', 6),
(32, 7, 'Piotr Wiśniewski', 'Strzyżenie męskie', '2024-03-10', '12:00:00', '12:30:00', 'scheduled', NULL, 2),
(33, 8, 'Katarzyna Nowicka', 'Masaż twarzy', '2024-03-10', '13:00:00', '14:00:00', 'scheduled', 'Wrażliwa cera', 7),
(34, 9, 'Tomasz Kowalczyk', 'Manicure', '2024-03-11', '09:00:00', '09:45:00', 'scheduled', 'Delikatne wykończenie', 3),
(35, 10, 'Michał Zieliński', 'Barber', '2024-03-11', '10:00:00', '10:30:00', 'scheduled', NULL, 6),
(36, 26, 'Anna Kowalska', 'Strzyżenie damskie', '2025-01-22', '10:00:00', '10:30:00', 'scheduled', 'test appoint', 1),
(37, 5, 'Anna Kowalska', 'Strzyżenie damskie', '2025-01-22', '12:00:00', '12:30:00', 'scheduled', 'test appoint2', 1),
(38, 1, 'paweleren', 'Strzyżenie', '2024-03-04', '09:00:00', '10:00:00', 'cancelled', 'Przykładowa notatka', 9),
(39, 1, 'paweleren', 'Strzyżenie', '2024-03-04', '13:00:00', '14:00:00', 'cancelled', 'Testowa rezerwacja', 9);

-- --------------------------------------------------------

--
-- Table structure for table `business`
--

CREATE TABLE `business` (
  `business_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `opening_hours` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL CHECK (json_valid(`opening_hours`))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `business`
--

INSERT INTO `business` (`business_id`, `name`, `opening_hours`) VALUES
(1, 'Cleopatra Beauty Salon', '{\"mon\": [\"09:00-17:00\"], \"tue\": [\"09:00-17:00\"], \"wed\": [\"09:00-17:00\"], \"thu\": [\"09:00-17:00\"], \"fri\": [\"09:00-15:00\"], \"sat\": [\"10:00-14:00\"], \"sun\": []}');

-- --------------------------------------------------------

--
-- Table structure for table `clients`
--

CREATE TABLE `clients` (
  `client_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `phone_number` varchar(15) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `notes` longtext DEFAULT NULL,
  `is_deleted` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `clients`
--

INSERT INTO `clients` (`client_id`, `name`, `phone_number`, `email`, `notes`, `is_deleted`) VALUES
(1, 'Anna Kowalska', '123456789', 'anna.kowalska@example.com', 'Preferuje wizyty poranne', 0),
(2, 'Jan Nowak', '987654321', 'jan.nowak@example.com', 'Alergia na niektóre kosmetyki', 0),
(3, 'Ewa Zielińska', '564738291', 'ewa.zielinska@example.com', 'Woli wizyty popołudniowe', 0),
(4, 'Piotr Wiśniewski', '456789123', 'piotr.wisniewski@example.com', NULL, 0),
(5, 'Maria Lewandowska', '102938475', 'maria.lewandowska@example.com', 'Stały klient, korzysta z rabatów', 0),
(6, 'Anna Malinowska', '123456789', 'anna.malinowska@example.com', 'Stała klientka, lubi poranne wizyty', 0),
(7, 'Piotr Wiśniewski', '987654321', 'piotr.wisniewski@example.com', 'Prośba o przypomnienia SMS o wizytach', 0),
(8, 'Katarzyna Nowicka', '123123123', 'katarzyna.nowicka@example.com', 'Lubi szybkie wizyty', 0),
(9, 'Tomasz Kowalczyk', '987987987', 'tomasz.kowalczyk@example.com', 'Woli popołudnia', 0),
(10, 'Michał Zieliński', '564564564', 'michal.zielinski@example.com', 'Prosi o ciche otoczenie', 0),
(11, 'Joanna Wiśniewska', '456456456', 'joanna.wisniewska@example.com', 'Stała klientka', 0),
(12, 'Karolina Lewandowska', '789789789', 'karolina.lewandowska@example.com', 'Prośba o przypomnienia e-mail', 0),
(13, 'Adam Kowalski', '333444555', 'adam.kowalski@example.com', 'Lubi rozmawiać podczas wizyt', 0),
(14, 'Paulina Nowak', '222333444', 'paulina.nowak@example.com', 'Alergia na lateks', 0),
(15, 'Marcin Malinowski', '111222333', 'marcin.malinowski@example.com', 'Chce wizyty z doświadczonymi pracownikami', 0),
(16, 'Ewelina Kamińska', '987111222', 'ewelina.kaminska@example.com', 'Preferuje masaże relaksacyjne', 0),
(17, 'Tomasz Lewandowski', '564987123', 'tomasz.lewandowski@example.com', 'Prosi o dyskrecję', 0),
(18, 'Agnieszka Zielińska', '123987456', 'agnieszka.zielinska@example.com', 'Nie lubi długich wizyt', 0),
(19, 'Paweł Kwiatkowski', '321654987', 'pawel.kwiatkowski@example.com', 'Stały klient, korzysta z pakietów', 0),
(20, 'Ewa Kowalczyk', '654789321', 'ewa.kowalczyk@example.com', 'Często odwołuje wizyty', 0),
(21, 'Mateusz Nowakowski', '111444777', 'mateusz.nowakowski@example.com', 'Preferuje wieczorne wizyty', 0),
(22, 'Monika Wiśniewska', '222555888', 'monika.wisniewska@example.com', 'Lubi kawę podczas wizyt', 0),
(23, 'Andrzej Kamiński', '333666999', 'andrzej.kaminski@example.com', 'Prośba o dłuższe masaże', 0),
(24, 'Magdalena Lewandowska', '444777000', 'magdalena.lewandowska@example.com', 'Woli kosmetyki ekologiczne', 0),
(25, 'Janina Zielińska', '555888111', 'janina.zielinska@example.com', 'Ma wrażliwą skórę', 0),
(26, 'Testowy Test', '511298027', 'test@test.com', 'ttestowa', 0);

-- --------------------------------------------------------

--
-- Table structure for table `employees`
--

CREATE TABLE `employees` (
  `employee_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `username` varchar(50) DEFAULT NULL,
  `phone_number` varchar(15) DEFAULT NULL,
  `role` enum('manager','worker') NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `working_hours` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL CHECK (json_valid(`working_hours`)),
  `specialties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL CHECK (json_valid(`specialties`)),
  `isDeleted` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `employees`
--

INSERT INTO `employees` (`employee_id`, `name`, `email`, `username`, `phone_number`, `role`, `password_hash`, `working_hours`, `specialties`, `isDeleted`) VALUES
(1, 'Anna Kowalska', 'anna.kowalska@example.com', 'anna.kowalska', '123456789', 'manager', '$2a$11$EnZKHWfBMC/v4lwZEiP5tevKAxtba9aEE08LsFpZO6PsGxbtvwUlq', '{\"mon\": [\"09:00-17:00\"], \"tue\": [\"09:00-17:00\"], \"wed\": [\"09:00-17:00\"], \"thu\": [\"09:00-17:00\"], \"fri\": [\"09:00-15:00\"]}', '[\"1\", \"2\", \"3\"]', 0),
(2, 'Jan Nowak', 'jan.nowak@example.com', 'jan.nowak', '987654321', 'worker', '$2a$11$H7GMBHQ3Jo.pmxns5PfXNuWgf70xmy0GJPozW7zyogzTFTxqvmNNC', '{\"mon\": [\"09:00-15:00\"], \"tue\": [\"10:00-16:00\"], \"wed\": [\"09:30-17:00\"], \"thu\": [\"09:00-15:00\"], \"fri\": [\"09:30-16:30\"]}', '[\"1\"]', 0),
(3, 'Ewa Zielińska', 'ewa.zielinska@example.com', 'ewa.zielińska', '564738291', 'worker', '$2a$11$.JM9n6lA/r4gyJufIZEND.DUepmrVd1G4njtdtfUvkrIF6fDPR7aS', '{\"mon\": [\"09:00-17:00\"], \"tue\": [\"09:00-17:00\"], \"wed\": [\"09:00-14:00\"], \"thu\": [\"10:00-16:00\"], \"fri\": [\"09:30-15:30\"]}', '[\"2\"]', 0),
(4, 'Jan Kowalski', 'jan.kowalski@example.com', 'jan.kowalski', '459314175', 'worker', '$2a$11$ZV/pkZvDrrvyJoj/cxSmx.1MsBSimsecRViSqEvquXGCV0n7hlv2K', '{\"mon\": [\"09:00-13:00\"], \"tue\": [\"11:00-17:00\"], \"wed\": [\"09:00-15:00\"], \"thu\": [\"10:00-17:00\"], \"fri\": [\"09:30-16:00\"]}', '[\"1\"]', 0),
(5, 'Maria Nowak', 'maria.nowak@example.com', 'maria.nowak', '729719767', 'manager', '$2a$11$86.MmHX55CUOF7s4EnJWl.Xl76sosxml1xKTNNUZDCft/iTMU/2Hu', '{\"mon\": [\"09:30-14:30\"], \"tue\": [\"10:00-15:00\"], \"wed\": [\"09:00-17:00\"], \"thu\": [\"09:30-16:30\"], \"fri\": [\"09:00-14:00\"]}', '[\"3\"]', 0),
(6, 'Adam Wiśniewski', 'adam.wisniewski@example.com', 'adam.wisniewski', '887180957', 'worker', '$2a$11$yB6JaDBN081U6z8X.1TjOuNmx61lm1NTZWT03fZc6B6YzX9UIK.TW', '{\"mon\": [\"09:00-17:00\"], \"tue\": [\"09:30-16:30\"], \"wed\": [\"10:00-17:00\"], \"thu\": [\"09:00-15:00\"], \"fri\": [\"10:00-16:00\"]}', '[\"1\"]', 0),
(7, 'Karolina Zielińska', 'karolina.zielinska@example.com', 'karolina.zielinska', '699280541', 'worker', '$2a$11$fvp3.I7LdRMlVXg7md3w8OenZLL.nw.efeHuyrwzyPdhJaIHtcm7O', '{\"mon\": [\"09:00-15:30\"], \"tue\": [\"09:30-16:30\"], \"wed\": [\"10:00-15:00\"], \"thu\": [\"09:00-16:30\"], \"fri\": [\"10:00-15:30\"]}', '[\"2\"]', 0),
(8, 'Piotr Lewandowski', 'piotr.lewandowski@example.com', 'piotr.lewandowski', '573597910', 'worker', '$2a$11$xQVVg334V9Iqb.jsxIC92uOsLeOIDLLrb42nXFT.bY5TBgE9nbIWu', '{\"mon\": [\"09:00-14:30\"], \"tue\": [\"09:30-17:00\"], \"wed\": [\"09:00-16:00\"], \"thu\": [\"10:00-15:00\"], \"fri\": [\"09:00-15:30\"]}', '[\"3\"]', 0),
(9, 'paweleren', 'paweleren@test.com', 'pawel.ereGGn', '123321123', 'worker', '$2a$11$Xz1GYftOv86J0oT6tHXAFe20ZNdw1uoextxC8b2FgpXOKwdXJsi0W', '{\"mon\": [\"09:00-17:00\"], \"tue\": [\"09:00-17:00\"], \"wed\": [\"09:00-14:00\"], \"thu\": [\"10:00-16:00\"], \"fri\": [\"09:30-15:30\"]}', '1', 1);

-- --------------------------------------------------------

--
-- Table structure for table `notifications`
--

CREATE TABLE `notifications` (
  `notification_id` int(11) NOT NULL,
  `client_id` int(11) NOT NULL,
  `type` enum('email','sms') NOT NULL,
  `content` text NOT NULL,
  `sent_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `notifications`
--

INSERT INTO `notifications` (`notification_id`, `client_id`, `type`, `content`, `sent_at`) VALUES
(1, 1, 'email', 'Przypomnienie o nadchodzącej wizycie na manicure', '2024-11-02 07:00:00'),
(2, 2, 'sms', 'Potwierdzenie rezerwacji na pedicure', '2024-11-02 08:00:00'),
(3, 3, 'email', 'Dziękujemy za skorzystanie z masażu twarzy', '2024-11-04 14:00:00'),
(4, 4, 'sms', 'Twoja rezerwacja na koloryzację włosów została anulowana', '2024-11-05 07:00:00'),
(5, 5, 'email', 'Przypomnienie o nadchodzącej wizycie na strzyżenie', '2024-11-04 09:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `reports`
--

CREATE TABLE `reports` (
  `report_id` int(11) NOT NULL,
  `type` varchar(50) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `file_path` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `reports`
--

INSERT INTO `reports` (`report_id`, `type`, `created_at`, `file_path`) VALUES
(1, 'Raport dzienny', '2024-11-02 17:00:00', '/reports/daily_report_2024_11_02.pdf'),
(2, 'Raport tygodniowy', '2024-11-06 17:00:00', '/reports/weekly_report_2024_11_06.pdf');

-- --------------------------------------------------------

--
-- Table structure for table `resources`
--

CREATE TABLE `resources` (
  `resource_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `quantity` int(11) NOT NULL,
  `unit` varchar(20) DEFAULT NULL,
  `reorder_level` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `resources`
--

INSERT INTO `resources` (`resource_id`, `name`, `quantity`, `unit`, `reorder_level`) VALUES
(1, 'Lakier do paznokci', 50, 'szt', 10),
(2, 'Szampon', 30, 'ml', 5),
(3, 'Farba do włosów - blond', 20, 'szt', 5),
(4, 'Krem do twarzy', 15, 'ml', 3),
(5, 'Mleczko do demakijażu', 25, 'ml', 5),
(6, 'Odżywka do włosów', 40, 'ml', 10),
(7, 'Pianka do włosów', 25, 'ml', 5),
(8, 'Masło do masażu', 20, 'ml', 5),
(9, 'Olej kokosowy', 15, 'ml', 3),
(10, 'Ręczniki jednorazowe', 200, 'szt', 50),
(11, 'Pędzelek do farbowania', 10, 'szt', 3),
(12, 'Folia do farbowania', 30, 'szt', 5),
(13, 'Lampa UV', 2, 'szt', 1),
(14, 'Pilniki do paznokci', 100, 'szt', 20),
(15, 'Zmywacz do paznokci', 50, 'ml', 10),
(16, 'Płatki kosmetyczne', 300, 'szt', 50),
(17, 'Sól do kąpieli', 15, 'ml', 5),
(18, 'Aromatyczny olejek lawendowy', 10, 'ml', 2),
(19, 'Chusteczki higieniczne', 200, 'szt', 50),
(20, 'Fartuch fryzjerski', 10, 'szt', 2);

-- --------------------------------------------------------

--
-- Table structure for table `servicecategories`
--

CREATE TABLE `servicecategories` (
  `category_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `servicecategories`
--

INSERT INTO `servicecategories` (`category_id`, `name`) VALUES
(1, 'Fryzjerstwo'),
(2, 'Masaże'),
(3, 'Kosmetyka');

-- --------------------------------------------------------

--
-- Table structure for table `services`
--

CREATE TABLE `services` (
  `service_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` text DEFAULT NULL,
  `duration` int(11) NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `category_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `services`
--

INSERT INTO `services` (`service_id`, `name`, `description`, `duration`, `price`, `category_id`) VALUES
(1, 'Strzyżenie damskie', 'Kompleksowe strzyżenie włosów damskich', 30, 80.00, 1),
(2, 'Strzyżenie męskie', 'Klasyczne strzyżenie włosów męskich', 30, 50.00, 1),
(3, 'Barber', 'Modelowanie i pielęgnacja zarostu', 30, 70.00, 1),
(4, 'Masaż relaksacyjny', 'Relaksujący masaż całego ciała', 60, 150.00, 2),
(5, 'Masaż sportowy', 'Masaż regeneracyjny dla sportowców', 60, 170.00, 2),
(6, 'Masaż twarzy', 'Relaksacyjny masaż twarzy i szyi', 60, 120.00, 2),
(7, 'Manicure', 'Pielęgnacja paznokci z malowaniem', 45, 100.00, 3),
(8, 'Manicure z zdobieniem', 'Manicure z dodatkowymi zdobieniami', 60, 120.00, 3);

-- --------------------------------------------------------

--
-- Table structure for table `vacations`
--

CREATE TABLE `vacations` (
  `vacation_id` int(11) NOT NULL,
  `employee_id` int(11) NOT NULL,
  `start_date` date NOT NULL,
  `end_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `vacations`
--

INSERT INTO `vacations` (`vacation_id`, `employee_id`, `start_date`, `end_date`) VALUES
(1, 2, '2024-03-01', '2024-03-07'),
(2, 3, '2024-03-10', '2024-03-10');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20241029181221_InitialCreate', '5.0.0');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `appointments`
--
ALTER TABLE `appointments`
  ADD PRIMARY KEY (`appointment_id`),
  ADD KEY `client_id` (`client_id`),
  ADD KEY `employee_id` (`employee_id`);

--
-- Indexes for table `business`
--
ALTER TABLE `business`
  ADD PRIMARY KEY (`business_id`);

--
-- Indexes for table `clients`
--
ALTER TABLE `clients`
  ADD PRIMARY KEY (`client_id`);

--
-- Indexes for table `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`employee_id`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `username` (`username`);

--
-- Indexes for table `notifications`
--
ALTER TABLE `notifications`
  ADD PRIMARY KEY (`notification_id`),
  ADD KEY `client_id` (`client_id`);

--
-- Indexes for table `reports`
--
ALTER TABLE `reports`
  ADD PRIMARY KEY (`report_id`);

--
-- Indexes for table `resources`
--
ALTER TABLE `resources`
  ADD PRIMARY KEY (`resource_id`);

--
-- Indexes for table `servicecategories`
--
ALTER TABLE `servicecategories`
  ADD PRIMARY KEY (`category_id`);

--
-- Indexes for table `services`
--
ALTER TABLE `services`
  ADD PRIMARY KEY (`service_id`),
  ADD KEY `category_id` (`category_id`);

--
-- Indexes for table `vacations`
--
ALTER TABLE `vacations`
  ADD PRIMARY KEY (`vacation_id`),
  ADD KEY `employee_id` (`employee_id`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `appointments`
--
ALTER TABLE `appointments`
  MODIFY `appointment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=40;

--
-- AUTO_INCREMENT for table `business`
--
ALTER TABLE `business`
  MODIFY `business_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `clients`
--
ALTER TABLE `clients`
  MODIFY `client_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT for table `employees`
--
ALTER TABLE `employees`
  MODIFY `employee_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `notifications`
--
ALTER TABLE `notifications`
  MODIFY `notification_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `reports`
--
ALTER TABLE `reports`
  MODIFY `report_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `resources`
--
ALTER TABLE `resources`
  MODIFY `resource_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `servicecategories`
--
ALTER TABLE `servicecategories`
  MODIFY `category_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `services`
--
ALTER TABLE `services`
  MODIFY `service_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `vacations`
--
ALTER TABLE `vacations`
  MODIFY `vacation_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `appointments`
--
ALTER TABLE `appointments`
  ADD CONSTRAINT `appointments_ibfk_1` FOREIGN KEY (`client_id`) REFERENCES `clients` (`client_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `appointments_ibfk_2` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`);

--
-- Constraints for table `notifications`
--
ALTER TABLE `notifications`
  ADD CONSTRAINT `notifications_ibfk_1` FOREIGN KEY (`client_id`) REFERENCES `clients` (`client_id`) ON DELETE CASCADE;

--
-- Constraints for table `services`
--
ALTER TABLE `services`
  ADD CONSTRAINT `services_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `servicecategories` (`category_id`);

--
-- Constraints for table `vacations`
--
ALTER TABLE `vacations`
  ADD CONSTRAINT `vacations_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
