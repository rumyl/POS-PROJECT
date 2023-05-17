-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 17, 2023 at 07:51 AM
-- Server version: 10.4.24-MariaDB
-- PHP Version: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `sample_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_products`
--

CREATE TABLE `tbl_products` (
  `product_id` int(11) NOT NULL,
  `product_name` varchar(100) NOT NULL,
  `barcode` varchar(100) NOT NULL,
  `cost` double NOT NULL,
  `srp` double NOT NULL,
  `stocks` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_products`
--

INSERT INTO `tbl_products` (`product_id`, `product_name`, `barcode`, `cost`, `srp`, `stocks`) VALUES
(1, 'Gin', '123', 40, 50, 102),
(2, 'Red Horse', '1234', 40, 50, 100),
(3, 'Tanduay', '202300004', 40, 60, 92),
(4, 'Mucho', '202300003', 55, 70, 11),
(5, 'Mighty', '3333', 5, 8, 100),
(6, 'rj 45', '202300001', 2, 5, 63),
(7, 'Cord', '202300002', 8, 5, 80),
(8, 'Pilot Pentel Pen', '4902505088827', 40, 60, 13),
(9, 'FlexStick - Ballpen', '8935001880141', 4, 6, 98),
(10, 'Nature Spring - 500ml', '4800049720114', 10, 15, 87),
(11, 'Avocado - Hairpin', '2000010076161', 10, 12, 96),
(12, 'Maye- Toothbrush', '6972919086830', 80, 100, 97);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_stockin`
--

CREATE TABLE `tbl_stockin` (
  `stockin_id` int(11) NOT NULL,
  `purchase_date` date NOT NULL,
  `supplier` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_stockin`
--

INSERT INTO `tbl_stockin` (`stockin_id`, `purchase_date`, `supplier`) VALUES
(1, '2023-05-15', 'san Miguel'),
(2, '2023-05-16', 'San Juan'),
(3, '2023-05-16', 'Glenn Store'),
(4, '2023-05-16', 'MARIAMIE'),
(5, '2023-05-17', 'tope');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_stockin_details`
--

CREATE TABLE `tbl_stockin_details` (
  `s_id` int(11) NOT NULL,
  `stockin_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `qty` int(11) NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_stockin_details`
--

INSERT INTO `tbl_stockin_details` (`s_id`, `stockin_id`, `product_id`, `qty`, `user_id`) VALUES
(1, 1, 3, 10, 6),
(2, 1, 4, 5, 6),
(3, 2, 1, 1, 6),
(4, 2, 2, 5, 6),
(5, 2, 3, 6, 6),
(6, 2, 4, 10, 6),
(7, 3, 6, 4, 4),
(8, 3, 7, 4, 4),
(9, 4, 8, 10, 4),
(10, 5, 1, 100, 4),
(11, 5, 2, 100, 4),
(12, 5, 3, 96, 4),
(13, 5, 5, 100, 4);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_stockout`
--

CREATE TABLE `tbl_stockout` (
  `stockout_id` int(11) NOT NULL,
  `tendered` double NOT NULL,
  `total_amount` double NOT NULL,
  `changed` double NOT NULL,
  `customer` varchar(100) NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_stockout`
--

INSERT INTO `tbl_stockout` (`stockout_id`, `tendered`, `total_amount`, `changed`, `customer`, `user_id`) VALUES
(1, 20, 15, 5, 'Rafael  - Paid', 4),
(2, 10, 10, 0, 'Diane', 4),
(3, 20, 15, 5, 'Neil -paid', 4),
(4, 20, 15, 5, 'Tenorio - paid', 4),
(5, 50, 30, 20, 'Jess/Parce - paid', 4),
(6, 10, 10, 0, 'Randy - paid', 4),
(7, 50, 15, 35, 'Kritz - Paid', 4),
(8, 20, 15, 5, 'Jahren - Paid', 4),
(9, 15, 15, 0, 'toji - utang', 4),
(10, 20, 15, 5, 'kamil - Paid', 4),
(11, 50, 15, 35, 'Maigue - Paid', 4),
(12, 20, 15, 5, 'alberto-paid', 4),
(13, 20, 20, 0, 'Jil - Paid', 4),
(14, 20, 5, 15, 'tenorio - paid', 4),
(15, 5, 5, 0, 'rafael - paid', 4),
(16, 15, 15, 0, 'Jayrel - utang', 4),
(17, 5, 5, 0, 'kamil - paid', 4),
(18, 5, 5, 0, 'Parce - paid', 4),
(19, 500, 230, 270, 'james', 4),
(20, 500, 230, 270, 'mariamie', 4),
(21, 500, 230, 270, 'mariamie', 4),
(22, 500, 340, 160, 'Mariamie', 4),
(23, 500, 360, 140, 'Mayang', 4),
(24, 500, 340, 160, 'Mayang', 4),
(25, 300, 290, 10, 'mayang', 4),
(26, 500, 230, 270, 'James', 4),
(27, 500, 230, 270, '', 4),
(28, 500, 230, 270, '', 4),
(29, 200, 180, 20, 'Hello', 4),
(30, 400, 360, 40, 'JA', 4),
(31, 100, 60, 40, 'Jefferson', 4),
(32, 200, 140, 60, 'James', 4),
(33, 200, 145, 55, 'Gwapo', 4),
(34, 500, 240, 260, 'Gwapo', 4),
(35, 200, 162, 38, 'Ace', 4),
(36, 50, 36, 14, 'Ace', 4),
(37, 1000, 457, 543, 'Ace', 4),
(38, 500, 390, 110, 'Maye', 4);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_stockout_details`
--

CREATE TABLE `tbl_stockout_details` (
  `s_id` int(11) NOT NULL,
  `stockout_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `qty` int(11) NOT NULL,
  `price` double NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_stockout_details`
--

INSERT INTO `tbl_stockout_details` (`s_id`, `stockout_id`, `product_id`, `qty`, `price`, `user_id`) VALUES
(1, 1, 6, 2, 5, 4),
(2, 1, 7, 1, 5, 4),
(3, 2, 7, 2, 5, 4),
(4, 3, 6, 2, 5, 4),
(5, 3, 7, 1, 5, 4),
(6, 4, 6, 2, 5, 4),
(7, 4, 7, 1, 5, 4),
(8, 5, 6, 4, 5, 4),
(9, 5, 7, 2, 5, 4),
(10, 6, 6, 1, 5, 4),
(11, 6, 7, 1, 5, 4),
(12, 7, 6, 2, 5, 4),
(13, 7, 7, 1, 5, 4),
(14, 8, 6, 2, 5, 4),
(15, 8, 7, 1, 5, 4),
(16, 9, 6, 2, 5, 4),
(17, 9, 7, 1, 5, 4),
(18, 10, 6, 2, 5, 4),
(19, 10, 7, 1, 5, 4),
(20, 11, 6, 2, 5, 4),
(21, 11, 7, 1, 5, 4),
(22, 12, 6, 2, 5, 4),
(23, 12, 7, 1, 5, 4),
(24, 13, 6, 3, 5, 4),
(25, 13, 7, 1, 5, 4),
(26, 14, 6, 1, 5, 4),
(27, 15, 6, 1, 5, 4),
(28, 16, 6, 2, 5, 4),
(29, 16, 7, 1, 5, 4),
(30, 17, 6, 1, 5, 4),
(31, 18, 6, 1, 5, 4),
(32, 19, 1, 1, 50, 4),
(33, 19, 2, 1, 50, 4),
(34, 19, 3, 1, 60, 4),
(35, 19, 4, 1, 70, 4),
(36, 20, 1, 1, 50, 4),
(37, 20, 2, 1, 50, 4),
(38, 20, 3, 1, 60, 4),
(39, 20, 4, 1, 70, 4),
(40, 21, 1, 1, 50, 4),
(41, 21, 2, 1, 50, 4),
(42, 21, 3, 1, 60, 4),
(43, 21, 4, 1, 70, 4),
(44, 22, 1, 2, 50, 4),
(45, 22, 2, 1, 50, 4),
(46, 22, 3, 2, 60, 4),
(47, 22, 4, 1, 70, 4),
(48, 23, 1, 1, 50, 4),
(49, 23, 2, 1, 50, 4),
(50, 23, 3, 2, 60, 4),
(51, 23, 4, 2, 70, 4),
(52, 24, 1, 2, 50, 4),
(53, 24, 2, 1, 50, 4),
(54, 24, 3, 2, 60, 4),
(55, 24, 4, 1, 70, 4),
(56, 25, 1, 2, 50, 4),
(57, 25, 3, 2, 60, 4),
(58, 25, 4, 1, 70, 4),
(59, 26, 1, 2, 50, 4),
(60, 26, 3, 1, 60, 4),
(61, 26, 4, 1, 70, 4),
(62, 27, 1, 2, 50, 4),
(63, 27, 3, 1, 60, 4),
(64, 27, 4, 1, 70, 4),
(65, 28, 1, 2, 50, 4),
(66, 28, 3, 1, 60, 4),
(67, 28, 4, 1, 70, 4),
(68, 29, 1, 1, 50, 4),
(69, 29, 3, 1, 60, 4),
(70, 29, 4, 1, 70, 4),
(71, 30, 8, 6, 60, 4),
(72, 31, 8, 1, 60, 4),
(73, 32, 3, 1, 60, 4),
(74, 32, 4, 1, 70, 4),
(75, 32, 6, 1, 5, 4),
(76, 32, 7, 1, 5, 4),
(77, 33, 3, 1, 60, 4),
(78, 33, 4, 1, 70, 4),
(79, 33, 6, 2, 5, 4),
(80, 33, 7, 1, 5, 4),
(81, 34, 3, 4, 60, 4),
(82, 35, 9, 2, 6, 4),
(83, 35, 10, 10, 15, 4),
(84, 36, 11, 3, 12, 4),
(85, 37, 3, 1, 60, 4),
(86, 37, 4, 1, 70, 4),
(87, 37, 10, 1, 15, 4),
(88, 37, 11, 1, 12, 4),
(89, 37, 12, 3, 100, 4),
(90, 38, 3, 1, 60, 4),
(91, 38, 4, 4, 70, 4),
(92, 38, 6, 2, 5, 4),
(93, 38, 7, 2, 5, 4),
(94, 38, 10, 2, 15, 4);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_temp1`
--

CREATE TABLE `tbl_temp1` (
  `temp_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `qty` int(11) NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_temp2`
--

CREATE TABLE `tbl_temp2` (
  `temp_id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `qty` int(11) NOT NULL,
  `price` double NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_users`
--

CREATE TABLE `tbl_users` (
  `user_id` int(11) NOT NULL,
  `fullname` varchar(100) NOT NULL,
  `username` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_users`
--

INSERT INTO `tbl_users` (`user_id`, `fullname`, `username`, `password`) VALUES
(4, 'rumyls', 'james', '123'),
(5, 'JAYREL HILARIO', 'jayrel', '123'),
(6, 'TOPE GWAPO', 'tope', '123');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_products`
--
ALTER TABLE `tbl_products`
  ADD PRIMARY KEY (`product_id`);

--
-- Indexes for table `tbl_stockin`
--
ALTER TABLE `tbl_stockin`
  ADD PRIMARY KEY (`stockin_id`);

--
-- Indexes for table `tbl_stockin_details`
--
ALTER TABLE `tbl_stockin_details`
  ADD PRIMARY KEY (`s_id`);

--
-- Indexes for table `tbl_stockout`
--
ALTER TABLE `tbl_stockout`
  ADD PRIMARY KEY (`stockout_id`);

--
-- Indexes for table `tbl_stockout_details`
--
ALTER TABLE `tbl_stockout_details`
  ADD PRIMARY KEY (`s_id`);

--
-- Indexes for table `tbl_temp1`
--
ALTER TABLE `tbl_temp1`
  ADD PRIMARY KEY (`temp_id`);

--
-- Indexes for table `tbl_temp2`
--
ALTER TABLE `tbl_temp2`
  ADD PRIMARY KEY (`temp_id`);

--
-- Indexes for table `tbl_users`
--
ALTER TABLE `tbl_users`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_products`
--
ALTER TABLE `tbl_products`
  MODIFY `product_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `tbl_stockin`
--
ALTER TABLE `tbl_stockin`
  MODIFY `stockin_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `tbl_stockin_details`
--
ALTER TABLE `tbl_stockin_details`
  MODIFY `s_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `tbl_stockout`
--
ALTER TABLE `tbl_stockout`
  MODIFY `stockout_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;

--
-- AUTO_INCREMENT for table `tbl_stockout_details`
--
ALTER TABLE `tbl_stockout_details`
  MODIFY `s_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=95;

--
-- AUTO_INCREMENT for table `tbl_temp1`
--
ALTER TABLE `tbl_temp1`
  MODIFY `temp_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `tbl_temp2`
--
ALTER TABLE `tbl_temp2`
  MODIFY `temp_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=171;

--
-- AUTO_INCREMENT for table `tbl_users`
--
ALTER TABLE `tbl_users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
