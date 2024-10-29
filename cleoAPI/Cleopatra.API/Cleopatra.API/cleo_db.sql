-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 29, 2024 at 07:32 PM
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
  `notes` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `appointments`
--

INSERT INTO `appointments` (`appointment_id`, `client_id`, `employee_name`, `service`, `appointment_date`, `start_time`, `end_time`, `status`, `notes`) VALUES
(1, 1, 'Agnieszka Malinowska', 'Manicure', '2024-11-03', '10:00:00', '11:00:00', 'scheduled', 'Prosiła o jasny lakier'),
(2, 2, 'Katarzyna Kwiatkowska', 'Pedicure', '2024-11-03', '12:00:00', '13:15:00', 'scheduled', NULL),
(3, 3, 'Anna Sobczak', 'Masaż twarzy', '2024-11-04', '14:00:00', '14:30:00', 'completed', 'Pierwsza wizyta - prosiła o delikatność'),
(4, 4, 'Michał Nowicki', 'Koloryzacja włosów', '2024-11-05', '09:00:00', '11:00:00', 'cancelled', 'Przeniesione na inny termin'),
(5, 5, 'Łukasz Lewicki', 'Strzyżenie męskie', '2024-11-05', '15:00:00', '15:45:00', 'scheduled', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `clients`
--

CREATE TABLE `clients` (
  `client_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `phone_number` varchar(15) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `notes` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `clients`
--

INSERT INTO `clients` (`client_id`, `name`, `phone_number`, `email`, `notes`) VALUES
(1, 'Anna Kowalska', '123456789', 'anna.kowalska@example.com', 'Preferuje wizyty poranne'),
(2, 'Jan Nowak', '987654321', 'jan.nowak@example.com', 'Alergia na niektóre kosmetyki'),
(3, 'Ewa Zielińska', '564738291', 'ewa.zielinska@example.com', 'Woli wizyty popołudniowe'),
(4, 'Piotr Wiśniewski', '456789123', 'piotr.wisniewski@example.com', NULL),
(5, 'Maria Lewandowska', '102938475', 'maria.lewandowska@example.com', 'Stały klient, korzysta z rabatów');

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
(5, 'Mleczko do demakijażu', 25, 'ml', 5);

-- --------------------------------------------------------

--
-- Table structure for table `services`
--

CREATE TABLE `services` (
  `service_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `description` text DEFAULT NULL,
  `duration` int(11) NOT NULL,
  `price` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `services`
--

INSERT INTO `services` (`service_id`, `name`, `description`, `duration`, `price`) VALUES
(1, 'Manicure', 'Kompleksowy zabieg pielęgnacyjny paznokci dłoni', 60, 50.00),
(2, 'Pedicure', 'Zabieg pielęgnacyjny paznokci stóp', 75, 70.00),
(3, 'Masaż twarzy', 'Relaksacyjny masaż twarzy', 30, 40.00),
(4, 'Koloryzacja włosów', 'Profesjonalna koloryzacja z konsultacją stylisty', 120, 150.00),
(5, 'Strzyżenie męskie', 'Strzyżenie męskie wraz z modelowaniem', 45, 40.00);

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
  ADD KEY `client_id` (`client_id`);

--
-- Indexes for table `clients`
--
ALTER TABLE `clients`
  ADD PRIMARY KEY (`client_id`);

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
-- Indexes for table `services`
--
ALTER TABLE `services`
  ADD PRIMARY KEY (`service_id`);

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
  MODIFY `appointment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `clients`
--
ALTER TABLE `clients`
  MODIFY `client_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

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
  MODIFY `resource_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `services`
--
ALTER TABLE `services`
  MODIFY `service_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `appointments`
--
ALTER TABLE `appointments`
  ADD CONSTRAINT `appointments_ibfk_1` FOREIGN KEY (`client_id`) REFERENCES `clients` (`client_id`) ON DELETE CASCADE;

--
-- Constraints for table `notifications`
--
ALTER TABLE `notifications`
  ADD CONSTRAINT `notifications_ibfk_1` FOREIGN KEY (`client_id`) REFERENCES `clients` (`client_id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
