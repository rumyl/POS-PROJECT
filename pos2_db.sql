-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 17, 2023 at 07:39 AM
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
-- Database: `pos2_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_category`
--

CREATE TABLE `tbl_category` (
  `cat_id` int(11) NOT NULL,
  `cat_name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_category`
--

INSERT INTO `tbl_category` (`cat_id`, `cat_name`) VALUES
(1, 'FRAPPE'),
(2, 'COFFEE'),
(3, 'BURGER'),
(4, 'CAKE'),
(5, 'PIZZA'),
(6, 'MILK TEA');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_menu`
--

CREATE TABLE `tbl_menu` (
  `menu_id` int(11) NOT NULL,
  `cat_id` int(11) NOT NULL,
  `menu_name` varchar(100) NOT NULL,
  `price` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_menu`
--

INSERT INTO `tbl_menu` (`menu_id`, `cat_id`, `menu_name`, `price`) VALUES
(1, 3, 'CHEESE BURGER', 34),
(2, 3, 'CHICKEN BURGER', 50),
(3, 4, 'STRAWBERRY - CAKE', 200),
(4, 4, 'BLACK FOREST', 250),
(5, 2, 'LATTE', 100),
(6, 2, 'DARK COFFEE', 90),
(7, 1, 'MANGO', 80),
(8, 6, 'TARO', 100),
(9, 6, 'COOKIESnCREAM', 120),
(10, 6, 'CHOCO', 90),
(11, 5, 'HAWAIAN', 110),
(12, 5, 'CHEESE', 110),
(13, 1, 'STRAWBERRY', 100);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_sales`
--

CREATE TABLE `tbl_sales` (
  `sales_id` int(11) NOT NULL,
  `tendered` double NOT NULL,
  `total_amount` double NOT NULL,
  `changed` double NOT NULL,
  `customer` varchar(100) NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_sales`
--

INSERT INTO `tbl_sales` (`sales_id`, `tendered`, `total_amount`, `changed`, `customer`, `user_id`) VALUES
(1, 1000, 1000, 0, 'Ace', 1),
(2, 1000, 1000, 0, 'lyn', 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_sales_details`
--

CREATE TABLE `tbl_sales_details` (
  `s_id` int(11) NOT NULL,
  `sales_id` int(11) NOT NULL,
  `menu_id` int(11) NOT NULL,
  `qty` int(11) NOT NULL,
  `price` double NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_sales_details`
--

INSERT INTO `tbl_sales_details` (`s_id`, `sales_id`, `menu_id`, `qty`, `price`, `user_id`) VALUES
(1, 1, 1, 2, 34, 1),
(2, 1, 4, 1, 250, 1),
(3, 1, 5, 1, 100, 1),
(4, 2, 2, 1, 50, 1),
(5, 2, 5, 1, 100, 1),
(6, 2, 8, 1, 100, 1),
(7, 2, 10, 1, 90, 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_temp`
--

CREATE TABLE `tbl_temp` (
  `temp_id` int(11) NOT NULL,
  `menu_id` int(11) NOT NULL,
  `qty` int(11) NOT NULL,
  `price` double NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_temp`
--

INSERT INTO `tbl_temp` (`temp_id`, `menu_id`, `qty`, `price`, `user_id`) VALUES
(33, 10, 1, 90, 1),
(34, 8, 1, 100, 1),
(35, 5, 1, 100, 1),
(36, 2, 1, 50, 1);

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
(1, 'Rumyl James', 'rumyl', '123');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_category`
--
ALTER TABLE `tbl_category`
  ADD PRIMARY KEY (`cat_id`);

--
-- Indexes for table `tbl_menu`
--
ALTER TABLE `tbl_menu`
  ADD PRIMARY KEY (`menu_id`);

--
-- Indexes for table `tbl_sales`
--
ALTER TABLE `tbl_sales`
  ADD PRIMARY KEY (`sales_id`);

--
-- Indexes for table `tbl_sales_details`
--
ALTER TABLE `tbl_sales_details`
  ADD PRIMARY KEY (`s_id`);

--
-- Indexes for table `tbl_temp`
--
ALTER TABLE `tbl_temp`
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
-- AUTO_INCREMENT for table `tbl_category`
--
ALTER TABLE `tbl_category`
  MODIFY `cat_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `tbl_menu`
--
ALTER TABLE `tbl_menu`
  MODIFY `menu_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `tbl_sales`
--
ALTER TABLE `tbl_sales`
  MODIFY `sales_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `tbl_sales_details`
--
ALTER TABLE `tbl_sales_details`
  MODIFY `s_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `tbl_temp`
--
ALTER TABLE `tbl_temp`
  MODIFY `temp_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=37;

--
-- AUTO_INCREMENT for table `tbl_users`
--
ALTER TABLE `tbl_users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
