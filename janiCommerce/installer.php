<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title>janiCommerce - Installer</title>
    </head>
    <body>
        <?php
            //Luodaan kaupan asetukset sisältävä tiedosto
            if (isset($_POST['store_name'])) {
                //Jos tiedosto on jo olemassa niin kerrotaan siitä käyttäjälle
                if (file_exists('config.php')) {
                    echo 'STORE CONFIGURATION FILE ALREADY EXISTS!<br>';
                }
                //Jos tiedostoa ei löydy niin luodaan sellainen
                else {
                    $handle = fopen('config.php', 'x');
                    //Jos luonti epäonnistuu niin ilmoitetaan siitä käyttäjälle
                    if ($handle == false) {
                        echo 'FAILED TO CREATE THE STORE CONFIGURATION FILE! CHECK THAT YOU HAVE PERMISSIONS TO CREATE, READ AND WRITE FILES!<br>';
                    }
                    //Jos tiedosto saadaan luotua niin tallennetaan asetukset tiedostoon
                    else {
                        $text = '<?php
    $store["name"] = ' . "'" . $_POST['store_name'] . "'" . ';
    $store["address"] = ' .  "'" . $_POST['store_address'] . "'" . ';
    $store["postalcode"] = ' . "'" .  $_POST['store_postalcode'] . "'" . ';
    $store["city"] = ' . "'" .  $_POST['store_city'] . "'" . ';
    $store["email"] = ' . "'" .  $_POST['store_email'] . "'" . ';
    $store["phone"] = ' . "'" . $_POST['store_phone'] .  "'" . ';
?>';
                        //Jos tallennus epäonnistuu niin ilmoitetaan siitä käyttäjälle
                        if (fwrite($handle, $text) == false) {
                            echo 'COULD NOT WRITE THE STORE CONFIGURATION FILE! CHECK THAT YOU HAVE PERMISSIONS TO CREATE, READ AND WRITE FILES!<br>';
                        }
                        //Ja jos onnistutaan niin ilmoitetaan siitäkin
                        else {
                            echo 'STORE CONFIGURATION FILE CREATED SUCCESSFULLY!<br>';
                            //Luodaan tietokanta-asetukset sisältävä tiedosto ja sen jälkeen luodaan tietokanta ja admin käyttäjä
                            if (isset($_POST['database_ip']) && isset($_POST['database_user']) && isset($_POST['database_password']) && isset($_POST['database_prefix'])) {
                                $ip = $_POST['database_ip'];
                                $user= $_POST['database_user'];
                                $password = $_POST['database_password'];
                                $database =$_POST['database_name'];
                                $prefix = $_POST['database_prefix'];
                                $connection = mysqli_connect("$ip", "$user", "$password", "$database");
                                //Jos tietokantaan ei saada yhteyttä niin kerrotaan siitä
                                if (!$connection) {
                                    echo 'UNABLE TO CONNECT TO THE DATABASE!<br>';
                                }
                                //Jos onnistutaan niin luodaan tietokanta-asetukset sisältävä tiedosto
                                else {
                                    mysqli_set_charset($connection, "utf8");//Asetetaan UTF-8 merkistö niin että kaikki merkit toimivat
                                    echo 'SUCCESSFULLY CONNECTED TO THE DATABASE!<br>';
                                //Jos tiedosto on jo olemassa niin kerrotaan siitä käyttäjälle
                                    if (file_exists('database/config.php')) {
                                        echo 'DATABASE CONFIGURATION FILE ALREADY EXISTS!<br>';
                                    }
                                    //Jos tiedostoa ei löydy niin luodaan sellainen
                                    else {
                                        $handle = fopen('database/config.php', 'x');
                                        //Jos luonti epäonnistuu niin ilmoitetaan siitä käyttäjälle
                                        if ($handle == false) {
                                            echo 'FAILED TO CREATE THE DATABASE CONFIGURATION FILE! CHECK THAT YOU HAVE PERMISSIONS TO CREATE, READ AND WRITE FILES!<br>';
                                        }
                                        //Jos tiedosto saadaan luotua niin tallennetaan asetukset tiedostoon
                                        else {
                                            $text = '<?php
    $database["ip"] = ' . "'" . $ip . "'" . ';
    $database["table_prefix"] = ' . "'" . $prefix . "'" . ';
    $database["name"] = ' . "'" . $database . "'" . ';
    $database["user"] = ' . "'" . $user . "'" . ';
    $database["password"] = ' . "'" . $password . "'" . ';
?>';
                                            //Jos tallennus epäonnistuu niin ilmoitetaan siitä käyttäjälle
                                            if (fwrite($handle, $text) == false) {
                                                echo 'COULD NOT WRITE THE DATABASE CONFIGURATION FILE CHECK THAT YOU HAVE PERMISSIONS TO CREATE, READ AND WRITE FILES!<br>';
                                            }
                                            //Ja jos onnistutaan niin ilmoitetaan siitäkin
                                            else {
                                                $orderrow = $prefix . 'OrderRow';
                                                $order = $prefix . 'Order';
                                                $user = $prefix . 'User';
                                                $product = $prefix . 'Product';
                                                $supplier = $prefix . 'Supplier';
                                                $product_supplier_FK = $product . '_' . $supplier . '_FK';
                                                $order_user_FK = $order . '_' . $user . '_FK';
                                                $orderrow_order_FK = $orderrow . '_' . $order . '_FK';
                                                $orderrow_product_FK = $orderrow . '_' . $product . '_FK';
                                                echo 'DATABASE CONFIGURATION FILE CREATED SUCCESSFULLY!<br>';
                                                //Ajetaan tietokantataulujen luontiscripti
                                                $result = mysqli_multi_query($connection,
                                                "DROP TABLE IF EXISTS $orderrow;
                                                DROP TABLE IF EXISTS $order;
                                                DROP TABLE IF EXISTS $user;
                                                DROP TABLE IF EXISTS $product;
                                                DROP TABLE IF EXISTS $supplier;
                                                
                                                CREATE TABLE $supplier(
                                                id INT AUTO_INCREMENT PRIMARY KEY,
                                                supplier_name VARCHAR(50) NOT NULL,
                                                supplier_address VARCHAR(50),
                                                supplier_postalcode VARCHAR(10),
                                                supplier_city VARCHAR(40),
                                                supplier_email VARCHAR(80)
                                                )ENGINE=INNODB;
                                                
                                                CREATE TABLE $product(
                                                id INT AUTO_INCREMENT PRIMARY KEY,
                                                product_id VARCHAR(30) NOT NULL UNIQUE,
                                                product_name VARCHAR(50) NOT NULL,
                                                product_desc TEXT(500),
                                                sellprice DECIMAL(9,2) NOT NULL,
                                                buyprice DECIMAL (9,2) NOT NULL,
                                                ean_code VARCHAR(30) UNIQUE,
                                                visible BOOLEAN NOT NULL DEFAULT TRUE,
                                                stock INT NOT NULL DEFAULT 0,
                                                supplier INT,
                                                supplier_product_id VARCHAR(30),
                                                CONSTRAINT $product_supplier_FK FOREIGN KEY (supplier) REFERENCES $supplier(id)
                                                )ENGINE=INNODB;
                                                
                                                CREATE TABLE $user(
                                                id INT AUTO_INCREMENT PRIMARY KEY,
                                                username VARCHAR(40) NOT NULL UNIQUE,
                                                userpassword VARCHAR(255) NOT NULL,
                                                first_name VARCHAR(50),
                                                last_name VARCHAR(50),
                                                user_address VARCHAR(50),
                                                user_postalcode VARCHAR(10),
                                                user_city VARCHAR(40),
                                                user_email VARCHAR(80) NOT NULL,
                                                user_phone VARCHAR(20),
                                                superuser BOOLEAN NOT NULL DEFAULT FALSE,
                                                superuser_products BOOLEAN NOT NULL DEFAULT FALSE,
                                                superuser_suppliers BOOLEAN NOT NULL DEFAULT FALSE,
                                                superuser_orders BOOLEAN NOT NULL DEFAULT FALSE,
                                                superuser_users BOOLEAN NOT NULL DEFAULT FALSE,
                                                administrator BOOLEAN NOT NULL DEFAULT FALSE
                                                )ENGINE=INNODB;
                                                
                                                CREATE TABLE $order(
                                                id INT AUTO_INCREMENT PRIMARY KEY,
                                                user_id INT,
                                                order_datetime DATETIME NOT NULL,
                                                delivery_firstname VARCHAR(50) NOT NULL,
                                                delivery_lastname VARCHAR(50) NOT NULL,
                                                delivery_address VARCHAR(50) NOT NULL,
                                                delivery_postalcode VARCHAR(10) NOT NULL,
                                                delivery_city VARCHAR(40) NOT NULL,
                                                delivery_email VARCHAR(80) NOT NULL,
                                                delivery_phone VARCHAR(20),
                                                CONSTRAINT $order_user_FK FOREIGN KEY (user_id) REFERENCES $user(id)
                                                )ENGINE=INNODB;
                                                
                                                CREATE TABLE $orderrow(
                                                id INT AUTO_INCREMENT PRIMARY KEY,
                                                order_id INT NOT NULL,
                                                product_id INT NOT NULL,
                                                quantity INT NOT NULL,
                                                CONSTRAINT $orderrow_order_FK FOREIGN KEY (order_id) REFERENCES $order(id),
                                                CONSTRAINT $orderrow_product_FK FOREIGN KEY (product_id) REFERENCES $product(id)
                                                )ENGINE=INNODB;");
                                                $errorlist = mysqli_error_list($connection);
                                                //Jos taulujen luonti ei onnistu niin ilmoitetaan siitä käyttäjälle.
                                                if (sizeof($errorlist) > 0) {
                                                    echo 'THERE WAS AN ERROR WHILE CREATING THE DATABASE TABLES! MAKE SURE YOUR DATABASE USER HAS ALL PERMISSIONS TO YOUR DATABASE!<br>';
                                                    foreach ($errorlist as $error) {
                                                        echo $error['error'] . '<br>';
                                                    }
                                                }
                                                //Jos taulut saadaan luotua niin ilmoitetaan siitäkin
                                                else {
                                                    echo 'TABLES CREATED SUCCESSFULLY!<br>';
                                                    //Vapautetaan tietokannan resurssit
                                                    while (mysqli_next_result($connection)) {;}
                                                    //Luodaan admin käyttäjä Users-tauluun
                                                    $username = $_POST['username'];
                                                    $password = password_hash($_POST['password'], PASSWORD_DEFAULT);//Salataan salasana
                                                    $firstname = $_POST['firstname'];
                                                    $lastname = $_POST['lastname'];
                                                    $address = $_POST['address'];
                                                    $postalcode = $_POST['postalcode'];
                                                    $city = $_POST['city'];
                                                    $email = $_POST['email'];
                                                    $phone = $_POST['phone'];
                                                    mysqli_query($connection, "INSERT INTO $user(username,userpassword,first_name,last_name,user_address,user_postalcode,user_city,user_email,user_phone,
                                                        superuser,superuser_products,superuser_suppliers,superuser_orders,superuser_users, administrator)
                                                        VALUES ('$username','$password','$firstname','$lastname','$address','$postalcode','$city','$email','$phone',true,true,true,true,true,true)");//Ajetaan kysely
                                                    if ($result == false) {
                                                        echo 'SOMETHING WENT WRONG WHILE CREATING THE ADMIN USER! MAKE SURE YOUR DATABASE USER HAS ALL PERMISSIONS TO YOUR DATABASE!<br>';
                                                        echo 'ERROR MESSAGE: ' . mysqli_error($connection) . '<br>';
                                                    }
                                                    else {
                                                        echo '<p>ADMIN USER CREATED SUCCESSFULLY!<br>
                                                        <a href="login.php">CLICK HERE TO LOGIN TO YOUR ACCOUNT AND START USING YOUR STORE!</a><br>
                                                        NOTE! IT IS RECOMMENDED TO DELETE THE installer.php FILE FROM THE PUBLIC HTML DIRECTORY BEFORE PUBLISHING THE STORE ON THE INTERNET!</p>';
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        ?>
        <h1>Welcome to the janiCommerce Installer</h1>
        <p>
            Fill in all the details below to start creating your very own janiCommerce web store!
        </p>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            <strong>Fill in the details of your store:</strong><br>
            Store name:<input type="text" name="store_name" maxlength="50" placeholder="MAX 50 CHARACTERS" required><br>
            Address:<input type="text" name="store_address" maxlength="50" placeholder="MAX 50 CHARACTERS" required><br>
            Postal Code:<input type="text" name="store_postalcode" maxlength="10" placeholder="MAX 10 CHARACTERS" required><br>
            City:<input type="text" name="store_city" maxlength="40" placeholder="MAX 40 CHARACTERS" required><br>
            Email:<input type="email" name="store_email" maxlength="80" placeholder="MAX 80 CHARACTERS" required><br>
            Phone:<input type="text" name="store_phone" maxlength="20" placeholder="MAX 20 CHARACTERS" required><br><br>
            <strong>Set the database information:</strong><br>
            IP-Address:<input type="text" name="database_ip" maxlength="20" required><br>
            Database Name:<input type="text" name="database_name" maxlength="50" required><br>
            Database User:<input type="text" name="database_user" maxlength="50" placeholder="MAX 50 CHARACTERS" required><br>
            Password:<input type="password" name="database_password" maxlength="40" placeholder="MAX 40 CHARACTERS" required><br>
            Table Prefix:<input type="text" name="database_prefix" maxlength="30" value="jc_" required><br><br>
            <strong>Set the web store administrators information. Username, password and email are required and everything else is optional:</strong><br>
            Username:<br><input type="text" name="username" maxlength="40" placeholder="MAX 40 CHARACTERS" required><br>
            Password:<br><input type="password" name="password" maxlength="40" placeholder="MAX 40 CHARACTERS" required><br>
            First Name:<br><input type="text" name="firstname" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Last Name:<br><input type="text" name="lastname" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Address:<br><input type="text" name="address" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Postal Code:<br><input type="text" name="postalcode" maxlength="10" placeholder="MAX 10 CHARACTERS"><br>
            City:<br><input type="text" name="city" maxlength="40" placeholder="MAX 40 CHARACTERS"><br>
            Email:<br><input type="text" name="email" maxlength="80" placeholder="MAX 80 CHARACTERS" required><br>
            Phone:<br><input type="text" name="phone" maxlength="20" placeholder="MAX 20 CHARACTERS"><br><br>
            <input type="submit" value="Install">   
        </form>
        <?php

        ?>
    </body>
</html>